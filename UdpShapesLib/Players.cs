using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpShapesLib {
    public class Players {
        private IDictionary<string, Player> players = new Dictionary<string, Player> ();

        public event Action<Player> OnEnter;

        public Player this[string name] => players[name];

        public bool TryGet (string name, out Player player) => players.TryGetValue (name, out player);

        public void ProcessMessage (byte[] bytes) {
            MemoryStream stream = new MemoryStream (bytes);
            BinaryReader reader = new BinaryReader (stream);
            byte message = reader.ReadByte ();
            if (message == Message.Enter) {
                Player newPlayer = new Player (reader);
                players.Add (newPlayer.Name, newPlayer);
                OnEnter?.Invoke (newPlayer);
            }
            else if (message == Message.Exist) {
                Player newPlayer = new Player (reader);
                if (players.ContainsKey (newPlayer.Name))
                    return;
                players.Add (newPlayer.Name, newPlayer);
            }
            else if (message == Message.Move) {
                string name = reader.ReadString ();
                players[name].Move (reader);
            }
            else if (message == Message.Leave) {
                string name = reader.ReadString ();
                players.Remove (name);
            }
            else if (message == Message.color)
            {
                string name = reader.ReadString();
                players.Add(name, new Player(reader));
            }
        }

        public void Draw (Graphics g) {
            foreach (Player player in players.Values)
                player.Draw (g);
        }
    }
}
