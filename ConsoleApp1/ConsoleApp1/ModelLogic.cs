using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace SkafRevengeModel
{
    class ModelWithLogic : Model
    {
        protected Queue<WaveElement> SkafQ;
        protected Queue<WaveElement> PlayerQ;
        const int BottlesHP = 10;

        public ModelWithLogic(Cell[,] map, Point player, Point skaf, int bottles) : base(bottles, map, player, skaf)
        {
            SkafQ = new Queue<WaveElement>();
            SkafQ.Enqueue(new WaveElement(10000, skaf));
            PlayerQ = new Queue<WaveElement>();
        }

        public string Start()
        {
            int ShafC = 0, PlayerC = 0;
            while(true)
            {
                int dP = ProcessQueue(ref PlayerQ, CellDomain.Player);
                int dS = ProcessQueue(ref SkafQ, CellDomain.Skaf);
                if (dS == 0 && dP == 0)
                {
                    if (ShafC > PlayerC) return "Дай взаймы";
                    else if (ShafC == PlayerC) return "Го по шаве";
                    else return "Отдай долг";
                }
                ShafC += dS; PlayerC += dP;
                Thread.Sleep(500);
            }
        }

        private bool InBounds(Point p)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X < Map.GetLength(0) && p.Y < Map.GetLength(1)) return true;
            return false;
        }

        private bool IsCellAvabForStep(CellDomain dom, Point p)
        {
            if ((Map[p.X, p.Y]).CellState == CellDomain.Free) return true;
            if ((Map[p.X, p.Y]).CellState == dom) return true;
            return false;
        }

        private void MovePlayer(Point delta)
        {
            var tmpPos = new Point(Player.Position.X + delta.X, Player.Position.Y + delta.Y);
            if (InBounds(tmpPos) && IsCellAvabForStep(CellDomain.Player, tmpPos)) Player.Position = tmpPos;
        }

        public void SpawnBottle()
        {
            if (Player.BottlesCount != 0 && (!Bottles.Contains(Player.Position)))
            {
                Bottles.Add(Player.Position);
                PlayerQ.Enqueue(new WaveElement(BottlesHP, Player.Position));
                Player.BottlesCount--;
            }
        }

        private int ProcessQueue(ref Queue<WaveElement>  wave, CellDomain waveDomain)
        {
            int count = 0;
            var tmpQ = new Queue<WaveElement>();
            while (wave.Count > 0)
            {
                var startElem = wave.Dequeue();
                if ((startElem.HPLeft <= 0) || (Map[startElem.Position.X, startElem.Position.Y].CellState != CellDomain.Free)) continue;
                count++;
                Map[startElem.Position.X, startElem.Position.Y].CellState = waveDomain;
                var delta = new int[] { -1, 1 };
                foreach (var tmp in
                    delta
                    .SelectMany(d =>
                    new Point[] { new Point(startElem.Position.X + d, startElem.Position.Y), new Point(startElem.Position.X, startElem.Position.Y + d) })
                    .Where(x => (InBounds(x) && IsCellAvabForStep(CellDomain.Player,x))))
                {
                    tmpQ.Enqueue(new WaveElement(startElem.HPLeft - 1, tmp));
                }
            }
            wave = tmpQ;
            return count;
        }

        public void MoveUp()
        {
            MovePlayer(new Point(-1, 0));
        }

        public void DownUp()
        {
            MovePlayer(new Point(1, 0));
        }

        public void MoveLeft()
        {
            MovePlayer(new Point(0, -1));
        }

        public void MoveRight()
        {
            MovePlayer(new Point(0, 1));
        }

    }
}
