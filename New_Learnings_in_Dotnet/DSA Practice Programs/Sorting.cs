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
            //// 1). Bubble Sort
            //int[] arr1 = { 5, 4, 1, 3, 2 };
            //int swaps = 0;
            ////arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            //for (int i = 0; i < arr1.Length - 1; i++)
            //{
            //    if (i > 0 && swaps == 0)
            //        break;
            //    else
            //    {
            //        for (int j = 0; j <= arr1.Length - 2 - i; j++)
            //        {
            //            if (arr1[j] > arr1[j + 1])
            //            {
            //                int temp = arr1[j];
            //                arr1[j] = arr1[j + 1];
            //                arr1[j + 1] = temp;
            //                swaps++;
            //            }
            //        }
            //    }
            //}
            //if (swaps > 0)
            //    Console.WriteLine("Sorted Array : " + string.Join(", ", arr1));
            //else
            //    Console.WriteLine("Array is already Sorted");
            //----------------------------------------------------
            //// 2). Selection Sort
            int[] arr1 = { 5, 4, 1, 3, 2 };
            int swaps = 0;
            //arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            for (int i = 0; i < arr1.Length - 1; i++)
            {
                int minpos = i;
                for (int j = i + 1; j < arr1.Length; j++)
                {
                    if (arr1[minpos] > arr1[j])
                    {
                        minpos = j;
                    }
                }
                //if (i > 0 && swaps == 0)
                //{
                //    Console.WriteLine("Array is already Sorted");
                //    break;
                //}
                //else
                //{
                //swap
                int temp = arr1[minpos];
                arr1[minpos] = arr1[i];
                arr1[i] = temp;
                //}                
            }
            Console.WriteLine("Sorted Array : " + string.Join(", ", arr1));






        }


    }
}
