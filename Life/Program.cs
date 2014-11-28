using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid Life = new Grid(new Size(Console.LargestWindowWidth-2, Console.LargestWindowHeight-2));
            while (Life.Alive > 0)
            {
                Life.MoveNext();
            }                                              
        }
    }
}
