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
        public  SpriteBatch _spriteBatch;
        public  int tileSize = 32;

        public Level       currentlevel;
        private string[]   totalLevels = { "Level1.txt" , "Level2.txt", "Level3.txt" };
        public int         level = 0;

        Texture2D levelComplete, dieScreen, pressEsc, pressEnter,gameComplete;
        
        public Game1()
        {
            Player player = new Player(this);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        { 
            //Constructors
            currentlevel = new Level(this, totalLevels[level]);

            //Grahics config
            _graphics.PreferredBackBufferHeight = tileSize * ( + currentlevel.matrix.GetLength(1));
            _graphics.PreferredBackBufferWidth  = tileSize * currentlevel.matrix.GetLength(0);
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            currentlevel.LoadLevelContent();
            Player.LoadSprite();
            levelComplete = Content.Load<Texture2D>("LevelComplete");
            dieScreen = Content.Load<Texture2D>("DieScreen");
            gameComplete = Content.Load<Texture2D>("GameComplete");
            pressEnter = Content.Load<Texture2D>("PressEnter");
            pressEsc = Content.Load<Texture2D>("PressEsc");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.R)) Initialize();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //Level Complete
                if (currentlevel.WinCondition())
                {
                    Player._instance.dinamites++;
                    level += 1;
                    Initialize();
                }
                //Defeat Restart
                else if (Player._instance.vidas <= 0)
                {
                    Initialize();
                    Player._instance.vidas = 3;
                    if (Player._instance.dinamites == 0) Player._instance.dinamites = 1;
                }
                //Win Restart/Exit
                else if (level == totalLevels.Length)
                {
                    if (currentlevel.WinCondition())
                    {
                        Initialize();
                        Player._instance.vidas = 3;
                    }

                }
            }

            Player.Movement(gameTime); //Notice how player.movement actualizes level.draw
            currentlevel.RockGravity(gameTime);
            currentlevel.PlaceDinamite(gameTime);
            currentlevel.ExDynamite(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();
            currentlevel.DrawLevel (gameTime, _spriteBatch);
                  Player.DrawPlayer(gameTime, _spriteBatch);

            //Level Complete UI
            if (currentlevel.WinCondition() && level < totalLevels.Length)
            {
                Vector2 windowSize = new Vector2(
                   _graphics.PreferredBackBufferWidth,
                   _graphics.PreferredBackBufferHeight);
                // Transparent Layer
                Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
                _spriteBatch.Draw(pixel,
                    new Rectangle(Point.Zero, windowSize.ToPoint()),
                    new Color(Color.DarkGray, 0.5f));

                // Draw Win Message
                
                
                _spriteBatch.Draw(levelComplete, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (4.5f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) - tileSize), Color.White);

            }

            //Defeat UI
            
            if (Player._instance.vidas <= 0)
            {
                Player.DrawPlayer(gameTime, _spriteBatch);
                Vector2 windowSize2 = new Vector2(
                   _graphics.PreferredBackBufferWidth,
                   _graphics.PreferredBackBufferHeight);
                // Transparent Layer
                Texture2D pixel2 = new Texture2D(GraphicsDevice, 1, 1);
                pixel2.SetData(new[] { Color.White });
                _spriteBatch.Draw(pixel2,
                    new Rectangle(Point.Zero, windowSize2.ToPoint()),
                    new Color(Color.Black, 0.9f));

                // Draw Win Message


                _spriteBatch.Draw(dieScreen, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (4.5f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) - (2*tileSize)), Color.White);
                _spriteBatch.Draw(pressEnter, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (3f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) + tileSize), Color.White);
                _spriteBatch.Draw(pressEsc, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (3f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) + 2*tileSize), Color.White);

            }
            // Game Compleye UI

            if (level == totalLevels.Length)
            {
                 if(currentlevel.WinCondition())
                 {
                    Vector2 windowSize2 = new Vector2(
                     _graphics.PreferredBackBufferWidth,
                     _graphics.PreferredBackBufferHeight);
                    // Transparent Layer
                    Texture2D pixel2 = new Texture2D(GraphicsDevice, 1, 1);
                    pixel2.SetData(new[] { Color.White });
                    _spriteBatch.Draw(pixel2,
                        new Rectangle(Point.Zero, windowSize2.ToPoint()),
                        new Color(Color.Black, 0.9f));

                    // Draw Win Message


                    _spriteBatch.Draw (gameComplete, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (4.5f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) - tileSize), Color.White);
                    _spriteBatch.Draw(pressEnter, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (3f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) + tileSize), Color.White);
                    _spriteBatch.Draw(pressEsc, new Vector2((_graphics.PreferredBackBufferWidth / 2f) - (3f * tileSize), (_graphics.PreferredBackBufferHeight / 2f) + 2 * tileSize), Color.White);

                 }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
