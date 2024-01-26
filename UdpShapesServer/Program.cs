using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UdpShapesLib;

namespace UdpShapesServer {
    class Program {
        static void Main (string[] args) => new Program ().Run ();

        private UdpClient server;
        private IList<UdpClient> clients = new List<UdpClient> ();
        private IList<IPEndPoint> endPoints = new List<IPEndPoint> ();

        private void Run () {
            server = new UdpClient (Ports.Server);
            Console.WriteLine ("Server is running");
            Listen ().Wait ();
        }
        private async Task Listen () {
            while (true) {
                UdpReceiveResult result = await server.ReceiveAsync ();
                if (!endPoints.Contains (result.RemoteEndPoint))
                    endPoints.Add (result.RemoteEndPoint);
                foreach (IPEndPoint endPoint in endPoints)
                    server.SendAsync (result.Buffer, result.Buffer.Length, endPoint);
                Console.WriteLine ($"Sent {result.Buffer.Length} bytes to {endPoints.Count} clients");
            }
        }
    }
}
