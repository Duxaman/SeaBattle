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
    public partial class WaitForPlayerForm : Form
    {
        private AdapterChoosingForm.AdapterCallBack Callback;
        public WaitForPlayerForm()
        {
            InitializeComponent();
            Program.ConnectionManager.BeginAcceptConnections();
            Callback = new AdapterChoosingForm.AdapterCallBack(SetAdapter);
            IpEndPointBox.Text = Program.ConnectionManager.LocalPoint.ToString();
            DialogResult = DialogResult.Cancel;          
        }

        private void ChooseAdapter_Click(object sender, EventArgs e)
        {
            AdapterChoosingForm ChooseAdapter = new AdapterChoosingForm(Program.ConnectionManager.Adapter, Callback);
            ChooseAdapter.Show();
        }

        private void SetAdapter(NetworkInterface Adapter)
        {
            Program.ConnectionManager.Adapter = Adapter;
            Program.ConnectionManager.BeginAcceptConnections();
            IpEndPointBox.Text = Program.ConnectionManager.LocalPoint.ToString();
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if(PlayerList.Items.Count > 0)
            {
                string[] Player = PlayerList.Items[PlayerList.SelectedIndex].ToString().Split(':');
                Program.ConnectionManager.SelectPlayer(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(Player[0]), Convert.ToUInt16(Player[1])));
                Program.ConnectionManager.StopAcceptConnections();
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            string[] players = Program.ConnectionManager.GetPlayersList();
            PlayerList.Items.Clear();
            PlayerList.Items.AddRange(players);
        }

        private void WaitForPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
                Program.ConnectionManager.StopAcceptConnections(); //TODO: will it be an exception if we'll stop already stopped server
                Program.ConnectionManager.CloseAllConnections(); 
            }
        }

        //add handlers here
    }
}
