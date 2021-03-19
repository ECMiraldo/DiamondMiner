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
        public static Player _instance;
        private Texture2D character;
        private Game1 game1;
        

        public char[,] level;

        private Point position;
        private Direction direction;
        private Vector2 directionVector;

        private int delta = 0;
        private int speed = 1;

        int vidas;
        int diamantes;
        int dinamites;

        public Player(Game1 game1, int x, int y) //Construtor chamado no construtor do level. 
        {
            if (_instance != null) throw new Exception("Player cons called twice");
            _instance = this;
            this.game1 = game1;
            position = new Point(x, y);
        }


        public static void LoadSprite() //Goes to game1 load
        {
            _instance.character = _instance.game1.Content.Load<Texture2D>("jogador");
        }

        public static void Movement(GameTime gameTime) //Goes to game1 update
        {
            KeyboardState kState = Keyboard.GetState();
            Point lastPosition = _instance.position;

            if (kState.IsKeyDown(Keys.A))
            {
                _instance.position.X--;
                _instance.direction = Direction.Left;
                _instance.delta = _instance.speed;
                _instance.directionVector = -Vector2.UnitX;
            }
            else if (kState.IsKeyDown(Keys.W))
            {
                _instance.position.Y--;
                _instance.direction = Direction.Up;
                _instance.delta = _instance.speed;
                _instance.directionVector = -Vector2.UnitY;
            }
            else if (kState.IsKeyDown(Keys.S))
            {
                _instance.position.Y++;
                _instance.direction = Direction.Down;
                _instance.delta = _instance.speed;
                _instance.directionVector = Vector2.UnitY;
            }
            else if (kState.IsKeyDown(Keys.D))
            {
                _instance.position.X++;
                _instance.direction = Direction.Right;
                _instance.delta = _instance.speed;
                _instance.directionVector = Vector2.UnitX;
            }




        }

        public static void DrawPlayer(GameTime gameTime, SpriteBatch _spriteBatch) //Goes to game1 draw
        {
            Rectangle pos = new Rectangle(_instance.position.X*_instance.game1.tileSize, _instance.position.Y*_instance.game1.tileSize, _instance.game1.tileSize, _instance.game1.tileSize);
            _spriteBatch.Draw(_instance.character, pos, Color.White);
        }

        //A ideia eu diria é ter dinamites no mapa, que o jogador apanha quando passa por cima e depois pode colocar ele ativo
        //atravess da tecla E, por exemplo.
        public void PlaceDinamite() //Goes to game1 update
        {

        }

        public bool CheckTerra(Point point)
        {
            return (level[point.X, point.Y] == '.');
        }
    }
}
