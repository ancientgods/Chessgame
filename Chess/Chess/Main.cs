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

namespace Chess
{
    public class Chess : Game
    {
        public static Texture2D Pointer;
        public static bool MouseDown;
        public static MouseState MouseState = Mouse.GetState();
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public Chess()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 400;
            graphics.ApplyChanges();
            Tools.LoadChessPieces();
            ChessBoard.Reset();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Pointer = Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "pointer");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();
            if (MouseState.LeftButton == ButtonState.Pressed)
                MouseDown = true;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            ChessBoard.Draw();

            spriteBatch.Draw(Pointer, new Rectangle(MouseState.X, MouseState.Y, 14, 22), Color.White);
            spriteBatch.End();

            MouseDown = false;
            base.Draw(gameTime);
        }
    }
}
