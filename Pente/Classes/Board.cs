using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Classes
{
    public class Board
    {
        public bool?[,] board { get; private set; }

        public Board()
        {
            board = new bool?[19,19];
        }

        public void PlaceStone(int x, int y, bool isWhite)
        {
            if (board[x, y] == null)
            {
                board[x, y] = isWhite;
            }
        }

        public void RemoveStone(int x, int y)
        {
            board[x, y] = null;
        }
    }
}
