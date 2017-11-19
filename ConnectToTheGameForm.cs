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
using System.Net;

namespace SeaBattle
{
    public partial class ConnectToTheGameForm : Form
    {
        private AdapterChoosingForm.AdapterCallBack Callback;
        private Timer TimeoutTimer;
        public ConnectToTheGameForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Callback = new AdapterChoosingForm.AdapterCallBack(SetAdapter);
            TimeoutTimer = new Timer();
            TimeoutTimer.Interval = 20;
            TimeoutTimer.Tick += OnConnectTimeout;
            IpEndPointBox.Text = Program.ConnectionManager.LocalPoint.ToString();
            Program.ConnectionManager.OnConnect += OnGameConnect;
        }

        private void OnConnectTimeout(object sender, EventArgs e)
        {
            MessageBox.Show("Невозможно поделючиться к " + IPAddress.Parse(IpBox.Text) + " : " + PortBox.Value.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Program.ConnectionManager.Disconnect();
        }

        private void ChangeAdapterBtn_Click(object sender, EventArgs e)
        {
            AdapterChoosingForm ChooseAdapter = new AdapterChoosingForm(Program.ConnectionManager.Adapter, Callback);
            ChooseAdapter.Show();
        }
        private void SetAdapter(NetworkInterface Adapter)
        {
            Program.ConnectionManager.Adapter = Adapter;
            IpEndPointBox.Text = Program.ConnectionManager.LocalPoint.ToString();
        }
        private void OnGameConnect(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConncectBtn_Click(object sender, EventArgs e)
        {
            IPAddress Opponent;
            if(IPAddress.TryParse(IpBox.Text, out Opponent))
            {
                Program.ConnectionManager.Connect((new IPEndPoint(Opponent, Convert.ToUInt16(PortBox.Value))));
                TimeoutTimer.Start();
            }            
        }

        private void ConnectToTheGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                Program.ConnectionManager.CloseAllConnections(); 
            }
        }
    }
}
