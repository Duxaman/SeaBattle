using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void CreateGameBtn_Click(object sender, EventArgs e)
        {
            ShipsMappingForm StartForm = new ShipsMappingForm(false);
            StartForm.Show();
            this.Hide();
        }

        private void ConnectToGameBtn_Click(object sender, EventArgs e)
        {
            ShipsMappingForm StartForm = new ShipsMappingForm(true);
            StartForm.Show();
            this.Hide();
        }

        private void QuitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
