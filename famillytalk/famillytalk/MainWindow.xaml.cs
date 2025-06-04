using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace famillytalk
{
    public partial class MainWindow : MetroWindow
    {
        private Client? _client;
        private Server? _server;
        private const int Port = 5000;

        private string _myPseudo = "";
        private string _friendPseudo = "Friend";
        private bool _friendPseudoSet = false;

        public MainWindow()
        {
            InitializeComponent();

            // Start server listening
            _server = new Server(IPAddress.Any, Port);
            _server.OnMessageReceived += (msg) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (msg.StartsWith("__pseudo__::"))
                    {
                        _friendPseudo = msg.Substring("__pseudo__::".Length);
                        _friendPseudoSet = true;
                        ChatHistory.AppendText($"*** {_friendPseudo} s'est connecté ***\n");
                    }
                    else
                    {
                        string name = _friendPseudoSet ? _friendPseudo : "Friend";
                        ChatHistory.AppendText($"{name}: {msg}\n");
                    }
                    ChatHistory.ScrollToEnd();
                });
            };
            _server.StartListening();
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string pseudo = PseudoBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(pseudo))
            {
                await this.ShowMessageAsync("Erreur", "Veuillez entrer un pseudo avant de vous connecter.");
                return;
            }

            string targetIp = TargetIPBox.Text.Trim();
            if (!IPAddress.TryParse(targetIp, out _))
            {
                await this.ShowMessageAsync("Erreur", "Adresse IP invalide.");
                return;
            }

            _myPseudo = pseudo;

            _client = new Client();
            bool connected = await _client.ConnectAsync(targetIp, Port);

            if (connected)
            {
                await this.ShowMessageAsync("Succès", $"Connecté à {targetIp}:{Port} en tant que {_myPseudo}");

                // Send pseudo to server
                await _client.SendMessageAsync($"__pseudo__::{_myPseudo}");

                _client.OnMessageReceived += (msg) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (msg.StartsWith("__pseudo__::"))
                        {
                            _friendPseudo = msg.Substring("__pseudo__::".Length);
                            _friendPseudoSet = true;
                            ChatHistory.AppendText($"*** {_friendPseudo} s'est connecté ***\n");
                        }
                        else
                        {
                            string name = _friendPseudoSet ? _friendPseudo : "Friend";
                            ChatHistory.AppendText($"{name}: {msg}\n");
                        }
                        ChatHistory.ScrollToEnd();
                    });
                };
            }
            else
            {
                await this.ShowMessageAsync("Erreur", $"Impossible de se connecter à {targetIp}:{Port}");
                _client = null;
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        private async System.Threading.Tasks.Task SendMessageAsync()
        {
            if (_client == null)
            {
                await this.ShowMessageAsync("Erreur", "Veuillez d'abord vous connecter à une IP cible.");
                return;
            }

            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _client.SendMessageAsync(message);
                ChatHistory.AppendText($"{_myPseudo}: {message}\n");
                ChatHistory.ScrollToEnd();
                MessageInput.Clear();
            }
        }

        private async void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                e.Handled = true;
                await SendMessageAsync();
            }
        }

        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string clipboardText = Clipboard.GetText().Trim();

            if (IPAddress.TryParse(clipboardText, out _))
            {
                TargetIPBox.Text = clipboardText;
                MessageBox.Show($"IP '{clipboardText}' ajoutée automatiquement.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Le contenu du presse-papiers n'est pas une IP valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
