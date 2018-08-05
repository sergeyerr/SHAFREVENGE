using System;
using System.Collections.Generic;
namespace SkafRevengeModel
{
    enum CellDomain
    {
        Free, Player, Skaf, Wall
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Model
    {
        public readonly Cell[,] Map;
        protected PlayerClass Player { get; private set; }
        protected Point SkafInitPos;
        protected HashSet<Point> Bottles;


        public Model(int bottles, Cell[,] drawMap, Point PlayerPos, Point skafInitPos)
        {
            Map = drawMap;
           
            Bottles = new HashSet<Point>();
            SkafInitPos = skafInitPos;
            Player = new PlayerClass(bottles, PlayerPos);
        }

        protected class WaveElement
        {
            public int HPLeft{get; set;}
            public Point Position { get; set; }
            public WaveElement(int hp, Point p)
            {
                HPLeft = hp;
                Position = p;
            }
        }

        protected class PlayerClass
        {
            public List<Bonus> Bonuses;
            public int BottlesCount { get; set; }

            public Point Position { get; set; }
            public PlayerClass(int bottles, Point pos)
            {
                Position = pos;
                BottlesCount = bottles;
                Bonuses = new List<Bonus>();
            }
        }

        #region Bonuses
        public abstract class Bonus
        {
            public int Live { get; protected set; }
            public bool IsInit { get; protected set; }
            //public abstract void Initialize(PlayerClass player);
            public abstract void Use(int points);
        }
        #endregion
        #region Cells
        public class Cell
        {
            //public Action<Player> Bonus;
            public CellDomain CellState { get; set; }
            public Cell(CellDomain dom)
            {
                CellState = dom;
            }
        }
        
        #endregion
    }
}
