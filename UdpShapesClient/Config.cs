using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UdpShapesLib;

namespace UdpShapesClient {
    public partial class Config : Form {
        public Config () {
            InitializeComponent ();
        }

        private void Config_Load (object sender, EventArgs e) {
            foreach (Shape shape in Enum.GetValues (typeof (Shape)))
                comboBoxShape.Items.Add (shape);

            comboBoxColor.Items.Add (Color.Red);
            comboBoxColor.Items.Add (Color.Yellow);
            comboBoxColor.Items.Add (Color.Blue);
            comboBoxColor.Items.Add (Color.Green);
        }

        private void buttonConnect_Click (object sender, EventArgs e) {
            try {
                string name = textBoxName.Text;
                if (name == "")
                    throw new ApplicationException ("Введите имя");
                if (comboBoxShape.SelectedIndex == -1)
                    throw new ApplicationException ("Выберите фигуру");
                if (comboBoxColor.SelectedIndex == -1)
                    throw new ApplicationException ("Выберите цвет");
                DialogResult = DialogResult.OK;
                Visible = false;

                MainForm form = new MainForm ();
                form.MyName = name;
                Shape shape = (Shape) comboBoxShape.SelectedItem;
                Color color = (Color) comboBoxColor.SelectedItem;
                form.ConnectToServer (new Player (name, shape, color.R, color.G, color.B));
                form.ShowDialog ();

                Close ();
            }
            catch (ApplicationException ex) {
                MessageBox.Show (ex.Message);
            }
        }
    }
}
