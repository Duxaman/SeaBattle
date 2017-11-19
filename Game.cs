using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;

namespace SeaBattle
{
    sealed public class Game
    {
        public enum GameState : byte
        {
            InProgress,
            Unstarted,
            Finished,
            ConnectionLost
        }
        private Field MyField;
        private Field EnemyField;
        private int GameId;
        public Game(Field MyField, bool FirstMove)
        {
            /*
             One Form will init gameObject and insert it ino the gameform
             If connection will be lost during the game, one must have opportunity to reconnect
             */
        }
        private void PlayerOnTryConnect()
        {
            /*
             * this will be called when game connection was lost and one wants to recconect 
             */
        }
        private void MaintainConnection()
        {

        }

    }
}
