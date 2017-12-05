using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class GameData : EventArgs
    {
        public string GameId; //7 bytes;
        public GameMessage Type;
        public Point AttackingCell { get; set; }
        public bool AttackResult;
        public string shipId;
        public ShipType shiptype;
        public byte[] Bytes
        {
            get
            {
                List<byte> data = new List<byte>();
                for(int i = 0; i < 7; ++i)
                {
                    data.Add((byte)GameId[i]);
                }
                data.Add((byte)Type);
                switch(Type)
                {
                    case GameMessage.Attack:
                        {
                            data.Add((byte)AttackingCell.X);
                            data.Add((byte)AttackingCell.Y);
                            data.AddRange(new byte[] { 0, 0, 0 , 0 });
                        } break;
                    case GameMessage.AttackRespond:
                        {
                            data.Add(Convert.ToByte(AttackResult));
                            data.Add((byte)shiptype);
                            data.Add((byte)AttackingCell.X);
                            data.Add((byte)AttackingCell.Y);
                            if (AttackResult)
                            {
                                data.Add((byte)shipId[0]);
                                data.Add((byte)shipId[1]); 
                            }
                            else
                            {
                                data.Add(0);
                                data.Add(0);
                            }
                        }
                        break;
                    case GameMessage.EndGame:
                        {
                            data.AddRange(new byte[] { 0, 0, 0, 0 , 0, 0});
                        }
                        break;
                }
                return data.ToArray();
            }
        }
        public GameData(GameMessage Type)
        {
            this.Type = Type;
        }
        public GameData(byte[] Data)
        {
            GameId = "";
            shipId = "";
            for (int i = 0; i < 7; i++)
            {
                GameId += (char)Data[i];
            }
            Type = (GameMessage)Data[7];
            switch(Data[7])
            {
                case (byte)GameMessage.Attack:
                    {
                        AttackingCell = new Point(Data[8],Data[9]);
                    }
                    break;
                case (byte)GameMessage.AttackRespond:
                    {
                        AttackResult = Convert.ToBoolean(Data[8]);
                        shiptype = (ShipType)Data[9];
                        AttackingCell = new Point(Data[10], Data[11]);
                        shipId += (char)Data[12];
                        shipId += (char)Data[13];
                    }
                    break;
                case (byte)GameMessage.EndGame:break;
                default: throw new Exception();
            }
        }

    }
    public class DataUpdateEventArguments: EventArgs
    {
        public bool LocalField { get; set; }
        public Point Cell { get; set; }
        public bool labelUpdateRequired { get; set; }
        public DataUpdateEventArguments(bool FieldOwner, Point Cell, bool LabelUpdateRequired)
        {
            this.LocalField = FieldOwner;
            this.Cell = Cell;
            labelUpdateRequired = LabelUpdateRequired;
        }

    }
    public enum GameMessage : byte
    {
        Attack, // contains info about attacking cell
        AttackRespond, // message that ensure users move 
        EndGame // end game message
    }
}
