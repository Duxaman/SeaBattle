using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Timers;
using static Seabattle.NetworkInstruments;

namespace SeaBattle
{
    sealed public partial class ConnectionController
    {
        private void ClearWatingList()
        {
            //clears all waiting users except the choosen one
            Dictionary<string, TcpClient>.KeyCollection Keys = ClientsPool.Keys;
            foreach (string key in Keys)
            {
                if (key != EnemyPoint.ToString())
                {
                    ClientsPool.Remove(key);
                }
            }
        }
        public void BeginAcceptConnections()
        {
            //elicit accpeting new connections
            Server = new TcpListener(LocalPoint);
            Server.Start();
            LocalPoint = ((IPEndPoint)Server.LocalEndpoint);
            Mode = true;
            Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
        }
        private void ClientOnTryConnect(IAsyncResult res)
        {
            try
            {
                TcpListener Server = res.AsyncState as TcpListener;
                TcpClient NewHost = Server.EndAcceptTcpClient(res);
                string key = ((IPEndPoint)NewHost.Client.RemoteEndPoint).ToString();
                if (!ClientsPool.ContainsKey(key))
                {
                    Thread Handler = new Thread(new ParameterizedThreadStart(MaintainConnection));
                    ClientsPool.Add(key, NewHost);
                    Handler.Start(NewHost);
                }
                Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        public void StopAcceptConnections()
        {
            Server.Stop();
        }
        //by invoking this method server sends Playaccept message, to confirm players choose
        public void SelectPlayer(IPEndPoint player)
        {
            if (ClientsPool.ContainsKey(player.ToString()))
            {
                EnemyPoint = player;
                SendMessage(ClientsPool[EnemyPoint.ToString()].GetStream(), MessageCode.PlayAccept);
            }
            else
            {
                throw new FailedToConnectException("Хост с таким адресом недоступен, похоже он отключился");
            }
        }
        //returns wairing players list
        public string[] GetPlayersList()
        {
            List<string> textclients = new List<string>();
            lock (Locker)
            {
                foreach (KeyValuePair<string, TcpClient> client in ClientsPool)
                {
                    textclients.Add(((IPEndPoint)(client.Value.Client.RemoteEndPoint)).ToString());
                }
            }
            return textclients.ToArray();
        }
    }
}
