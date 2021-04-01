using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Timers;


namespace DiamondMiner
{
    public class Level
    {

        private Texture2D diamond, rocks, dirt, dynamite, wall, presentUI;
        private Texture2D[] explosionAnim;

        private Game1  game1;
        private SpriteFont arial12;
        private double timer;
        public char[,]     matrix;
        public List<Point> Rocks;
        public List<Point> Diamonds;
        public List<Point> Dynamite;

        int delta = 0;
        int speed = 2;
        private Vector2 rockvec;

        private bool explosion;
        private Point explosionPos;
        //Basicamente, tudo o que este construtor faz é prencher a matriz do level, e adicionar os Diamantes e Dynamites nas respectivas listas.
        public Level(Game1 game1, string levelFile) //Goes to game1 Initialize
        {
            this.game1 = game1;

            explosionAnim = new Texture2D[11];
            explosion     = false;

            Diamonds = new List<Point>();
            Rocks    = new List<Point>();
            Dynamite = new List<Point>();

            string[] linhas = File.ReadAllLines($"Content/{levelFile}");  // "Content/" + level
            int nrLinhas    = linhas.Length;
            int nrColunas   = linhas[0].Length;

            matrix = new char[nrColunas, nrLinhas];
            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    if (linhas[y][x] == '$')
                    {
                        Diamonds.Add(new Point(x, y));
                        //matrix[x, y] = ' '; // put a blank instead of the diamond ' '
                    }
                    else if (linhas[y][x] == 'M')
                    {
                        Player._instance.position = new Point(x, y);
                        matrix[x,y] = ' '; 
                    }
                    else if (linhas[y][x] == 'D')
                    {
                        Dynamite.Add(new Point(x, y));
                        //matrix[x, y] = ' ';
                    }
                    else if (linhas[y][x] == '*')
                    {
                        Rocks.Add(new Point(x, y));
                        //matrix[x, y] = ' ';
                    }
                    else
                    {
                        matrix[x, y] = linhas[y][x];
                    }
                }
            }
        }
       
        public void LoadLevelContent() //Goes to game1 load
        {
            diamond   = game1.Content.Load<Texture2D>("Gift");
            rocks     = game1.Content.Load<Texture2D>("Stone");
            dirt      = game1.Content.Load<Texture2D>("ground_snow");
            wall      = game1.Content.Load<Texture2D>("IceBox");
            dynamite  = game1.Content.Load<Texture2D>("candytnt");
            arial12   = game1.Content.Load<SpriteFont>("arial12");
            presentUI = game1.Content.Load<Texture2D>("Gift");


            explosionAnim[0]  = game1.Content.Load<Texture2D>("explosion1");
            explosionAnim[1]  = game1.Content.Load<Texture2D>("explosion2");
            explosionAnim[2]  = game1.Content.Load<Texture2D>("explosion3");
            explosionAnim[3]  = game1.Content.Load<Texture2D>("explosion4");
            explosionAnim[4]  = game1.Content.Load<Texture2D>("explosion5");
            explosionAnim[5]  = game1.Content.Load<Texture2D>("explosion6");
            explosionAnim[6]  = game1.Content.Load<Texture2D>("explosion7");
            explosionAnim[7]  = game1.Content.Load<Texture2D>("explosion8");
            explosionAnim[8]  = game1.Content.Load<Texture2D>("explosion9");
            explosionAnim[9]  = game1.Content.Load<Texture2D>("explosion10");
            explosionAnim[10] = game1.Content.Load<Texture2D>("explosion11");

            presentUI = game1.Content.Load<Texture2D>("Gift");

        }

        public void DrawLevel(GameTime gametime, SpriteBatch _spriteBatch ) //Goes to game1 draw
        {

            Rectangle position = new Rectangle(0, 0, game1.tileSize, game1.tileSize); //Retangulo utilizado para desenhar as sprites,
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    position.X = x * game1.tileSize; //aqui entao estamos a mudar a posicao em que os retangulos sao desenhados
                    position.Y = y * game1.tileSize;
                    switch (matrix[x, y])
                    {
                        case '#':
                            _spriteBatch.Draw(wall, position, Color.White);
                            break;
                        case '.':
                            _spriteBatch.Draw(dirt, position, Color.White);
                            break;
                        case 'E':
                            _spriteBatch.Draw(dynamite, position, Color.White);
                            break;
                    }
                }
            }

            // Draw the diamonds
            foreach (Point b in Diamonds)
            {
                position.X = b.X * game1.tileSize;
                position.Y = b.Y * game1.tileSize;
                _spriteBatch.Draw(diamond, position, Color.White);
            }

            // Draw the dynamites
            foreach (Point d in Dynamite)
            {
                position.X = d.X * game1.tileSize;
                position.Y = d.Y * game1.tileSize;
                _spriteBatch.Draw(dynamite, position, Color.White);
            }
            //rocks
            foreach (Point r in Rocks)
            {
                Point pixelpos = r; pixelpos.X = game1.tileSize * pixelpos.X ; pixelpos.Y = game1.tileSize* pixelpos.Y;
                Rectangle rect = new Rectangle(pixelpos, new Point(game1.tileSize));
                _spriteBatch.Draw(rocks, rect, Color.White);

            }

            //explosion
            if (explosion)
            {
                Point pos = new Point((explosionPos.X - 1) * game1.tileSize, (explosionPos.Y - 1) * game1.tileSize);
                Rectangle rect = new Rectangle(pos, new Point(game1.tileSize * 3));
                
                if (timer > 2.5)
                {
                    timer = 1.08 * timer;
                    if (timer < 11)
                    {
                        _spriteBatch.Draw(explosionAnim[(int)timer], rect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);

                    }
                }
            }

            //Draw UI
            //present
            string diamondsUI = $"{ Diamonds.Count}";
            Point measure = arial12.MeasureString(diamondsUI).ToPoint();
            int posX = matrix.GetLength(0) * game1.tileSize - measure.X - 5;
            _spriteBatch.DrawString(
                arial12,
                diamondsUI,
                new Vector2(posX, matrix.GetLength(1) * game1.tileSize - 15),
                Color.Coral);
            int posX2 = matrix.GetLength(0) * game1.tileSize - measure.X - 34;
            _spriteBatch.Draw(presentUI, new Vector2(posX2, matrix.GetLength(1) * game1.tileSize - 20), Color.DarkRed);

            //lifes
            string lifesUI = $"{ Player._instance.GetVidas()}";
            int posX3 = matrix.GetLength(0) * game1.tileSize - measure.X - 55;
            _spriteBatch.DrawString(
                arial12,
                lifesUI,
                new Vector2(posX3, matrix.GetLength(1) * game1.tileSize - 15),
                Color.Coral);
        }

        public bool HasRock(Point p)     => Rocks.Contains(p);
        public bool HasWall(Point p)     => matrix[p.X, p.Y] == '#';
        public bool HasDynamite(Point p) => Dynamite.Contains(p);
        public bool HasDiamond(Point p)  => Diamonds.Contains(p);
        public bool EmptyTile(Point p)   => (InMatrix(p) &&  matrix[p.X, p.Y] == ' ');
        public bool DirtTile(Point p)    => (InMatrix(p) && (matrix[p.X, p.Y] == '.'));
        public bool InMatrix(Point p)    => ((p.X >= 0 && p.Y >= 0) && (p.X < matrix.GetLength(0) && p.Y < matrix.GetLength(1)));
        public void RockGravity(GameTime gametime)
        {
            if (delta > 0)
            {
                delta = (delta + speed) % game1.tileSize;
            }
            else
            {
                for (int i = 0; i < Rocks.Count; i++)
                {
                    Point origin = Rocks[i];
                    Point Down = new Point(Rocks[i].X, Rocks[i].Y + 1);
                    if (HasRock(Down) || HasDiamond(Down) || HasDynamite(Down))
                    {
                        Point left = new Point(Down.X - 1, Down.Y);
                        if (EmptyTile(left))
                        {
                            if (left == Player._instance.position)
                            {
                                Player._instance.position.Y -= 1;
                                Player._instance.vidas--;
                            }
                            rockvec = Vector2.UnitY - Vector2.UnitX;
                            Rocks[i] = Rocks[i] + rockvec.ToPoint();
                            delta = speed;
                            matrix[origin.X, origin.Y] = ' ';
                            matrix[Rocks[i].X, Rocks[i].Y] = '*';
                        }
                        Point right = new Point(Down.X + 1, Down.Y);
                        if (EmptyTile(right))
                        {
                            if (right == Player._instance.position)
                            {
                                Player._instance.position.Y -= 1;
                                Player._instance.vidas--;
                            }
                            rockvec = Vector2.UnitY + Vector2.UnitX;
                            Rocks[i] = rockvec.ToPoint();
                            delta = speed;
                            matrix[origin.X, origin.Y] = ' ';
                            matrix[Rocks[i].X, Rocks[i].Y] = '*';
                        }
                    }
                    else if (EmptyTile(Down)) //se nao houver terra em baixo ela cai
                    {
                        if (Down == Player._instance.position)
                        {
                            Player._instance.position.Y -= 1;
                            Player._instance.vidas--;
                        }
                        rockvec = Vector2.UnitY;
                        Rocks[i] = Rocks[i] + rockvec.ToPoint();
                        delta = speed;
                        matrix[origin.X, origin.Y] = ' ';
                        matrix[Rocks[i].X, Rocks[i].Y] = '*';
                    }
                    
                }
            }
        }

        public void PlaceDinamite(GameTime gameTime) //Goes to game1 update
        {
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.E) && Player._instance.dinamites > 0)
            {
                timer = 0;
                Player._instance.dinamites--;
                Player._instance.game1.currentlevel.matrix[Player._instance.position.X, Player._instance.position.Y] = 'E';
                explosion = true;
                explosionPos = Player.GetPosition();         
            }
        }

        public void ExDynamite(GameTime gameTime)
        {
            if (explosion)
            {
                int x = explosionPos.X;
                int y = explosionPos.Y;


                timer = timer + gameTime.ElapsedGameTime.TotalSeconds;
                Console.Write(timer);
                List<Point> ExplosionRadius = new List<Point>();
                ExplosionRadius.Add(new Point(x, y));
                ExplosionRadius.Add(new Point(x - 1, y));
                ExplosionRadius.Add(new Point(x + 1, y));
                ExplosionRadius.Add(new Point(x, y + 1));
                ExplosionRadius.Add(new Point(x, y - 1));
                ExplosionRadius.Add(new Point(x + 1, y + 1));
                ExplosionRadius.Add(new Point(x - 1, y - 1));
                ExplosionRadius.Add(new Point(x + 1, y - 1));
                ExplosionRadius.Add(new Point(x - 1, y + 1));
                if (timer > 2.5f)
                {
                    if (ExplosionRadius.Contains(Player._instance.position)) Player._instance.vidas--;
                    foreach (Point p in ExplosionRadius)
                    {
                        matrix[x, y] = ' ';
                        matrix[p.X, p.Y] = ' ';
                        explosion = false;
                    }
                    ExplosionRadius.Clear();

                }
            }
        }

        public bool WinCondition()
        {
            return Diamonds.Count == 0;
        }

    }



} 

