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
        public static Point SelectedPiece = new Point(4, 4);
        public static Texture2D EmptyTile = Tools.GetTexture(50, 50, Color.White);
        public static Texture2D[] Texture = new Texture2D[12];
        public static Tile[,] Tiles = new Tile[8, 8];
        public static List<Point> AM = new List<Point>();

        public static void Reset()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tiles[i, j] = new Tile(false);
                }
            }

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
            Rectangle MouseRect = new Rectangle(Chess.MouseState.X, Chess.MouseState.Y, 1, 1);
            AM = GetMoves(SelectedPiece.X, SelectedPiece.Y);


            if (Chess.MouseDown)
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
                    }

                    if (SelectedPiece != null)
                    {
                        if (t.ChessPiece != ChessPiece.None)
                        {
                            if (SelectedPiece == new Point(x, y))
                            {
                                drawingcolor = Color.DarkGreen;
                            }
                        }
                    }

                    if (Location.Intersects(MouseRect))
                    {
                        if (t.ChessPiece != ChessPiece.None)
                        {
                            if (Chess.MouseDown)
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
                        {
                            if ((x + y) % 2 == 0)
                                drawingcolor = Color.IndianRed;
                            else
                                drawingcolor = Color.DarkRed;
                        }
                        else
                        {
                            if ((x + y) % 2 == 0)
                                drawingcolor = Color.Yellow;
                            else
                                drawingcolor = new Color(191, 179, 86);
                        }
                    }

                    Texture2D texture = GetTexture(t.ChessPiece, t.White);
                    Chess.spriteBatch.Draw(texture, new Rectangle(x * 50, y * 50, 50, 50), drawingcolor);
                }
            }
        }

        public static Texture2D GetTexture(ChessPiece piece, bool white)
        {
            if (piece == ChessPiece.None)
                return EmptyTile;
            return Texture[(int)piece + (white ? 0 : 6)];
        }

        public static List<Point> GetMoves(int X, int Y)
        {
            List<Point> Moves = new List<Point>();
            Tile t = Tiles[X, Y];
            bool CurrWhite = t.White;
            if (t.ChessPiece == ChessPiece.None)
                return Moves;

            switch (t.ChessPiece)
            {
                case ChessPiece.Pawn:
                    #region Pawn
                    if(t.White)
                    {
                        if (Y > 0)
                        {
                            if (Tiles[X, Y - 1].ChessPiece == ChessPiece.None)
                                Moves.Add(new Point(X, Y - 1));

                            if (Y == 6 && Tiles[X, Y - 2].ChessPiece == ChessPiece.None)
                            {
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
                            if (Tiles[X, Y + 1].ChessPiece == ChessPiece.None)
                                Moves.Add(new Point(X, Y + 1));

                            if (Y == 1 && Tiles[X, Y + 1].ChessPiece == ChessPiece.None)
                            {
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
                    #endregion Pawn
                    break;
                case ChessPiece.Knight:
                    #region Knight
                    if (X < 6 && Y > 0)
                    {
                        if (!(Tiles[X + 2, Y - 1].ChessPiece != ChessPiece.None && Tiles[X + 2, Y - 1].White==CurrWhite))
                        {
                            Moves.Add(new Point(X + 2, Y - 1));
                        }
                    }
                    if (X > 1 && Y > 0)
                    {
                        if (!(Tiles[X - 2, Y - 1].ChessPiece != ChessPiece.None && Tiles[X - 2, Y - 1].White==CurrWhite))
                        {
                            Moves.Add(new Point(X - 2, Y - 1));
                        }
                    }
                    if (X < 6 && Y < 7)
                    {
                        if (!(Tiles[X + 2, Y + 1].ChessPiece != ChessPiece.None && Tiles[X + 2, Y + 1].White==CurrWhite))
                        {
                            Moves.Add(new Point(X + 2, Y + 1));
                        }
                    }
                    if (X > 1 && Y < 7)
                    {
                        if (!(Tiles[X - 2, Y + 1].ChessPiece != ChessPiece.None && Tiles[X - 2, Y + 1].White==CurrWhite))
                        {
                            Moves.Add(new Point(X - 2, Y + 1));
                        }
                    }
                    if (X > 0 && Y < 6)
                    {
                        if (!(Tiles[X - 1, Y + 2].ChessPiece != ChessPiece.None && Tiles[X - 1, Y + 2].White==CurrWhite))
                        {
                            Moves.Add(new Point(X - 1, Y + 2));
                        }
                    }
                    if (X > 0 && Y > 1)
                    {
                        if (!(Tiles[X - 1, Y - 2].ChessPiece != ChessPiece.None && Tiles[X - 1, Y - 2].White==CurrWhite))
                        {
                            Moves.Add(new Point(X - 1, Y - 2));
                        }
                    }
                    if (X < 7 && Y < 6)
                    {
                        if (!(Tiles[X + 1, Y + 2].ChessPiece != ChessPiece.None && Tiles[X + 1, Y + 2].White==CurrWhite))
                        {
                            Moves.Add(new Point(X + 1, Y + 2));
                        }
                    }
                    if (X < 7 && Y > 1)
                    {
                        if (!(Tiles[X + 1, Y - 2].ChessPiece != ChessPiece.None && Tiles[X + 1, Y - 2].White == CurrWhite))
                        {
                            Moves.Add(new Point(X + 1, Y - 2));
                        }
                    }
                    #endregion Knight
                    break;
                case ChessPiece.King:
                    #region King
                    if (Y > 0)
                    {
                        if (!(Tiles[X, Y - 1].ChessPiece != ChessPiece.None && Tiles[X, Y - 1].White == CurrWhite))
                        {
                            Moves.Add(new Point(X, Y - 1));
                        }

                        if (X < 7)
                        {
                            if (!(Tiles[X + 1, Y - 1].ChessPiece != ChessPiece.None && Tiles[X + 1, Y - 1].White == CurrWhite))
                            {
                                Moves.Add(new Point(X + 1, Y - 1));
                            }
                        }
                        if (X > 0)
                        {
                            if (!(Tiles[X - 1, Y - 1].ChessPiece != ChessPiece.None && Tiles[X - 1, Y - 1].White == CurrWhite))
                            {
                                Moves.Add(new Point(X - 1, Y - 1));
                            }
                        }
                    }
                    if (Y < 7)
                    {
                        if (!(Tiles[X, Y + 1].ChessPiece != ChessPiece.None && Tiles[X, Y + 1].White == CurrWhite))
                        {
                            Moves.Add(new Point(X, Y + 1));
                        }

                        if (X < 7)
                        {
                            if (!(Tiles[X + 1, Y + 1].ChessPiece != ChessPiece.None && Tiles[X + 1, Y + 1].White == CurrWhite))
                            {
                                Moves.Add(new Point(X + 1, Y + 1));
                            }
                        }

                        if (X > 0)
                        {
                            if (!(Tiles[X - 1, Y + 1].ChessPiece != ChessPiece.None && Tiles[X - 1, Y + 1].White == CurrWhite))
                            {
                                Moves.Add(new Point(X - 1, Y + 1));
                            }
                        }
                    }

                    if (X > 0)
                    {
                        if (!(Tiles[X - 1, Y].ChessPiece != ChessPiece.None && Tiles[X - 1, Y].White == CurrWhite))
                        {
                            Moves.Add(new Point(X - 1, Y));
                        }
                    }

                    if (X < 7)
                    {
                        if (!(Tiles[X + 1, Y].ChessPiece != ChessPiece.None && Tiles[X + 1, Y].White == CurrWhite))
                        {
                            Moves.Add(new Point(X + 1, Y));
                        }
                    }
                    #endregion King
                    break;
                case ChessPiece.Rook:
                    #region Rook
                    for (int i = X + 1; i < 8; i++)
                    {
                        if (Tiles[i, Y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(i, Y));
                        else
                        {
                            if (Tiles[i, Y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(i, Y));
                                break;
                            }
                        }
                    }
                    for (int i = X - 1; i >= 0; i--)
                    {
                        if (Tiles[i, Y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(i, Y));
                        else
                        {
                            if (Tiles[i, Y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(i, Y));
                                break;
                            }
                        }
                    }
                    for (int i = Y + 1; i < 8; i++)
                    {
                        if (Tiles[X, i].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(X, i));
                        else
                        {
                            if (Tiles[X, i].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(X, i));
                                break;
                            }
                        }
                    }
                    for (int i = Y - 1; i >= 0; i--)
                    {
                        if (Tiles[X, i].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(X, i));
                        else
                        {
                            if (Tiles[X, i].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(X, i));
                                break;
                            }
                        }
                    }

                    #endregion Rook
                    break;
                case ChessPiece.Queen:
                    #region Queen
                    for (int i = X + 1; i < 8; i++)
                    {
                        if (Tiles[i, Y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(i, Y));
                        else
                        {
                            if (Tiles[i, Y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(i, Y));
                                break;
                            }
                        }
                    }
                    for (int i = X - 1; i >= 0; i--)
                    {
                        if (Tiles[i, Y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(i, Y));
                        else
                        {
                            if (Tiles[i, Y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(i, Y));
                                break;
                            }
                        }
                    }
                    for (int i = Y + 1; i < 8; i++)
                    {
                        if (Tiles[X, i].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(X, i));
                        else
                        {
                            if (Tiles[X, i].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(X, i));
                                break;
                            }
                        }
                    }
                    for (int i = Y - 1; i >= 0; i--)
                    {
                        if (Tiles[X, i].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(X, i));
                        else
                        {
                            if (Tiles[X, i].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(X, i));
                                break;
                            }
                        }
                    }
                    for (int x = X + 1, y = Y + 1; x < 8 && y < 8; x++, y++)
                    {
                        if (Tiles[x, y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(x, y));
                        else
                        {
                            if (Tiles[x, y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(x, y));
                                break;
                            }
                        }
                    }
                    for (int x = X - 1, y = Y + 1; x >= 0 && y < 8; x--, y++)
                    {
                        if (Tiles[x, y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(x, y));
                        else
                        {
                            if (Tiles[x, y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(x, y));
                                break;
                            }
                        }
                    }
                    for (int x = X - 1, y = Y - 1; x >= 0 && y >= 0; x--, y--)
                    {
                        if (Tiles[x, y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(x, y));
                        else
                        {
                            if (Tiles[x, y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(x, y));
                                break;
                            }
                        }
                    }
                    for (int x = X + 1, y = Y - 1; x < 8 && y >= 0; x++, y--)
                    {
                        if (Tiles[x, y].ChessPiece == ChessPiece.None)
                            Moves.Add(new Point(x, y));
                        else
                        {
                            if (Tiles[x, y].White == CurrWhite)
                                break;
                            else
                            {
                                Moves.Add(new Point(x, y));
                                break;
                            }
                        }
                    }
                    #endregion Queen
                    break;

            }
            return Moves;
        }
    }
}
