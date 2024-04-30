using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public class _2D_Arrays
    {
        public static void Main()
        {
            //------------------------------------------------------
            int[,] matrix2D = new int[3, 4];
            //Input
            for (int i = 0; i < matrix2D.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2D.GetLength(1); j++)
                {
                    matrix2D[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            //output
            Console.WriteLine("Output :- 2D Matrix");
            for (int i = 0; i < matrix2D.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2D.GetLength(1); j++)
                {
                    Console.Write(matrix2D[i, j] + " ");
                }
                Console.WriteLine();
            }

            //------------------------------------------------------
            int[][] JaggedArray = new int[3][] { new int[5] { 1, 2, 3, 11, 12 }, new int[3] { 4, 5, 6 }, new int[4] { 7, 8, 9, 10 } };
            Console.WriteLine("Output :- myArray");
            Console.WriteLine(string.Join("\n", JaggedArray.Select(x => string.Join(" ", x))));

  

        }
    }
}
