using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using static Seabattle.NetworkInstruments;

namespace SeaBattle
{
    public partial class AdapterChoosingForm : Form
    {
        private List<NetworkInterface> Adapters;
        public delegate void AdapterCallBack(NetworkInterface NewAdapter);
        AdapterCallBack SetAdapter;
        public AdapterChoosingForm(NetworkInterface OldAdapter, AdapterCallBack SetAdapter)
        {
            InitializeComponent();
            this.SetAdapter = SetAdapter;
            NetworkInterface[] AdaptersRange = NetworkInterface.GetAllNetworkInterfaces();
            Adapters = new List<NetworkInterface>();
            for (int i = 0; i < AdaptersRange.Length; ++i)
            {
                if (AdaptersRange[i].OperationalStatus == OperationalStatus.Up && AdaptersRange[i].NetworkInterfaceType != NetworkInterfaceType.Loopback && getAdapterIPAddress(AdaptersRange[i]) != IPAddress.Any)
                {
                    Adapters.Add(AdaptersRange[i]);
                    AdapterBox.Items.Add(AdaptersRange[i].Name + " Status: " + AdaptersRange[i].OperationalStatus.ToString() + " Type: " + AdaptersRange[i].NetworkInterfaceType);
                    if (AdaptersRange[i].Id == OldAdapter.Id)
                    {
                        AdapterBox.SelectedIndex = AdapterBox.Items.Count - 1;
                        CurAdapterLab.Text = AdaptersRange[i].Name + " Status: " + AdaptersRange[i].OperationalStatus.ToString() + " Type: " + AdaptersRange[i].NetworkInterfaceType;
                    }
                }
            }
            if (AdapterBox.SelectedIndex == -1 && AdapterBox.Items.Count != 0)
            {
                AdapterBox.SelectedIndex = 0;
            }
        }

        private void AdapterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IpLab.Text = getAdapterIPAddress(Adapters[AdapterBox.SelectedIndex]).ToString();
            MacLab.Text = Adapters[AdapterBox.SelectedIndex].GetPhysicalAddress().ToString();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetBtn_Click(object sender, EventArgs e)
        {
            SetAdapter(Adapters[AdapterBox.SelectedIndex]);
            this.Close();
        }
    }
}
