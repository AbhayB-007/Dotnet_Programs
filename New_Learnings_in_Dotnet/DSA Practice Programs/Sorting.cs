using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public class Sorting
    {
        public static void Main(String[] argd)
        {
            //----------------------------------------------------
            // 1). Bubble Sort
            int[] arr1 = { 5, 4, 1, 3, 2 };
            for (int i = 0; i < arr1.Length - 1; i++)
            {
                for (int j = 0; j <= arr1.Length - 2 - i; j++)
                {
                    if (arr1[j] > arr1[j + 1])
                    {
                        int temp = arr1[j];
                        arr1[j] = arr1[j + 1];
                        arr1[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine(string.Join(", ", arr1));
        }


    }
}
