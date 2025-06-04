using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace famillytalk
{
    public class Server
    {
        private readonly TcpListener _listener;
        private bool _isRunning;

        private readonly List<TcpClient> _clients = new();
        private readonly HashSet<string> _activeClientIPs = new();

        public event Action<string>? OnMessageReceived;

        public Server(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port);
        }

        public void StartListening()
        {
            _listener.Start();
            _isRunning = true;
            Task.Run(() => AcceptClientsAsync());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();

            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    client.Close();
                }
                _clients.Clear();
                _activeClientIPs.Clear();
            }
        }

        private async Task AcceptClientsAsync()
        {
            while (_isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    lock (_clients)
                    {
                        _clients.Add(client);
                    }
                    _ = HandleClientAsync(client);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AcceptClientsAsync : {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            string? clientIP = (client.Client.RemoteEndPoint as IPEndPoint)?.Address.ToString();

            if (clientIP != null)
            {
                lock (_activeClientIPs)
                {
                    if (_activeClientIPs.Contains(clientIP))
                    {
                        client.Close();
                        lock (_clients) _clients.Remove(client);
                        return;
                    }
                    else
                    {
                        _activeClientIPs.Add(clientIP);
                    }
                }
            }

            var stream = client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (_isRunning)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    BroadcastMessage(message, client);

                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur HandleClientAsync ({clientIP}): {ex.Message}");
            }
            finally
            {
                if (clientIP != null)
                {
                    lock (_activeClientIPs)
                    {
                        _activeClientIPs.Remove(clientIP);
                    }
                }
                lock (_clients)
                {
                    _clients.Remove(client);
                }
                client.Close();
            }
        }

        private void BroadcastMessage(string message, TcpClient excludeClient)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    if (client != excludeClient)
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            if (stream.CanWrite)
                            {
                                stream.Write(data, 0, data.Length);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}
