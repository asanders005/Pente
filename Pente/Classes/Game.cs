using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Classes
{
    public class Game
    {
        public Board GameBoard { get; private set; }
        public string[] Players { get; private set; }

        public bool IsTria { get; private set; } = false;
        public bool IsTessera { get; private set; } = false;
        public bool GameOver { get; private set; } = false;

        public int CapturedWhite { get; private set; } = 0;
        public int CapturedBlack { get; private set; } = 0;

        public Game(string player1, string player2)
        {
            Players = new[] { player1, player2 };
            GameBoard = new Board();
        }

        public void PlaceStone(int x, int y)
        {
            GameBoard.PlaceStone(x, y, currentPlayer == 1);
            currentPlayer = currentPlayer == 0 ? 1 : 0;
        }

        public void CheckLines(int x, int y)
        {
            for (int cY = y - 1; cY < y + 1; cY++)
            {
                for (int cX = x - 1; cX < x + 1; cX++)
                {
                    if (GameBoard.board[cX, cY] == null) continue;

                    bool colorMatch = false;
                    if (currentStone == currentPlayer) colorMatch = true;
                }
            }
        }

        private int currentPlayer = 0;
    }
}
