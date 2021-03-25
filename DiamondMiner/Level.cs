using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DiamondMiner
{
    public class Level
    {
        Texture2D diamond, rocks, dirt, dynamite, wall;

        private Game1  game1;
        private Player player;
        //Criei estas variáveis para conseguir escrever no UI
        public int tileSize = 32;
        private SpriteFont arial12;

        public char[,]     matrix;
        public List<Point> Rocks;
        public List<Point> Diamonds;
        public List<Point> Dynamite;

       
        //Basicamente, tudo o que este construtor faz é prencher a matriz do level, e adicionar os Diamantes e Dynamites nas respectivas listas.
        public Level(Game1 game1, string levelFile) //Goes to game1 Initialize
        {
            this.game1 = game1;

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
                        new Player(game1, x, y);
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
            diamond  = game1.Content.Load<Texture2D>("gift_resized");
            rocks    = game1.Content.Load<Texture2D>("Stone");
            dirt     = game1.Content.Load<Texture2D>("ground_snow");
            wall     = game1.Content.Load<Texture2D>("IceBox");
            dynamite = game1.Content.Load<Texture2D>("candytnt");
            arial12 = game1.Content.Load<SpriteFont>("arial12");
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
            foreach (Point r in Rocks)
            {
                position.X = r.X * game1.tileSize;
                position.Y = r.Y * game1.tileSize;
                _spriteBatch.Draw(rocks, position, Color.White);
            }

            //Draw UI
            string diamondsUI =$"{ Diamonds.Count}";
            Point measure = arial12.MeasureString(diamondsUI).ToPoint();
            int posX = matrix.GetLength(0) * tileSize - measure.X - 5;
            _spriteBatch.DrawString(
                arial12,
                diamondsUI,
                new Vector2(posX, matrix.GetLength(1) * tileSize + 10),
                Color.Coral);
        }

        public bool HasRock(Point p)     => Rocks.Contains(p);
        public bool HasWall(Point p)     => matrix[p.X, p.Y] == '#';
        public bool HasDynamite(Point p) => Dynamite.Contains(p);
        public bool HasDiamond(Point p)  => Diamonds.Contains(p);
        public bool EmptyTile(Point p) {
            if (InMatrix(p)) return matrix[p.X, p.Y] == ' ';
            else return false;
        }
        public bool DirtTile(Point p)    => (InMatrix(p) && (matrix[p.X, p.Y] == '.'));
        public bool InMatrix(Point p)    => ((p.X >= 0 && p.Y >= 0) && (p.X < matrix.GetLength(0) && p.Y < matrix.GetLength(1)));

        public void RockGravity(GameTime gametime)
        {
            for (int i = 0; i<Rocks.Count; i++)
            {
                if (InMatrix(Rocks[i]) && EmptyTile(Rocks[i]))
                {
                    Rocks[i] = new Point(Rocks[i].X, Rocks[i].Y + 1);
                }
            }
            
        }
        


    }
}
