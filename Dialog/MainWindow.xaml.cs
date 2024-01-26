using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dialog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (type.Text == "5")
            {
                string[] messages = { type.Text, name.Text, color1.Text,
                color2.Text, color3.Text };
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                for (int i = 0; i < messages.Length; i++) await Send(messages[i], socket);
            }
        }
        public async Task Send(string message, Socket socket)
        {
            byte[] buffer = Encoding.Default.GetBytes(type.Text);
            await socket.SendToAsync(new ArraySegment<byte>(buffer), SocketFlags.None, new IPEndPoint(IPAddress.Parse("192.168.1.130"), 2019));
        }
    }
}