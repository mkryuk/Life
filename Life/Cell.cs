using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Life
{
    public static class CellColor
    {
        public static List<ConsoleColor> Color = new List<ConsoleColor>
        {
            ConsoleColor.Blue,
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Magenta,
            ConsoleColor.Green
        };
    }
    class Cell
    {
        public Point Position { get; set; }
        private Grid _mainGrid;
        private bool _isAlive;
        private bool _shouldRedraw;
        private List<Cell> _neighbors;
        public int AliveNeighborsCount { get; set; }

        //cell color
        private ConsoleColor _color;

        private static Random _fatum = new Random();
        public bool IsAlive {
            get { return _isAlive; }
            set {
                if (value == true)
                {
                    _mainGrid.Alive++;
                }
                _isAlive = value; 
            }
        }

        public Cell(Grid mainGrid, Point position = default(Point), bool isAlive = false)
        {
            _mainGrid = mainGrid;
            _isAlive = isAlive;
            _shouldRedraw = true;
            _color = CellColor.Color[_fatum.Next(0,CellColor.Color.Count)];
            if (isAlive)
            {
                _mainGrid.Alive++;
            }
            Position = position;
        }

        public void CalcNeighbors(object sender, EventArgs e)
        {
            var alived = _neighbors.FindAll(cell => cell.IsAlive);
            AliveNeighborsCount = alived.Count;
            var bigger = alived.Find((cell)=>cell.AliveNeighborsCount >= AliveNeighborsCount);
            if (bigger != null) _color = bigger._color;
            //AliveNeighborsCount = _mainGrid.GetNeighbors(Position).ToList().FindAll(cell => cell.IsAlive).Count;
        }

        public void NextStep(object sender, EventArgs e)
        {
            //cell was dead and should be born
            if (!IsAlive && AliveNeighborsCount == 3)
            {
                _isAlive = true;
                _mainGrid.Alive++;
                _shouldRedraw = true;
                return;
            }

            //if cell is dead - return
            if (!IsAlive) return;

            //if cell is alive and should die
            if (AliveNeighborsCount < 2 || AliveNeighborsCount > 3)
            {
                _isAlive = false;
                _mainGrid.Alive--;
                _shouldRedraw = true;
            }
        }

        public void Draw(object sender, EventArgs e)
        {
            //if cell should be drawn
            if (_shouldRedraw)
            {
                if (IsAlive)
                {
                    Console.ForegroundColor = _color;
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("*");
                }
                else
                {
                    //Console.ResetColor();
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write(" ");
                }
            }
           
            _shouldRedraw = false;
        }

        public void FillNeighbors(object sender, EventArgs e)
        {
            _neighbors = _mainGrid.GetNeighbors(Position).ToList();
        }
    }
}
