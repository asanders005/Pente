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
            board = new bool?[19,19];
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

        public Vector2 GetLineStart(int x, int y, LineDirection lineDirection)
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

            bool currentIsBlack = board[x, y] == true;

            int cX = x;
            int cY = y;
            bool? checkCapture = null;
            int consecutiveCount = 0;
            bool quit = false;
            do
            {
                if (board[cX + xMod, cY + yMod] == null) return new Vector2(cX, cY);

                if (checkCapture == null)
                {
                    checkCapture = (board[cX + xMod, cY + yMod] != currentIsBlack);
                }

                if (checkCapture == true)
                {
                    if (consecutiveCount < 2 && board[cX + xMod, cY + yMod] != currentIsBlack) consecutiveCount++;
                    else if (consecutiveCount == 2 && board[cX + xMod, cY + yMod] == currentIsBlack) return new Vector2(cX, cY);
                    else return new Vector2(x, y);
                }

                if (checkCapture == false && board[cX + xMod, cY + yMod] != currentIsBlack) return new Vector2(cX, cY);

                cX += xMod;
                cY += yMod;
            } while (!quit);


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
        CAPTURE,
        TRIA,
        TESSERA
    }
}
