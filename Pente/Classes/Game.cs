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
        public int CurrentPlayer { get; private set; } = 0;
        public NotificationType Notification { get; set; } = NotificationType.NONE;
        public bool GameOver { get; private set; } = false;
        public string? Winner { get; set; }

        public int CapturedWhite { get; private set; } = 0;
        public int CapturedBlack { get; private set; } = 0;

        public bool ResetTimer { get; set; } = false;

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
                GameBoard.PlaceStone(x, y, CurrentPlayer == 1);
                CheckLines(x, y);
                PassTurn();
            }
        }

        public void PassTurn()
        {
            ResetTimer = true;
            CurrentPlayer = CurrentPlayer == 0 ? 1 : 0;
        }

        public void CheckLines(int x, int y)
        {
            bool lineChecked = false;
            for (int i = 0; i < 4; i++)
            {
                LineDirection lineToCheck = (LineDirection)i;
                Vector2 lineStart = GameBoard.GetLineStart(x, y, lineToCheck);
                LineType lineType = GameBoard.CheckLine((int)lineStart.X, (int)lineStart.Y, lineToCheck);

                switch (lineType)
                {
                    case LineType.TRIA:
                        if (!lineChecked || Notification == NotificationType.NONE)
                        {
                            Notification = NotificationType.TRIA;
                            lineChecked = true;
                        }
                        break;
                    case LineType.TESSERA:
                        if (!lineChecked || Notification == NotificationType.NONE || Notification == NotificationType.TRIA)
                        {
                            Notification = NotificationType.TESSERA;
                            lineChecked = true;
                        }
                        break;
                    case LineType.WIN:
                        if (!lineChecked || Notification != NotificationType.WIN)
                        {
                            Notification = NotificationType.WIN;
                            GameOver = true;
                            Winner = Players[CurrentPlayer];
                            lineChecked = true;
                        }
                        break;
                    default:
                        if (!lineChecked)
                        {
                            Notification = NotificationType.NONE;
                        }
                        break;
                }

                lineStart = GameBoard.GetLineStart(x, y, lineToCheck, true);
                bool captureStones = GameBoard.CheckCapture((int)lineStart.X, (int)lineStart.Y, lineToCheck);

                if (captureStones)
                {
                    if (CurrentPlayer == 0)
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
                }

                if (Notification == NotificationType.WIN) return;
            }
        }
    }

    public enum NotificationType
    {
        NONE,
        TRIA,
        TESSERA,
        WIN
    }
}
