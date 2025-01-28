using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Classes
{
    public class Game
    {
        public Board GameBoard { get; private set; }
        public string[] Players { get; private set; }
        public NotificationType Notification { get; set; } = NotificationType.NONE;
        public bool GameOver { get; private set; } = false;
        public string? Winner { get; set; }

        public int CapturedWhite { get; private set; } = 0;
        public int CapturedBlack { get; private set; } = 0;

        public Game(string player1, string player2)
        {
            Players = new[] { player1, player2 };
            GameBoard = new Board();
            PlaceStone(9, 9);
        }

        public void PlaceStone(int x, int y)
        {
            if (!GameOver)
            {
                GameBoard.PlaceStone(x, y, currentPlayer == 1);
                CheckLines(x, y);
                PassTurn();
            }
        }

        public void PassTurn()
        {
            currentPlayer = currentPlayer == 0 ? 1 : 0;
        }

        public void CheckLines(int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                LineDirection lineToCheck = (LineDirection)i;
                Vector2 lineStart = GameBoard.GetLineStart(x, y, lineToCheck);
                LineType lineType = GameBoard.CheckLine((int)lineStart.X, (int)lineStart.Y, lineToCheck);

                switch (lineType)
                {
                    case LineType.CAPTURE:
                        if (currentPlayer == 0)
                        {
                            CapturedBlack += 2;
                        }
                        else
                        {
                            CapturedWhite += 2;
                        }

                        if (CapturedBlack >= 10)
                        {
                            Notification = NotificationType.WIN;
                            GameOver = true;
                            Winner = Players[0];
                        }
                        else if (CapturedWhite >= 10)
                        {
                            Notification = NotificationType.WIN;
                            GameOver = true;
                            Winner = Players[1];
                        }
                        break;
                    case LineType.TRIA:
                        Notification = NotificationType.TRIA;
                        break;
                    case LineType.TESSERA:
                        Notification = NotificationType.TESSERA;
                        break;
                    case LineType.WIN:
                        Notification = NotificationType.WIN;
                        GameOver = true;
                        Winner = Players[currentPlayer];
                        break;
                    default:
                        Notification = NotificationType.NONE;
                        break;
                }
            }
        }

        private int currentPlayer = 0;
    }

    public enum NotificationType
    {
        NONE,
        CAPTURE,
        TRIA,
        TESSERA,
        WIN
    }
}
