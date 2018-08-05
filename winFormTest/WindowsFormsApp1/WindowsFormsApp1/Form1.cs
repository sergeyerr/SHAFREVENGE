using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WindowsFormsApp1.Properties;
using System.Threading;
using System.Media;
using SkafRevengeModel;

namespace WindowsFormsApp1
{
    public partial class GameForm : Form
    {
        Brush playerBrush, wallBrush, freeBrush, playerAreaBrush;
        Image shkafImage;
        Point player, shkaf;
        Model.Cell[,] map;
        ModelWithLogic game;
        Stopwatch gameUpdTimer;
        const int cellSize = 10;
        const int offset = 30;
        const int gameSpeed = 3000;

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    game.MoveUp();
                    break;
                case Keys.Down:
                    game.MoveDown();
                    break;
                case Keys.Right:
                    game.MoveRight();
                    break;
                case Keys.Left:
                    game.MoveLeft();
                    break;
                case Keys.Space:
                    game.SpawnBottle();
                    break;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
              string message = game.Upd();
              //if (message != "game")

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SoundStart();
        }

        private void SoundStart()
        {
            System.IO.Stream str = Resources.baraban;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GameCanvas.Update();
            GameCanvas.Invalidate();

        }

        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Rectangle field = new Rectangle(offset, offset, game.Map.GetLength(1) * cellSize, game.Map.GetLength(0) * cellSize);
            Rectangle face = new Rectangle(offset + game.Player.Position.Y * cellSize, offset + game.Player.Position.X * cellSize, cellSize, cellSize);
            var graphic = e.Graphics;
            graphic.DrawImage(shkafImage, field);
            for (int i = 0; i < game.Map.GetLength(0); i++)
            {
                for (int j = 0; j < game.Map.GetLength(1); j++)
                {
                    switch (game.Map[i, j].CellState)
                    {
                        case CellDomain.Free:
                            DrawCell(graphic, i, j, freeBrush);
                            break;
                        case CellDomain.Player:
                            DrawCell(graphic, i, j, playerAreaBrush);
                            break;
                        case CellDomain.Wall:
                            DrawCell(graphic, i, j, wallBrush);
                            break;
                    }
                    if (game.Bottles.Contains(new Point(i, j)))
                    {
                        Rectangle bottle = new Rectangle(offset + j * cellSize, offset + i * cellSize, cellSize, cellSize);
                        graphic.DrawImage(Resources.baltika, bottle);
                    }
                }
            }
            graphic.DrawImage(Resources.face, face);
        }

        private void DrawCell(Graphics graphics, int i, int j, Brush brush)
        {
            graphics.FillRectangle(brush, new Rectangle(offset + j * cellSize, offset + i * cellSize, cellSize, cellSize));
        }

        public GameForm()
        {
            SoundStart();
            InitializeComponent();
            playerBrush = new SolidBrush(Color.Violet);
            freeBrush = new SolidBrush(Color.White);
            wallBrush = new TextureBrush(Resources.stone);
            playerAreaBrush = new TextureBrush(Resources.peevo);
            shkafImage = Resources.skaf;
            Parser.ParseChar("Map_design.txt", out player, out shkaf, out map);
            game = new ModelWithLogic(map, player, shkaf, 5);
            //string res = game.Start();
            timer1.Interval = 1000 / 60;
            gameUpdTimer = new Stopwatch();
            timer2.Interval = 1000;

        }

    }
}
