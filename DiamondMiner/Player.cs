using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiamondMiner
{
    public class Player 
    {
        enum Direction
        {
            Up, Down, Left, Right
        }
        private Game1 game1;
        public Texture2D character;

        public char[,] level;

        private Point position;
        private Direction direction;
        private Vector2 directionVector;

        private int delta = 0;
        private int speed = 2;

        int vidas;
        int diamantes;
        int dinamites;

        public Player(Game1 game1, int x, int y) //Construtor chamado no construtor do level. 
        {
            position = new Point(x, y);
            this.game1 = game1;
        }


        public void LoadSprite() //Goes to game1 load
        {
            character = game1.Content.Load<Texture2D>("jogador");
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

        public void DrawPlayer(GameTime gameTime, SpriteBatch _spriteBatch) //Goes to game1 draw
        {
            Rectangle position = new Rectangle(0, 0, game1.tileSize, game1.tileSize); //Retangulo utilizado para desenhar as sprites,
            for (int x = 0; x < game1.currentlevel.matrix.GetLength(0); x++)
            {
                for (int y = 0; y < game1.currentlevel.matrix.GetLength(1); y++)
                {
                    position.X = x * game1.tileSize; //aqui entao estamos a mudar a posicao em que os retangulos sao desenhados
                    position.Y = y * game1.tileSize;
                    if (game1.currentlevel.matrix[x, y] == 'M') _spriteBatch.Draw(character, position, Color.White);
                }
            }
        }

        public void PlaceDinamite() //Goes to game1 update
        {

        }

        public bool CheckTerra(Point point)
        {
            if (level[point.X, point.Y] == '.') return true;
            else return false;
        }
    }
}
