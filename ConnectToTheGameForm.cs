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
        private delegate void SafeLabelChange(Label label, string text);
        private delegate void SafeFormClose(ConnectToTheGameForm form);
        private Timer TimeoutTimer;
        public ConnectToTheGameForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Callback = new AdapterChoosingForm.AdapterCallBack(SetAdapter);
            TimeoutTimer = new Timer();
            TimeoutTimer.Interval = 20000;
            TimeoutTimer.Tick += OnConnectTimeout;
            IpEndPointBox.Text = Program.ConnectionManager.LocalPoint.ToString();
            Program.ConnectionManager.OnConnect += OnGameConnect;
        }

        private void OnConnectTimeout(object sender, EventArgs e)
        {
            MessageBox.Show("Невозможно поделючиться к " + IPAddress.Parse(IpBox.Text) + " : " + PortBox.Value.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            try
            {
                Program.ConnectionManager.Disconnect();
            }
            catch (Exception)
            {}
            TimeoutTimer.Stop();
            StatusLabel.Text = "Не подключен";
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
            SafeStatusChange(StatusLabel, "Не подключен");
        }
        private void OnGameConnect(object sender, EventArgs e)
        {
            TimeoutTimer.Stop();
            SafeStatusChange(StatusLabel, "Подключено");
            DialogResult = DialogResult.OK;
            SafeClose(this);
        }

        private void SafeClose(ConnectToTheGameForm form)
        {
            if (form.InvokeRequired)
            {
                SafeFormClose CloseForm = new SafeFormClose(SafeClose);
                this.Invoke(CloseForm, form);
            }
            else
            {
                form.Close();
            }
        }
        private void ConncectBtn_Click(object sender, EventArgs e)
        {
            IPAddress Opponent;
            if (IPAddress.TryParse(IpBox.Text, out Opponent))
            {
                try
                {
                    Program.ConnectionManager.Connect((new IPEndPoint(Opponent, Convert.ToUInt16(PortBox.Value))));
                    TimeoutTimer.Start();
                    StatusLabel.Text = "Ожидание подтверждения...";
                }
                catch (FailedToConnectException err)
                {
                    MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SafeStatusChange(Label StatusLab, string text)
        {
            if (StatusLabel.InvokeRequired)
            {
                SafeLabelChange LabelChange = new SafeLabelChange(SafeStatusChange);
                this.Invoke(LabelChange, new object[] { StatusLab, text });
            }
            else
            {
                StatusLab.Text = text;
            }
        }
        private void ConnectToTheGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                Program.ConnectionManager.CloseAllConnections();
            }
            TimeoutTimer.Stop();
            Application.DoEvents(); //required when form is shutting from another thread
        }
    }
}
