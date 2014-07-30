using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public class Tile
    {
        /*public int X;
        public int Y;*/
        //public PieceColor PieceColor;
        public bool White;
        public ChessPiece ChessPiece;
      
        public Tile(/*int x, int y,*/bool white, ChessPiece chesspiece = ChessPiece.None)
        {
            /*X = x;
            Y = y;*/
            White = white;
            ChessPiece = chesspiece;
        }
    }
}
