using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UdpShapesLib;
using Message = UdpShapesLib.Message;

namespace UdpShapesClient {
    public partial class MainForm : Form {
        public string MyName { get; set; }
        private Players players = new Players ();
        private UdpClient server;
        private bool closed = false;

        public MainForm () {
            InitializeComponent ();
        }

        public void ConnectToServer (Player player) {
            Random random = new Random ();
            player.X = random.Next (ClientSize.Width - 50);
            player.Y = random.Next (ClientSize.Height - 50);
            byte[] enter = player.EnterMessage ();

            server = new UdpClient ("192.168.1.130", Ports.Server);
            server.Send (enter, enter.Length);
            players.OnEnter += OnEnter;
            ListenServer ();
        }
        private async void ListenServer () {
            try {
                while (!closed) {
                    UdpReceiveResult result = await server.ReceiveAsync ();
                    byte[] bytes = result.Buffer;
                    lock (players)
                        players.ProcessMessage (bytes);
                    Invalidate ();
                }
            }
            catch (SocketException) {
                Close ();
            }
        }
        private void OnEnter (Player newPlayer) {
            if (!players.TryGet (MyName, out Player me))
                return;
            byte[] exist = me.ExistMessage ();
            server.Send (exist, exist.Length);
        }

        private void MainForm_FormClosed (object sender, FormClosedEventArgs e) {
            byte[] leave = players[MyName].LeaveMessage ();
            server.Send (leave, leave.Length);
            closed = true;
        }

        private void MainForm_Paint (object sender, PaintEventArgs e) {
            lock (players)
                players.Draw (e.Graphics);
        }

        private bool dragging = false;
        private Point lastMouse;

        private void MainForm_MouseDown (object sender, MouseEventArgs e) {
            Point point = e.Location;
            if (!players[MyName].Contains (point))
                return;
            dragging = true;
            lastMouse = point;
        }

        private void MainForm_MouseMove (object sender, MouseEventArgs e) {
            if (!dragging)
                return;
            players[MyName].X += e.X - lastMouse.X;
            players[MyName].Y += e.Y - lastMouse.Y;
            lastMouse = e.Location;

            byte[] move = players[MyName].MoveMessage ();
            server.SendAsync (move, move.Length);
        }

        private void MainForm_MouseUp (object sender, MouseEventArgs e) {
            dragging = false;
        }
    }
}
