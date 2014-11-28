using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
        
        //public int AliveNeighborsCount { get; set; }
        private Dictionary<ConsoleColor, int> ArroundColorsCount;

        //cell color
        public ConsoleColor _color;

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
            int index = _fatum.Next(0, CellColor.Color.Count);
            _color = CellColor.Color[index];
            mainGrid.result[index]++;

            if (isAlive)
            {
                _mainGrid.Alive++;
            }
            Position = position;
        }

        public void CalcNeighbors(object sender, EventArgs e)
        {
            var alived = _neighbors.FindAll(cell => cell.IsAlive);

            var arroundColorsCount = new Dictionary<ConsoleColor, int>();
            foreach (var cell in alived)
            {
                var value = 0;
                if (!arroundColorsCount.TryGetValue(cell._color, out value))
                {
                    arroundColorsCount.Add(cell._color, 0);
                }
                arroundColorsCount[cell._color]++;
            }
            ArroundColorsCount = arroundColorsCount;
            //AliveNeighborsCount = alived.FindAll((cell)=>cell._color == _color).Count;

            //var bigger = alived.Find((cell)=>cell.AliveNeighborsCount >= AliveNeighborsCount);
            //if (bigger != null) _color = bigger._color;

            //ToList().FindAll((keyVal) => keyVal.Value == 3);
            //AliveNeighborsCount = _mainGrid.GetNeighbors(Position).ToList().FindAll(cell => cell.IsAlive).Count;
        }

        public ConsoleColor GetWinnerColor()
        {
            var list = new List<ConsoleColor>();
            ArroundColorsCount.All((item) =>
            {
                if (item.Value == 3) list.Add(item.Key);
                return false;
            });
            var result = ConsoleColor.Black;
            switch (list.Count)
            {
                case 0:
                    result = ConsoleColor.Black;
                    break;
                case 1:
                    result = list[0];
                    break;
                case 2:
                    result = list[_fatum.Next(0, 2)];
                    break;
            }
            return result;
        }

        public void NextStep(object sender, EventArgs e)
        {
            //if current neibors count is 3 and they are the same color we are alive
            int sameColorCount = 0;
            ArroundColorsCount.TryGetValue(_color, out sameColorCount);
            if (sameColorCount == 3)
            {
                return;
            }

            ConsoleColor winnerColor = GetWinnerColor();

            //if cell is dead and there are 3 cells of some color let it born
            if (!IsAlive && winnerColor != ConsoleColor.Black)
            {
                _isAlive = true;
                //_mainGrid.Alive++;
                _shouldRedraw = true;
                _color = winnerColor;
                return;
            }

            //if cell is dead - return
            if (!IsAlive) return;

            // if there are 2 cells with same color 
            if (sameColorCount == 2)
            {
                return;
            }

            //if cell is alive and should die
            _isAlive = false;
            _color = ConsoleColor.Black;
            _shouldRedraw = true;
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
