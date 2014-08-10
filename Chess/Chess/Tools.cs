using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Drawing = System.Drawing;

namespace Chess
{
    public class Tools
    {
        public static void LoadChessPieces()
        {
            Drawing.Image img = Drawing.Bitmap.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", "spritesheet.jpg"));
            Drawing.Bitmap bmp = new Drawing.Bitmap(img);

            for (int i = 0; i < 12; i++)
            {
                Color[] clrs = new Color[64 * 64];
                for (int j = 0; j < clrs.Length; j++)
                    clrs[j] = bmp.GetPixel(j % 64 + (64 * (i % 6)), j / 64 + (64 * (i / 6))).ToXnaColor();

                ChessBoard.Texture[i] = new Texture2D(Chess.graphics.GraphicsDevice, 64, 64);
                ChessBoard.Texture[i].SetData(clrs);
            }
        }

        public static Texture2D GetTexture(int width, int height, Color color)
        {
            Texture2D temp = new Texture2D(Chess.graphics.GraphicsDevice, width, height);
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
