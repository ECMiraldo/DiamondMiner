using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace DiamondMiner
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public Level currentlevel;
        public Player player;


        public  SpriteBatch _spriteBatch;
        public int tileSize = 32;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentlevel = new Level(this, "Level1.txt");
            _graphics.PreferredBackBufferHeight = tileSize * (1 + currentlevel.matrix.GetLength(1));
            _graphics.PreferredBackBufferWidth = tileSize * currentlevel.matrix.GetLength(0);
            _graphics.ApplyChanges();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            currentlevel.LoadLevelContent();
            player.LoadSprite();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Movement(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            currentlevel.DrawLevel(gameTime, _spriteBatch);
            player.DrawPlayer(gameTime, _spriteBatch);
            //player.DrawPlayer(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
