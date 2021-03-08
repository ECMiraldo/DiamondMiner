using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DiamondMiner
{
    public class Level : Game
    {
        char[,] matrix;
        public string levelName;
        Texture2D diamond, rocks, dirt, dynamite, wall;

        public List<Point> Diamonds;

        public void LoadLevelContent()
        {
            diamond = Content.Load<Texture2D>("diamante");
            rocks = Content.Load<Texture2D>("pedra");
            dirt = Content.Load<Texture2D>("terra");
            wall = Content.Load<Texture2D>("muro");
            //dynamite = Content.Load<Texture2D>("");
        }



        public void LoadLevel(string levelFile) //Goes to game1 load
        {
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
                        matrix[x, y] = ' '; // put a blank instead of the box '#'
                    }
                    else if (linhas[y][x] == '*')
                    {
                        sokoban = new Player(this, x, y);
                        level[x, y] = ' '; // put a blank instead of the sokoban 'Y'
                    }
                    else
                    {
                        matrix[x, y] = linhas[y][x];
                    }
                }
            }
        }

        public void DrawLevel() //Goes to game1 draw
        {

        }

        public void UpdateLevel() //Goes to game1 update
        {

        }

    }
}
