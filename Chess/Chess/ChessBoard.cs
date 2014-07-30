using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chess
{
    public enum ChessPiece
    {
        None = -1,
        Pawn = 0,
        Bishop = 1,
        Knight = 2,
        Rook = 3,
        Queen = 4,
        King = 5
    }
    public class ChessBoard
    {
        public static Point SelectedPiece = Point.Zero;
        public static Texture2D EmptyTile = Tools.GetTexture(50, 50, Color.White);
        public static Texture2D[] Texture = new Texture2D[12];
        public static Tile[,] Tiles = new Tile[8, 8];
        public static List<Point> AM = new List<Point>();

        public static void Reset()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Tiles[i, j] = new Tile(false, ChessPiece.None);

            Tiles[0, 0] = new Tile(false, ChessPiece.Rook);
            Tiles[7, 0] = new Tile(false, ChessPiece.Rook);
            Tiles[0, 7] = new Tile(true, ChessPiece.Rook);
            Tiles[7, 7] = new Tile(true, ChessPiece.Rook);


            Tiles[1, 0] = new Tile(false, ChessPiece.Knight);
            Tiles[6, 0] = new Tile(false, ChessPiece.Knight);
            Tiles[1, 7] = new Tile(true, ChessPiece.Knight);
            Tiles[6, 7] = new Tile(true, ChessPiece.Knight);


            Tiles[2, 0] = new Tile(false, ChessPiece.Bishop);
            Tiles[5, 0] = new Tile(false, ChessPiece.Bishop);
            Tiles[2, 7] = new Tile(true, ChessPiece.Bishop);
            Tiles[5, 7] = new Tile(true, ChessPiece.Bishop);


            Tiles[3, 0] = new Tile(false, ChessPiece.King);
            Tiles[4, 0] = new Tile(false, ChessPiece.Queen);
            Tiles[3, 7] = new Tile(true, ChessPiece.King);
            Tiles[4, 7] = new Tile(true, ChessPiece.Queen);


            for (int i = 0; i < 8; i++)
            {
                Tiles[i, 6] = new Tile(true, ChessPiece.Pawn);
                Tiles[i, 1] = new Tile(false, ChessPiece.Pawn);
            }
        }

        public static void Draw()
        {
            Rectangle MouseRect = new Rectangle(Game1.MouseState.X,Game1.MouseState.Y,1,1);

            if (Game1.MouseDown)
            {
                for (int i = 0; i < AM.Count; i++)
                {
                    if (new Rectangle(AM[i].X * 50, AM[i].Y * 50, 50, 50).Intersects(MouseRect))
                    {
                        {
                            Tile currt = Tiles[SelectedPiece.X, SelectedPiece.Y];
                            Tiles[AM[i].X, AM[i].Y] = new Tile(currt.White, currt.ChessPiece);
                            Tiles[SelectedPiece.X, SelectedPiece.Y] = new Tile(true);
                        }
                    }
                }
            }

            bool temp = false;
            AM = GetMoves(SelectedPiece.X, SelectedPiece.Y);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Tile t = Tiles[x, y];

                    Rectangle Location = new Rectangle(x * 50, y * 50, 50, 50);

                    Color drawingcolor = Color.Gray;
                    if ((x + y) % 2 == 0)
                    {
                        drawingcolor = Color.White;
                        if (temp)
                        {
                            drawingcolor = Color.Yellow;
                        }

                    }
                    else if (temp)
                        drawingcolor = new Color(191, 179, 86);

                    if (SelectedPiece != null)
                    {
                        if (t.ChessPiece != ChessPiece.None)
                        {
                            if (SelectedPiece == new Point(x, y) /*&& t.PieceColor == true*/)
                            {
                                drawingcolor = Color.Blue;
                            }
                        }
                    }

                    if (Location.Intersects(MouseRect))
                    {
                        if (t.ChessPiece != ChessPiece.None /*&& t.PieceColor == true*/)
                        {
                            if (Game1.MouseDown)
                            {
                                SelectedPiece = new Point(x, y);
                            }
                            drawingcolor = Color.Green;
                        }
                    }
                    if (AM.Contains(new Point(x, y)))
                    {
                        bool white = Tiles[SelectedPiece.X, SelectedPiece.Y].White;
                        if (Tiles[x, y].ChessPiece != ChessPiece.None && Tiles[x, y].White != white)
                            drawingcolor = Color.Red;
                        else
                        drawingcolor = Color.LightGreen;
                    }

                    Texture2D texture = GetTexture(t.ChessPiece, t.White);
                    Game1.spriteBatch.Draw(texture, new Rectangle(x * 50, y * 50, 50, 50), drawingcolor);
                }
            }
        }

        public static Texture2D GetTexture(ChessPiece piece, bool white)
        {
            if (piece == ChessPiece.None)
                return EmptyTile;
            int i = white ? 0 : 6;
            return Texture[(int)piece + i];
        }

        public static List<Point> GetMoves(int X, int Y)
        {
            List<Point> Moves = new List<Point>();
            Tile t = Tiles[X, Y];
            if (t.ChessPiece == ChessPiece.None)
                return Moves;

            switch (t.ChessPiece)
            {
                case ChessPiece.Pawn:
                    if (t.White)
                    {
                        if (Y > 0)
                        {
                            if (!(Tiles[X, Y - 1].ChessPiece != ChessPiece.None && !Tiles[X, Y - 1].White))
                                Moves.Add(new Point(X, Y - 1));

                            if (Y == 6)
                            {
                                if (Y == 6 && Tiles[X, Y - 1].ChessPiece == ChessPiece.None && Tiles[X, Y - 1].ChessPiece == ChessPiece.None)
                                    Moves.Add(new Point(X, Y - 2));
                            }
                            if (X > 0)
                            {
                                if (Tiles[X - 1, Y - 1].ChessPiece != ChessPiece.None && !Tiles[X - 1, Y - 1].White)
                                {
                                    Moves.Add(new Point(X - 1, Y - 1));
                                }
                            }
                            if (X < 7)
                            {
                                if (Tiles[X + 1, Y - 1].ChessPiece != ChessPiece.None && !Tiles[X + 1, Y - 1].White)
                                {
                                    Moves.Add(new Point(X + 1, Y - 1));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Y < 7)
                        {
                            if (!(Tiles[X, Y + 1].ChessPiece != ChessPiece.None && Tiles[X, Y + 1].White))
                                Moves.Add(new Point(X, Y + 1));
                            if (Y == 1)
                            {
                                if (Y == 1 && Tiles[X, Y + 1].ChessPiece == ChessPiece.None && Tiles[X, Y + 1].ChessPiece == ChessPiece.None)
                                    Moves.Add(new Point(X, Y + 2));
                            }
                            if (X > 0)
                            {
                                if (Tiles[X - 1, Y + 1].ChessPiece != ChessPiece.None && Tiles[X - 1, Y + 1].White)
                                {
                                    Moves.Add(new Point(X - 1, Y + 1));
                                }
                            }
                            if (X < 7)
                            {
                                if (Tiles[X + 1, Y + 1].ChessPiece != ChessPiece.None && Tiles[X + 1, Y + 1].White)
                                {
                                    Moves.Add(new Point(X + 1, Y + 1));
                                }
                            }
                        }
                    }
                    break;
                case ChessPiece.Knight:

                    break;
            }
            for (int i = 0; i < Moves.Count; i++)
            {
                if (Moves[i].X > 7 || Moves[i].X < 0 || Moves[i].Y > 7 || Moves[i].Y < 0)
                {
                    Moves.Remove(Moves[i]);
                    i--;
                }
            }
            return Moves;
        }
    }
}
