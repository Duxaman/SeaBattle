using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace SeaBattle
{
    public partial class ConnectToTheGameForm : Form
    {
        private NetworkInterface Adapter;
        //TODO: Bind networkinstruments here
        public ConnectToTheGameForm()
        {
            InitializeComponent();
        }

        private void ChangeAdapterBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
