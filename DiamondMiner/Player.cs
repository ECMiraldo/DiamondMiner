﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondMiner
{
    class Player 
    {
        enum Direction
        {
            Up, Down, Left, Right
        }
        private Game1 game;
        private Texture2D character;

        private Point position;
        private Direction direction;
        private Vector2 directionVector;

        private int delta = 0;
        private int speed = 2;

        int vidas;
        int diamantes;
        int dinamites;

        public Player(Game1 game1, int x, int y)
        {
            position = new Point(x, y);
            game = game1;
        }


        public void LoadSprite() //Goes to game1 load
        {

        }

        public void Movement(GameTime gameTime) //Goes to game1 update
        {
            KeyboardState kState = Keyboard.GetState();
            Point lastPosition = position;

            if (kState.IsKeyDown(Keys.A))
            {
                position.X--;
                direction = Direction.Left;
                delta = speed;
                // directionVector = new Vector2(-1, 0);
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




        }

        public void DrawPlayer() //Goes to game1 draw
        {

        }

        public void PlaceDinamite() //Goes to game1 update
        {

        }


    }
}
