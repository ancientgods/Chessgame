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
using System.IO;
using Chess;
using Drawing = System.Drawing;

namespace Chess
{
    public class Tools
    {
        public static void LoadChessPieces()
        { 
            Drawing.Image img = Drawing.Bitmap.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "SpriteSheet", "ChessPieces.jpg"));
            Drawing.Bitmap bmp = new Drawing.Bitmap(img);

            for (int i = 0; i < 12; i++)
            {
                Color[] clrs = new Color[64 * 64];
                for (int j = 0; j < clrs.Length; j++)
                    clrs[j] = bmp.GetPixel(j % 64 + (64 * (i % 6)), j / 64 + (64 * (i / 6))).ToXnaColor();

                Texture2D texture = new Texture2D(Game1.graphics.GraphicsDevice, 64, 64);
                texture.SetData(clrs);
                ChessBoard.Texture[i] = texture;
            }
        }

        public static Texture2D GetTexture(int width, int height, Color color)
        {
            Texture2D temp = new Texture2D(Game1.graphics.GraphicsDevice, width, height);
            Color[] colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = color;
            temp.SetData(colors);
            return temp;
        }
    }
    public static class Extra
    {
        public static Color ToXnaColor(this Drawing.Color c)
        {
            return new Color(c.R, c.G, c.B);
        }
    }
}
