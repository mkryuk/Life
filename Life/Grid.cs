using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Grid
    {
        public Size FieldSize;
        private Cell[][] _cells;
        public event EventHandler Draw;
        public event EventHandler CalcNeighbors;
        public event EventHandler NextStep;
        public event EventHandler FillNeighbors;

        protected virtual void OnFillNeighbors()
        {
            EventHandler handler = FillNeighbors;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public int Alive { get; set; }

        protected virtual void OnMoveNext()
        {
            //Calc neighbors for next step
            OnCalcNeighbors();

            //Draw all cells
            OnDraw();           

            EventHandler handler = NextStep;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnCalcNeighbors()
        {
            EventHandler handler = CalcNeighbors;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnDraw()
        {
            EventHandler handler = Draw;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public Grid(Size size)
        {
            FieldSize = size;
            _cells = new Cell[FieldSize.Height][];
            for (var y = 0; y < FieldSize.Height; y++)
            {
                _cells[y] = new Cell[FieldSize.Width];
            }
            Console.SetWindowSize(FieldSize.Width + 1, FieldSize.Height + 2);
            Console.SetBufferSize(FieldSize.Width + 1, FieldSize.Height + 2);
            FillGrid();
        }

        public IEnumerable<Cell> GetNeighbors(Point pt)
        {
            //return top neighbors
            if (pt.X - 1 >= 0 && pt.Y - 1 >= 0)
            {
                yield return _cells[pt.Y - 1][pt.X - 1];
            }
            if (pt.Y - 1 >= 0)
            {
                yield return _cells[pt.Y - 1][pt.X];
            }
            if (pt.X + 1  < FieldSize.Width && pt.Y - 1 >= 0)
            {
                yield return _cells[pt.Y - 1][pt.X + 1];
            }
            

            //return left & right neighbors
            if (pt.X - 1 >= 0)
            {
                yield return _cells[pt.Y][pt.X - 1];
            }
            if (pt.X + 1 < FieldSize.Width)
            {
                yield return _cells[pt.Y][pt.X + 1];
            }
            
            //return bottom neighbors
            if (pt.X - 1 >= 0 && pt.Y + 1 < FieldSize.Height)
            {
                yield return _cells[pt.Y + 1][pt.X - 1];
            }
            if (pt.Y + 1 < FieldSize.Height)
            {
                yield return _cells[pt.Y + 1][pt.X];
            }
            if (pt.X + 1 < FieldSize.Width && pt.Y + 1 < FieldSize.Height)
            {
                yield return _cells[pt.Y + 1][pt.X + 1];
            }
            
        }

        private void FillGrid()
        {
            var giveLife = new Random();
            //Create grid with cells
            for (var y = 0; y < FieldSize.Height; y++)
            {
                for (var x = 0; x < FieldSize.Width; x++)
                {
                    _cells[y][x] = new Cell(this, new Point(x, y), Convert.ToBoolean(giveLife.Next(0,2)));
                    Draw += _cells[y][x].Draw;
                    CalcNeighbors += _cells[y][x].CalcNeighbors;
                    NextStep += _cells[y][x].NextStep;
                    FillNeighbors += _cells[y][x].FillNeighbors;
                }
            }

            //Filling cells neighbors
            OnFillNeighbors();

            //_cells[0][1].IsAlive = true;
            //_cells[1][2].IsAlive = true;
            //_cells[2][0].IsAlive = true;
            //_cells[2][1].IsAlive = true;
            //_cells[2][2].IsAlive = true;
        }

        public void MoveNext()
        {            
            OnMoveNext();
        }
    }
}
