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
            int[,] matrix = new int[3, 3];
            //Input
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
            //output
            Console.WriteLine("Output :- Matrix");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

            //------------------------------------------------------
            int[][] myArray = new int[3][] { new int[3] { 1, 2, 3 }, new int[3] { 4, 5, 6 }, new int[3] { 7, 8, 9 } };
            Console.WriteLine("Output :- myArray");
            Console.WriteLine(string.Join("\n", myArray.Select(x => string.Join(" ", x))));


        }
    }
}
