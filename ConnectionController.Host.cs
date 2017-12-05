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
        public void Connect(IPEndPoint Opponent)
        {
            //will be called when player 2 wants to connect to the host
            //method will add player to pool, and elicit select player
            TcpClient NewPlayer = new TcpClient(LocalPoint);
            try
            {
                NewPlayer.Connect(Opponent);
                Mode = false;
                ClientsPool.Add(Opponent.ToString(), NewPlayer);
                EnemyPoint = Opponent;
                Thread Handler = new Thread(new ParameterizedThreadStart(MaintainConnection));
                Handler.Start(NewPlayer);
                SendMessage(NewPlayer.GetStream(), MessageCode.PlayRequest);

            }
            catch (SocketException err)
            {
                throw new FailedToConnectException("Невозможно подключиться к серверу");
            }
        }
        public void Disconnect()
        {
            if (!Mode)
            {
                if (ClientsPool.ContainsKey(EnemyPoint.ToString()))
                {
                    SendMessage(ClientsPool[EnemyPoint.ToString()].GetStream(), MessageCode.Fin);
                    ClientsPool[EnemyPoint.ToString()].Close();
                }
            }
        }
    }
}
