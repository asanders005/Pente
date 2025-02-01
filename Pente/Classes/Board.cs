using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Classes
{
    public class Board
    {
        // 1 == black
        // 0 == white
        // null == empty
        public bool?[,] board { get; private set; }

        public Board()
        {
            board = new bool?[19, 19];
        }

        public void PlaceStone(int x, int y, bool isBlack)
        {
            if (board[x, y] == null)
            {
                board[x, y] = isBlack;
            }
        }

        public int CheckStone(int x, int y)
        {
            if (board[x, y] == true) return 1;
            return 0;
        }

        public Vector2 GetLineStart(int x, int y, LineDirection lineDirection, bool checkCapture = false)
        {
            int xMod = 0;
            int yMod = 0;
            switch (lineDirection)
            {
                case LineDirection.DIAGONAL_DOWN:
                    xMod = -1;
                    yMod = -1;
                    break;
                case LineDirection.DIAGONAL_UP:
                    xMod = -1;
                    yMod = 1;
                    break;
                case LineDirection.VERTICAL:
                    yMod = -1;
                    break;
                case LineDirection.HORIZONTAL:
                    xMod = -1;
                    break;
            }
            if (x + xMod < 0 || x + xMod > 18 || y + yMod < 0 || y + yMod > 18) return new Vector2(x, y);
            if (board[x + xMod, y + yMod] == null) return new Vector2(x, y);

            bool currentIsBlack = board[x, y] == true;

            int cX = x;
            int cY = y;
            int consecutiveCount = (checkCapture) ? 0 : 1;
            do
            {
                if (cX + xMod < 0 || cX + xMod > 18 || cY + yMod < 0 || cY + yMod > 18) return new Vector2(cX, cY);

                if (board[cX + xMod, cY + yMod] == null) return new Vector2(cX, cY);

                if (checkCapture == true)
                {
                    if (consecutiveCount < 2 && board[cX + xMod, cY + yMod] != currentIsBlack) consecutiveCount++;
                    else if (consecutiveCount == 2 && board[cX + xMod, cY + yMod] == currentIsBlack) return new Vector2(cX + xMod, cY + yMod);
                    else return new Vector2(x, y);
                }

                if (checkCapture == false && board[cX + xMod, cY + yMod] != currentIsBlack) return new Vector2(cX, cY);

                cX += xMod;
                cY += yMod;
            } while (true);
        }

        public LineType CheckLine(int startX, int startY, LineDirection lineDirection)
        {
            if (board[startX, startY] == null) return LineType.NONE;

            int xMod = 0, yMod = 0;
            switch (lineDirection)
            {
                case LineDirection.DIAGONAL_DOWN:
                    xMod = 1;
                    yMod = 1;
                    break;
                case LineDirection.DIAGONAL_UP:
                    xMod = 1;
                    yMod = -1;
                    break;
                case LineDirection.VERTICAL:
                    yMod = 1;
                    break;
                case LineDirection.HORIZONTAL:
                    xMod = 1;
                    break;
            }
            if (startX + xMod < 0 || startX + xMod > 18 || startY + yMod < 0 || startY + yMod > 18) return LineType.NONE;

            if (board[startX + xMod, startY + yMod] == null) return LineType.NONE;

            bool currentIsBlack = board[startX, startY] == true;
            Vector2[] capCoords = new Vector2[2];
            int cX = startX, cY = startY;
            int currentCount = 1;

            do
            {
                if (cX + xMod < 0 || cX + xMod > 18 || cY + yMod < 0 || cY + yMod > 18) return LineType.NONE;

                if (board[cX + xMod, cY + yMod] == currentIsBlack && currentCount < 5) currentCount++;
                else
                {
                    switch (currentCount)
                    {
                        case 3:
                            return LineType.TRIA;
                        case 4:
                            return LineType.TESSERA;
                        case 5:
                            return LineType.WIN;
                        default:
                            return LineType.NONE;
                    }
                }

                cX += xMod;
                cY += yMod;
            } while (true);
        }

        public bool CheckCapture(int startX, int startY, LineDirection lineDirection)
        {
            if (board[startX, startY] == null) return false;

            int xMod = 0, yMod = 0;
            switch (lineDirection)
            {
                case LineDirection.DIAGONAL_DOWN:
                    xMod = 1;
                    yMod = 1;
                    break;
                case LineDirection.DIAGONAL_UP:
                    xMod = 1;
                    yMod = -1;
                    break;
                case LineDirection.VERTICAL:
                    yMod = 1;
                    break;
                case LineDirection.HORIZONTAL:
                    xMod = 1;
                    break;
            }
            if (startX + xMod < 0 || startX + xMod > 18 || startY + yMod < 0 || startY + yMod > 18) return false;

            if (board[startX + xMod, startY + yMod] == null) return false;

            bool currentIsBlack = board[startX, startY] == true;
            Vector2[] capCoords = new Vector2[2];
            int cX = startX, cY = startY;
            int currentCount = 0;

            do
            {
                if (cX + xMod < 0 || cX + xMod > 18 || cY + yMod < 0 || cY + yMod > 18) return false;

                if (board[cX + xMod, cY + yMod] == null) return false;
                if (board[cX + xMod, cY + yMod] != currentIsBlack && currentCount < 2)
                {
                    capCoords[currentCount] = new Vector2(cX + xMod, cY + yMod);
                    currentCount++;
                }
                else if (board[cX + xMod, cY + yMod] == currentIsBlack && currentCount == 2)
                {
                    foreach (var coordinate in capCoords)
                    {
                        RemoveStone((int)coordinate.X, (int)coordinate.Y);
                    }
                    return true;
                }
                else return false;

                cX += xMod;
                cY += yMod;
            } while (true);
        }

        public void RemoveStone(int x, int y)
        {
            board[x, y] = null;
        }
    }

    public enum LineDirection
    {
        DIAGONAL_DOWN,
        DIAGONAL_UP,
        VERTICAL,
        HORIZONTAL
    }

    public enum LineType
    {
        NONE,
        TRIA,
        TESSERA,
        WIN
    }
}
