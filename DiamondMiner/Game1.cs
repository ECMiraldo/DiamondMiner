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


        public Level       currentlevel;
        public SpriteBatch _spriteBatch;
        public int         tileSize = 32;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        { 
            //Constructors
            currentlevel = new Level(this, "Level1.txt");

            //Grahics config
            _graphics.PreferredBackBufferHeight = tileSize * (1 + currentlevel.matrix.GetLength(1));
            _graphics.PreferredBackBufferWidth = tileSize * currentlevel.matrix.GetLength(0);
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            currentlevel.LoadLevelContent();
            Player.LoadSprite();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player.Movement(gameTime); //Notice how player.movement actualizes level.draw
            currentlevel.RockGravity(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();
            currentlevel.DrawLevel (gameTime, _spriteBatch);
                  Player.DrawPlayer(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
