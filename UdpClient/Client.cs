using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpClient {
    class Client {
        static void Main (string[] args) {
            new Client ().Run ();
        }

        private void Run () {
            Socket conn = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            EndPoint target = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 2019);

            conn.SendTo (Encoding.ASCII.GetBytes ("Greetings from UDP client!"), target);

            conn.Shutdown (SocketShutdown.Both);
            conn.Close ();
        }
    }
}
