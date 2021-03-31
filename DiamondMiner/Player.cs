﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;


namespace DiamondMiner
{
    public class Player 
    {
        enum Direction
        {
            Up, Down, Left, Right
        }

        public static Player    _instance;
        public bool explosion;
        private Texture2D[] character;
        public       Game1     game1;
        
        public  Point     position;
        private Direction direction;
        private Vector2   directionVector;

        public int delta = 0;
        private int speed = 1;

        public int vidas;
        public int diamonds;
        public int dinamites;


        public Player(Game1 game) //Construtor chamado no construtor do level. 
        {
            if (_instance != null) throw new Exception("Player cons called twice");
            _instance      = this;
            this.game1     = game;
            position       = new Point(0, 0);  //Default pos
            explosion      = false;

            vidas     = 3;
            diamonds  = 0;
            dinamites = 1;
        }  

        public static void LoadSprite() //Goes to game1 load
        {
            _instance.character = new Texture2D[13];

            _instance.character[0]  = _instance.game1.Content.Load<Texture2D>("Walk (1)");
            _instance.character[1]  = _instance.game1.Content.Load<Texture2D>("Walk (2)");
            _instance.character[2]  = _instance.game1.Content.Load<Texture2D>("Walk (3)");
            _instance.character[3]  = _instance.game1.Content.Load<Texture2D>("Walk (4)");
            _instance.character[4]  = _instance.game1.Content.Load<Texture2D>("Walk (5)");
            _instance.character[5]  = _instance.game1.Content.Load<Texture2D>("Walk (6)");
            _instance.character[6]  = _instance.game1.Content.Load<Texture2D>("Walk (7)");
            _instance.character[7]  = _instance.game1.Content.Load<Texture2D>("Walk (8)");
            _instance.character[8]  = _instance.game1.Content.Load<Texture2D>("Walk (9)");
            _instance.character[9]  = _instance.game1.Content.Load<Texture2D>("Walk (10)");
            _instance.character[10] = _instance.game1.Content.Load<Texture2D>("Walk (11)");
            _instance.character[11] = _instance.game1.Content.Load<Texture2D>("Walk (12)");
            _instance.character[12] = _instance.game1.Content.Load<Texture2D>("Walk (13)");
        }

        public static void Movement(GameTime gametime)
        {
            _instance.Movement2(gametime);
        }
        public void Movement2(GameTime gameTime) //Goes to game1 update
        {
            if (delta > 0)
            {
                delta = (delta + speed) % game1.tileSize;
            }

            else
            {
                KeyboardState kState = Keyboard.GetState();
                Point lastPosition = position;
                if (kState.IsKeyDown(Keys.A))
                {
                    position.X--;
                    direction = Direction.Left;
                    delta = speed;
                    directionVector = -Vector2.UnitX;
                }
                else if (kState.IsKeyDown(Keys.W))
                {
                    position.Y--;
                    direction = Direction.Up;
                    delta = speed;
                    directionVector = -Vector2.UnitY;
                }
                else if (kState.IsKeyDown(Keys.S))
                {
                    position.Y++;
                    direction = Direction.Down;
                    delta = speed;
                    directionVector = Vector2.UnitY;
                }
                else if (kState.IsKeyDown(Keys.D))
                {
                    position.X++;
                    direction = Direction.Right;
                    delta = speed;
                    directionVector = Vector2.UnitX;
                }

                if (!game1.currentlevel.InMatrix(position))
                {
                    position = lastPosition;
                    delta = 0;
                }
                //Opcoes de destino:
                // Destino = Pedra / Muro == no movement
                // Destino = Diamante / dinamite == movement, coletar ponto
                // Destino = terra = movement

                // Se o Destino é pedra ou muro, nao pode mover.
                if (game1.currentlevel.HasRock(position) || game1.currentlevel.HasWall(position))
                {
                    position = lastPosition;
                    delta = 0;
                }
                // Se o destino é diamante, recolhe diamante e move
                else if (game1.currentlevel.HasDiamond(position))
                {
                    game1.currentlevel.Diamonds.Remove(position);
                    diamonds++;
                }
                // Se o destino é dynamite, recolhe dinamite e move
                else if (game1.currentlevel.HasDynamite(position))
                {
                    game1.currentlevel.Diamonds.Remove(position);
                    dinamites++;
                }
                // Se o destino é terra, entao retira a terra
                else if (game1.currentlevel.DirtTile(position))
                {
                    game1.currentlevel.matrix[position.X, position.Y] = ' ';
                }
            }
        }


        public static void DrawPlayer(GameTime gameTime, SpriteBatch _spritebatch)
        {
            _instance.DrawPlayer2(gameTime, _spritebatch);
        }
        public void DrawPlayer2(GameTime gameTime, SpriteBatch _spriteBatch) //Goes to game1 draw
        {
            float rotation = 0;
            Vector2 scale = new Vector2(2, 2);
            Vector2 origin = Vector2.Zero;


            Vector2 pos = position.ToVector2() * game1.tileSize;
            int frame = 0;
            if (delta > 0)
            {
                pos -= (game1.tileSize - delta) * directionVector;
                float animSpeed = 4f;
                frame = (int)((delta / speed) / animSpeed);
            }
            if (direction == Direction.Left)
            {
                Rectangle rect = new Rectangle(pos.ToPoint(), new Point(game1.tileSize));
                _spriteBatch.Draw(character[frame], rect, null, Color.White,rotation,origin,SpriteEffects.FlipHorizontally,0);
            }
            else
            {
                Rectangle rect = new Rectangle(pos.ToPoint(), new Point(game1.tileSize));
                _spriteBatch.Draw(character[frame], rect, Color.White);
            }
        }

        public int GetVidas()
        {
            return vidas;
        }

        public static Point GetPosition()
        {
            return _instance.position;
        }
    }
}
