using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace famillytalk
{
    public class Client
    {
        private TcpClient _tcpClient = new();
        private NetworkStream? _stream;

        public event Action<string>? OnMessageReceived;

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            try
            {
                await _tcpClient.ConnectAsync(ip, port);
                _stream = _tcpClient.GetStream();
                StartListening();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async void StartListening()
        {
            if (_stream == null) return;

            var buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch
            {
                // Nothing here
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (_stream == null) return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
}
