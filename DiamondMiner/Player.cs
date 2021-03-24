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
        public static Player    _instance;
        private       Texture2D character;
        private       Game1     game1;
        
        private Point     position;
        private Direction direction;
        private Vector2   directionVector;

        private int delta = 0;
        private int speed = 2;

        int vidas;
        int diamonds;
        int dinamites;

        public Player(Game1 game1, int x, int y) //Construtor chamado no construtor do level. 
        {
            if (_instance != null) throw new Exception("Player cons called twice");
            _instance      = this;
            this.game1     = game1;
            position       = new Point(x, y);


            vidas     = 3;
            diamonds  = 0;
            dinamites = 0;
        }  


        public static void LoadSprite() //Goes to game1 load
        {
            _instance.character = _instance.game1.Content.Load<Texture2D>("jogador");
        }

        public static void Movement(GameTime gameTime) //Goes to game1 update
        {
            if (_instance.delta > 0)
            {
                _instance.delta = (_instance.delta + _instance.speed) % _instance.game1.tileSize;
            }

            else
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

                //Opcoes de destino:
                // Destino = Pedra / Muro == no movement
                // Destino = Diamante / dinamite == movement, coletar ponto
                // Destino = terra = movement

                // Se o Destino é pedra ou muro, nao pode mover.
                if (_instance.game1.currentlevel.HasRock(_instance.position) || _instance.game1.currentlevel.HasWall(_instance.position))
                {
                    _instance.position.X = lastPosition.X;
                    _instance.position.Y = lastPosition.Y;
                }
                // Se o destino é diamante, recolhe diamante e move
                else if (_instance.game1.currentlevel.HasDiamond(_instance.position))
                {
                    _instance.game1.currentlevel.Diamonds.Remove(_instance.position);
                    _instance.diamonds++;
                }
                // Se o destino é dynamite, recolhe dinamite e move
                else if (_instance.game1.currentlevel.HasDynamite(_instance.position))
                {
                    _instance.game1.currentlevel.Diamonds.Remove(_instance.position);
                    _instance.dinamites++;
                }
                // Se o destino é terra, entao retira a terra
                else if (_instance.game1.currentlevel.DirtTile(_instance.position))
                {
                    _instance.game1.currentlevel.matrix[_instance.position.X, _instance.position.Y] = ' ';
                }
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
    }
}
