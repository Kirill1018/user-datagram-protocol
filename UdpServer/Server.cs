using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer {
    class Server {
        static void Main (string[] args) {
            new Server ().Run ();
        }

        private void Run () {
            Socket listen = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            EndPoint orig = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 2019);
            listen.Bind (orig);
            Console.WriteLine ("Receiving on UDP port 2019");

            byte[] buffer = new byte[1024];
            while (true) {
                EndPoint point = orig;
                int count = listen.ReceiveFrom (buffer, ref point);
                if (count == 0)
                    break;
                Console.WriteLine (Encoding.ASCII.GetString (buffer, 0, count) + " from " + point.ToString ());
            }
        }
    }
}
