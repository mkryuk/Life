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
    class Cell
    {
        public Point Position { get; set; }
        private Grid _mainGrid;
        private bool _isAlive;
        private bool _shouldRedraw;
        public int NeighborCount { get; set; }
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
            if (isAlive)
            {
                _mainGrid.Alive++;
            }
            Position = position;
        }

        public void CalcNeighbors(object sender, EventArgs e)
        {
            NeighborCount = _mainGrid.GetNeighbors(Position).ToList().FindAll(cell => cell.IsAlive).Count;
        }

        public void NextStep(object sender, EventArgs e)
        {
            //birthday
            if (!IsAlive && NeighborCount == 3)
            {
                _isAlive = true;
                _mainGrid.Alive++;
                _shouldRedraw = true;
                return;
            }
            if (IsAlive)
            {
                if (NeighborCount < 2 || NeighborCount > 3)
                {
                    _isAlive = false;
                    _mainGrid.Alive--;
                    _shouldRedraw = true;
                }
                else
                {
                    _isAlive = true;
                    _shouldRedraw = false;
                }
            }    
        }

        public void Draw(object sender, EventArgs e)
        {
            if (_shouldRedraw)
            {
                if (IsAlive)
                {
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("*");
                }
                else 
                {
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write(" ");
                }
            }
           
            _shouldRedraw = false;
        }
    }
}
