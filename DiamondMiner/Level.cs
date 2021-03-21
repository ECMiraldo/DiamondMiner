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
        
        private Game1 game1;
        public char[,] matrix;
        Texture2D diamond, rocks, dirt, dynamite, wall;
        private Player player;

        public List<Point> Rocks;
        public List<Point> Diamonds;
        public List<Point> Dynamite;

       
        //Basicamente, tudo o que este construtor faz é prencher a matriz do level, e adicionar os Diamantes e Dynamites nas respectivas listas.
        public Level(Game1 game1, string levelFile) //Goes to game1 Initialize
        {
            this.game1 = game1;
            Diamonds = new List<Point>();
            string[] linhas = File.ReadAllLines($"Content/{levelFile}");  // "Content/" + level
            int nrLinhas = linhas.Length;
            int nrColunas = linhas[0].Length;

            matrix = new char[nrColunas, nrLinhas];
            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    if (linhas[y][x] == '$')
                    {
                        Diamonds.Add(new Point(x, y));
                        matrix[x, y] = ' '; // put a blank instead of the diamond ' '
                    }
                    else if (linhas[y][x] == 'M')
                    {
                        new Player(game1, x, y);
                    }
                    else if (linhas[y][x] == 'D')
                    {
                        Dynamite.Add(new Point(x, y));
                        matrix[x, y] = ' ';
                    }
                    else if (linhas[y][x] == '*')
                    {
                        Rocks.Add(new Point(x, y));
                        matrix[x, y] = ' ';
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
            diamond = game1.Content.Load<Texture2D>("diamante");
            //rocks = Content.Load<Texture2D>("pedra");
            dirt = game1.Content.Load<Texture2D>("terra");
            wall = game1.Content.Load<Texture2D>("muro");
            //dynamite = Content.Load<Texture2D>("");
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

            //Draw the rocks
            foreach(Point r in Rocks)
            {
                position.X = r.X * game1.tileSize;
                position.Y = r.Y * game1.tileSize;
                _spriteBatch.Draw(rocks, position, Color.White);
            }
        }


        public bool HasRock(int x, int y)
        {
            return Rocks.Contains(new Point(x, y));
        }

        public bool HasDynamite(int x, int y)
        {
            return Dynamite.Contains(new Point(x, y));
        }

        public bool HasDiamond(int x, int y)
        {
            return Diamonds.Contains(new Point(x, y));
        }







        //Atualizar quando diamantes sao recolhidos, ou dinamites, quando o player mina uma celula, etc...
        public void UpdateLevel() //Goes to game1 update
        {

        }

    }
}
