using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using static Seabattle.NetworkInstruments;

namespace SeaBattle
{
    /*
 * When one wants to play with server machine, it elicits tcp connection first
 * after connection was established, it waits until server responds with confirmation or rejection
 * In order to perform that, host send PlayRequest message rigth after it has connected to the server
 * Server side collects all players who is waiting, (keeps connections alive), and shows that info in the form to the user
 * User can choose desired player to play
 * The choosen one is selected by SelectPlayerMethod, which send PlayerAccept message to him, and allow game traffic through this player.
 * Afrer allowing game traffic all other waiting users are discarding from the user pool
 * After receivig that message, host allow game traffic too. 
 * */
    sealed public class ConnectionController
    {
        private NetworkInterface adapter;
        private volatile IPEndPoint EnemyPoint;
        private volatile Dictionary<string, TcpClient> ClientsPool;
        private TcpListener Server;
        private volatile bool GameTrafficAllowed;
        private static ConnectionController instance;
        private Object Locker;
        private bool Mode;
        //public ControllerState Status { get; private set; }
        private ConnectionController()
        {
            Adapter = getAnyAdaptor();
            EnemyPoint = new IPEndPoint(IPAddress.Any, 0);
            GameTrafficAllowed = false;
            ClientsPool = new Dictionary<string, TcpClient>();
            Mode = false;
        }
        public static ConnectionController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectionController();
                }
                return instance;
            }
        }
        private enum MessageCode : byte
        {
            PlayRequest,
            PlayAccept,
            GameData,
            Fin
        }
        private void MaintainConnection(Object Param)
        {
            TcpClient Connection = Param as TcpClient;
            string key = ((IPEndPoint)(Connection.Client.RemoteEndPoint)).ToString();
            NetworkStream Stream = Connection.GetStream();
            Connection.ReceiveBufferSize = 1000;
            Connection.ReceiveTimeout = 200;
            byte[] buffer = new byte[100]; //TODO: Determine game message size
            bool DataCollected;
            bool Finished = false;
            try
            {
                while (Connection.Connected && !Finished)
                {
                    DataCollected = false;
                    if (Connection.Available >= 1)
                    {
                        int code = Stream.ReadByte();
                        switch (code)
                        {

                            case (int)MessageCode.PlayAccept:                                              //if PlayerAccept messages arrived
                                if (EnemyPoint.Equals((IPEndPoint)(Connection.Client.RemoteEndPoint)))     //and that player was choosen
                                {
                                    if (!GameTrafficAllowed)                                               //and game traffic still not allowed
                                    {
                                        if (Mode)                                                          //if its the server side,
                                        {
                                            SendMessage(Stream, MessageCode.PlayAccept);                   // answer with confirmation
                                            ClearWatingList();                                                   //discard other players
                                        }
                                        else
                                        {
                                            OnConnect(this, new EventArgs());                              //if its host side invoke the event that announce to user that connection was succesfull
                                        }
                                        GameTrafficAllowed = true;
                                    }
                                }
                                break;
                            case (int)MessageCode.GameData:
                                if (EnemyPoint.Equals((IPEndPoint)(Connection.Client.RemoteEndPoint)))     //if the player is choosen one
                                {
                                    if (GameTrafficAllowed)                                                //and game traffic allowed
                                    {
                                        //collect all data depending on the gamedata size
                                        OnMessageReceive(this, new MessageReceiveEventArguments());        //invoke event to handle game data
                                    }
                                }
                                break;
                            case (int)MessageCode.Fin:
                                Finished = true;
                                break;
                        }

                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception)
            {
                //if one cannot send the message it means that connection was lost(if it was established)
                //so we need invoke the exception only when choosen player was disconnected
                lock (Locker)
                {
                    if (EnemyPoint.ToString() == key) //if choosen one was choosen(its in the poolist), and current stream equals
                    {
                        OnConnectionLost(this, new EventArgs()); // connection to the opponnet was lost

                    }
                }
            }
            ClientsPool[key].Close();
            ClientsPool.Remove(key);  //if connection was lost, or finished, that host should be deleted from the pool
        }
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
        private void SendMessage(NetworkStream ClientStream, MessageCode Code, params byte[] GameData)
        {

            if (Code == MessageCode.GameData)
            {
                ClientStream.WriteByte((byte)Code);
            }
            ClientStream.Write(new byte[] { (byte)Code }.Concat(GameData).ToArray(), 0, GameData.Length + 1);


        }
        public IPEndPoint LocalPoint { get; private set; }
        public class MessageReceiveEventArguments : EventArgs
        {
            public byte[] GameData;
        }

        public NetworkInterface Adapter
        {
            get
            {
                return adapter;
            }
            set
            {
                adapter = value;
                LocalPoint = new IPEndPoint(getAdapterIPAddress(adapter), 0);
                if (Mode) Server.Stop();
                //determine adapter address
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
        //by invoking this method server sends Playaccept message, to confirm his choose
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
                //player was disconnected exception
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
                    textclients.Add(client.ToString());
                }
            }
            return textclients.ToArray();
        }
        public void SendMsg(byte[] GameData)
        {
            try
            {
                SendMessage(ClientsPool[EnemyPoint.ToString()].GetStream(), MessageCode.GameData, GameData);
            }
            catch (Exception err)
            {

                OnConnectionLost(this, new EventArgs());
            }
            //send message to the connected opponent, if there is no connected one raised an exception
        }
        public void Connect(IPEndPoint Opponent)
        {
            //will be called when player 2 wants to connect to the host
            //method will add player to pool, and elicit select player
            TcpClient NewPlayer = new TcpClient(LocalPoint);
            try
            {
                NewPlayer.Connect(Opponent);
                Mode = false;
                ClientsPool.Add(NewPlayer.ToString(), NewPlayer);
                EnemyPoint = Opponent;
                Thread Handler = new Thread(new ParameterizedThreadStart(MaintainConnection));
                Handler.Start(NewPlayer);
                SendMessage(NewPlayer.GetStream(), MessageCode.PlayRequest);

            }
            catch (SocketException err)
            {
                //throw connection timeout
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
        //Will close all established connections
        public void CloseAllConnections()
        {
            EnemyPoint = new IPEndPoint(IPAddress.Any, 0);
            GameTrafficAllowed = false;
            Mode = false;
            lock(Locker)
            {
                foreach (KeyValuePair<string,TcpClient> Peer in ClientsPool)
                {
                    Peer.Value.Close();
                }
            }
        }
        public event EventHandler<MessageReceiveEventArguments> OnMessageReceive;
        public event EventHandler<EventArgs> OnConnectionLost;
        public event EventHandler<EventArgs> OnConnect;
    }
    public class FailedToConnectException : Exception
    {
        public FailedToConnectException() { }
        public FailedToConnectException(string message) : base(message) { }
        public FailedToConnectException(string message, Exception inner) : base(message, inner) { }
        protected FailedToConnectException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
