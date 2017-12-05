using System;
using System.Collections.Generic;

namespace SeaBattle
{
    sealed public class Game
    {
        public Field MyField { get; private set; }
        public Field EnemyField { get; private set; }
        public int[] MyCounters { get; private set; }
        public int[] EnemyCounters { get; private set; }
        private class ship
        {
            public ShipType Type;
            public int DeckRemains;
            public ship(ShipType type)
            {
                Type = type;
                switch(type)
                {
                    case ShipType.OneDeck: DeckRemains = 1; break;
                    case ShipType.TwoDeck: DeckRemains = 2; break;
                    case ShipType.ThreeDeck: DeckRemains = 3; break;
                    case ShipType.FourDeck: DeckRemains = 4; break;
                    default: throw new Exception();
                }
            }
        }
        private Dictionary<string, ship> MyDeckCounter;
        private Dictionary<string, ship> EnemyDeckCounter;

        private string GameId;
        public bool CurrentMove { get;  set; }
        public Game(Field MyField, bool FirstMove)
        {
            CurrentMove = FirstMove;
            this.MyField = MyField;
            EnemyField = new Field(MyField.FieldSize);
            EnemyCounters = new int[4];
            MyCounters = new int[4];
            MyDeckCounter = new Dictionary<string, ship>(10);
            EnemyDeckCounter = new Dictionary<string, ship>(10);
            for(int i = 1; i <= 4; ++i)
            {
                EnemyCounters[i - 1] = i;
                MyCounters[i - 1] = i;
            }
            Field.Cell Cell = new Field.Cell();
            for(int i = 0; i < MyField.FieldSize; ++i)
                for(int j = 0; j < MyField.FieldSize; ++j)
                {
                    Cell = MyField.getCell(i, j);
                    if (Cell.State == CellState.Occupied)
                    {
                        if(!MyDeckCounter.ContainsKey(Cell.Shipid))
                        {
                            MyDeckCounter.Add(Cell.Shipid, new ship(Cell.Type));
                        }
                    }
                }
            if(FirstMove)
            {
                //set id
                Random Randomizer = new Random();
                GameId = DateTime.Now.ToString("HHmm") + Randomizer.Next(100,999).ToString();
            }
            else
            {
                GameId = "00000-1"; //set default id
            }
        }
        public event EventHandler<GameData> SendMessage;
        public event EventHandler<bool> OnGameFinished;
        public event EventHandler<DataUpdateEventArguments> OnDataUpdate;
        public event EventHandler ChangeMove;
        public void ReceiveMsg(object sender, GameData Data)
        {
            //parse message
            /*
             * first check if id is the same, if id is not set set it
             * dann if its attack request return message about succesfuly
             * 
             */
             if(Data.GameId == GameId)
            {
                bool labelupdaterequired = false;
                switch (Data.Type)
                {
                    case GameMessage.Attack:
                        {
                            GameData respond = new GameData(GameMessage.AttackRespond);
                            
                            respond.GameId = GameId;
                            //check and send respond back
                            Field.Cell CheckingCell = MyField.getCell(Data.AttackingCell.X, Data.AttackingCell.Y);
                            if(CheckingCell.State == CellState.Occupied)
                            {
                                MyDeckCounter[CheckingCell.Shipid].DeckRemains -= 1;
                                if(MyDeckCounter[CheckingCell.Shipid].DeckRemains == 0)
                                {
                                    switch(MyDeckCounter[CheckingCell.Shipid].Type)
                                    {
                                        case ShipType.FourDeck: MyCounters[0]--; break;
                                        case ShipType.ThreeDeck: MyCounters[1]--; break;
                                        case ShipType.TwoDeck: MyCounters[2]--; break;
                                        case ShipType.OneDeck: MyCounters[3]--; break;
                                    }
                                    MyDeckCounter.Remove(CheckingCell.Shipid);
                                    labelupdaterequired = true;
                                }
                                MyField.setCellState(Data.AttackingCell.X, Data.AttackingCell.Y, CellState.Hit);
                                respond.shiptype = CheckingCell.Type;
                                respond.AttackResult = true;
                                respond.shipId = CheckingCell.Shipid;
                                respond.AttackingCell = Data.AttackingCell;
                                SendMessage(this, respond);
                                OnDataUpdate(this, new DataUpdateEventArguments(true, Data.AttackingCell,labelupdaterequired));
                                if (labelupdaterequired)
                                {
                                    if(MyDeckCounter.Count == 0)
                                    {
                                        OnGameFinished(this, false);
                                    }
                                }
                            }
                            else if(CheckingCell.State == CellState.Free)
                            {
                                MyField.setCellState(Data.AttackingCell.X, Data.AttackingCell.Y, CellState.Missed);
                                respond.AttackResult = false;
                                respond.AttackingCell = Data.AttackingCell;
                                SendMessage(this, respond);
                                OnDataUpdate(this, new DataUpdateEventArguments(true, Data.AttackingCell, false));
                                CurrentMove = true;
                            }


                            //if the there is no ships, invoke endgame
                            ChangeMove(this, new EventArgs());
                        }
                        break;
                    case GameMessage.AttackRespond:
                        {
                            //add data to local list, invoke gamefinish if there is no ships left
                            //invoke update data, change mode
                            if(Data.GameId == GameId)
                            {
                                if(Data.AttackResult)
                                {                                    
                                    if (EnemyDeckCounter.ContainsKey(Data.shipId))
                                    {
                                        CurrentMove = true;
                                        EnemyDeckCounter[Data.shipId].DeckRemains--;
                                        if (EnemyDeckCounter[Data.shipId].DeckRemains == 0)
                                        {
                                            labelupdaterequired = true;
                                            EnemyCounters[(byte)EnemyDeckCounter[Data.shipId].Type]--;
                                            EnemyDeckCounter.Remove(Data.shipId);
                                        }
                                        EnemyField.setCellState(Data.AttackingCell.X, Data.AttackingCell.Y, CellState.Hit);
                                        OnDataUpdate(this, new DataUpdateEventArguments(false, Data.AttackingCell, labelupdaterequired));
                                        //if all enemycounters == 0  dann we win
                                        if(EnemyCounters[0] == 0 && EnemyCounters[1] == 0 && EnemyCounters[2] == 0 && EnemyCounters[3] == 0)
                                        {
                                            OnGameFinished(this, true);
                                        }
                                        
                                    }
                                    else
                                    {
                                        EnemyDeckCounter.Add(Data.shipId, new ship(Data.shiptype));
                                        ReceiveMsg(this, Data);
                                    }                                    
                                }
                                else
                                {
                                    CurrentMove = false;
                                    EnemyField.setCellState(Data.AttackingCell.X, Data.AttackingCell.Y, CellState.Missed);
                                    OnDataUpdate(this, new DataUpdateEventArguments(false, Data.AttackingCell, labelupdaterequired));
                                }
                            }
                            ChangeMove(this, new EventArgs());
                        }
                        break;
                    case GameMessage.EndGame:
                        {
                            //finish game with win msg
                            //invoke game finished
                            OnGameFinished(this, true);
                        }
                        break;
                }
            }
            else
            {
                if(GameId == "00000-1")
                {
                    GameId = Data.GameId;
                    ReceiveMsg(sender, Data);
                }
            }
        }
        public bool MakeMove(Point cell)
        {
            //do not change current move state until answer from enemy received
            if(CurrentMove)
            {
                //check if one can fire that cell
                if(EnemyField.getCell(cell.X,cell.Y).State == CellState.Free)
                {
                    GameData Data = new GameData(GameMessage.Attack);
                    Data.AttackingCell = cell;
                    Data.GameId = GameId;
                    SendMessage(this, Data);
                    return true;
                }
                
            }
            return false;
        }

    }
}
