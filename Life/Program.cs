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
            var life = new Grid(new Size(Console.LargestWindowWidth-2, Console.LargestWindowHeight-2));
            
            
            //var rand = new Random();
            //int[] result = new int[4];
            //for (int i = 0; i < 1000; i++)
            //{
            //    result[rand.Next(0, 4)]++;
            //}

            //foreach (var i in result)
            //{
            //    Console.WriteLine("{0}", i);
            //}  
          
            while (life.Alive > 0)
            {
                life.MoveNext();
            } 
        }
    }
}
