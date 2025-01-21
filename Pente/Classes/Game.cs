using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Classes
{
    public class Game
    {
        public Board GameBoard { get; private set; }
        public string[] Players { get; private set; }

        public Game(string player1, string player2)
        {
            Players = new[] { player1, player2 };
            GameBoard = new Board();
        }

        public void PlaceStone(int x, int y)
        {
            GameBoard.PlaceStone(x, y, currentPlayer == 0);
            currentPlayer = currentPlayer == 0 ? 1 : 0;
        }

        private int currentPlayer = 0;
    }
}
