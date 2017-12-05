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
    /*
 * When one wants to play with server machine, it elicits tcp connection first
 * after connection was established, it waits until server responds with confirmation or rejection
 * Server side collects all players who is waiting, (keeps connections alive), and shows that info in the form to the user
 * User can choose desired player to play
 * The choosen one is selected by SelectPlayerMethod, which send PlayAccept message to host, and allow game traffic through this player.
 * Afrer allowing game traffic all other waiting users are discarding from the user pool
 * After receivig that message, host allow game traffic too. 
 * 1)user have choosen the player
 * server -> playaccpet -> host --> prepare for game
 * host ->  playaccept -> server --> prepare for game
 * 
 * 
 * */

    sealed public partial class ConnectionController
    {
        private NetworkInterface adapter;
        private volatile IPEndPoint EnemyPoint;
        private volatile Dictionary<string, TcpClient> ClientsPool;
        private TcpListener Server;
        private volatile bool GameTrafficAllowed;
        private static ConnectionController instance;
        private System.Timers.Timer ConnectionChecker;
        private Object Locker;
        public bool Mode { get; private set; }
        public byte MessageSize { get; set; }
        private ConnectionController(byte MessageSize)
        {
            Locker = new object();
            EnemyPoint = new IPEndPoint(IPAddress.Any, 0);
            GameTrafficAllowed = false;
            ClientsPool = new Dictionary<string, TcpClient>();
            ConnectionChecker = new System.Timers.Timer();
            ConnectionChecker.Elapsed += ConnectionChecker_Elapsed;
            ConnectionChecker.Interval = 3000;
            Adapter = getAnyAdaptor();
            Mode = false;
            this.MessageSize = MessageSize;
        }

        private void ConnectionChecker_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendMessage(ClientsPool[EnemyPoint.ToString()].GetStream(), MessageCode.Echo);
            }
            catch (Exception)
            {
                ConnectionChecker.Stop();
                OnConnectionLost(this, new EventArgs());
            }
            /*
             send to the opponent echo
             if connection was lost tcpclient wont be able to deliver the message, and onconnectionlost wiil be raised
             */
        }

        public static ConnectionController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectionController(14);
                }
                return instance;
            }
        }
        private enum MessageCode : byte
        {
            PlayRequest,
            PlayAccept,
            GameData,
            Echo,
            Fin
        }
        private void MaintainConnection(Object Param)
        {
            TcpClient Connection = Param as TcpClient;
            string key = ((IPEndPoint)(Connection.Client.RemoteEndPoint)).ToString();
            NetworkStream Stream = Connection.GetStream();
            Connection.ReceiveBufferSize = 1000;
            Connection.ReceiveTimeout = 200;
            byte[] buffer = new byte[MessageSize]; 
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
                                            ClearWatingList();                                             //discard other players
                                        } 
                                        else
                                        {
                                            SendMessage(Stream, MessageCode.PlayAccept);                   //answer with ack msg
                                            OnConnect(this, new EventArgs());                              //if its host side invoke the event that announce to user that connection was succesfully established
                                        }
                                        GameTrafficAllowed = true;
                                        ConnectionChecker.Start();                                         //enable connection checker timer
                                    }
                                }
                                break;
                            case (int)MessageCode.GameData:
                                if (EnemyPoint.Equals((IPEndPoint)(Connection.Client.RemoteEndPoint)))     //if the player is choosen one
                                {
                                    if (GameTrafficAllowed)                                                //and game traffic allowed
                                    {
                                        while(!DataCollected && (Connection.Connected && !Finished))
                                        {
                                            if(Connection.Available >= MessageSize)
                                            {
                                                DataCollected = true;
                                                Stream.Read(buffer, 0, MessageSize);
                                            }
                                        }
                                        //collect all data depending on the gamedata size
                                        if (DataCollected)
                                        {
                                            OnMessageReceive(this, new GameData(buffer));    
                                        }  
                                    }
                                }
                                break;
                            case (int)MessageCode.Fin:
                                Finished = true;
                                break;
                            default:;
                                break;
                        }

                    }
                }
            }
            catch (ObjectDisposedException err)
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
                        ConnectionChecker.Stop();        //stops the timer
                        GameTrafficAllowed = false;       //forbid game traffic
                        OnConnectionLost(this, new EventArgs()); // connection to the opponnet was lost

                    }
                }
            }
            ClientsPool[key].Close();
            ClientsPool.Remove(key);  //if connection was lost, or finished, that host should be deleted from the pool
        }
       
        private void SendMessage(NetworkStream ClientStream, MessageCode Code, params byte[] GameData)
        {
            ClientStream.WriteByte((byte)Code);
            if (Code == MessageCode.GameData)
            {
                ClientStream.Write(GameData, 0, GameData.Length);
            }
           


        }
        public IPEndPoint LocalPoint { get; private set; }

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
                CloseAllConnections();
            }
        }

        public void SendMsg(object sender, GameData data)
        {
            try
            {
                SendMessage(ClientsPool[EnemyPoint.ToString()].GetStream(), MessageCode.GameData, data.Bytes);
            }
            catch (Exception err)
            {
                ConnectionChecker.Stop();
                OnConnectionLost(this, new EventArgs());
            }
            //send message to the connected opponent, if there is no connected one raised an exception
        }

        //Will close all established connections
        public void CloseAllConnections()
        {
            ConnectionChecker.Stop();
            EnemyPoint = new IPEndPoint(IPAddress.Any, 0);
            GameTrafficAllowed = false;
            Mode = false;
            string[] keys = ClientsPool.Keys.ToArray();
            lock(Locker)
            {
                for(int i = 0; i < keys.Length; ++i)
                {
                    if(ClientsPool.ContainsKey(keys[i]))
                    {
                        try
                        {
                            SendMessage(ClientsPool[keys[i]].GetStream(), MessageCode.Fin);
                            ClientsPool[keys[i]].Close();
                        }
                        catch (Exception)
                        {}
                    }
                }
            }
        }
        public event EventHandler<GameData> OnMessageReceive;
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
