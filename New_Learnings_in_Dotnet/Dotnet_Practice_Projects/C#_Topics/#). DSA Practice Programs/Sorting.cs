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
            //----------------------------------------------------------
            //// 1). Bubble Sort --> Time Complexity -> Best -> O(n)
            //                                       -> Avg -> O(n^2)
            //                                       -> Worst -> O(n^2)
            //                   --> Space Complexity -> Worst -> O(1)
            //----------------------------------------------------------

            //int[] arr1 = { 5, 4, 1, 3, 2 };
            //int swaps = 0;
            //arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            //for (int i = 0; i < arr1.Length - 1; i++)
            //{
            //    if (i > 0 && swaps == 0)
            //        break;
            //    else
            //    {
            //        for (int j = 0; j < arr1.Length - 1 - i; j++)
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

            //------------------------------------------------------------
            //// 2). Selection Sort --> Time Complexity -> Best -> O(n^2)
            //                                       -> Avg -> O(n^2)
            //                                       -> Worst -> O(n^2)
            //                      --> Space Complexity -> Worst -> O(1)
            //------------------------------------------------------------

            //int[] arr1 = { 5, 4, 1, 3, 2 };
            //int swaps = 0;
            ////arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            //for (int i = 0; i < arr1.Length - 1; i++)
            //{
            //    int minpos = i;
            //    for (int j = i + 1; j < arr1.Length; j++)
            //    {
            //        if (arr1[minpos] > arr1[j])
            //            minpos = j;
            //    }
            //    if (minpos != i)
            //    {
            //        //swap
            //        int temp = arr1[minpos];
            //        arr1[minpos] = arr1[i];
            //        arr1[i] = temp;
            //        swaps++;
            //    }
            //}
            //Console.WriteLine($"Swaps : {swaps} | Sorted Array : " + string.Join(", ", arr1));

            //----------------------------------------------------------
            //// 3). Insert Sort --> Time Complexity -> Best -> O(n)
            //                                       -> Avg -> O(n^2)
            //                                       -> Worst -> O(n^2)
            //                   --> Space Complexity -> Worst -> O(1)
            //----------------------------------------------------------

            //int[] arr1 = { 5, 4, 1, 3, 2 };
            ////arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            //for (int i = 1; i < arr1.Length; i++)
            //{
            //    int curr = arr1[i];
            //    int prev = i - 1;

            //    while (prev >= 0 && arr1[prev] > curr)
            //    {
            //        arr1[prev + 1] = arr1[prev];
            //        prev--;
            //    }

            //    //insertion
            //    arr1[prev + 1] = curr;
            //}
            //Console.WriteLine($"Sorted Array : " + string.Join(", ", arr1));

            //----------------------------------------------------------
            //// 4). Count Sort --> Time Complexity -> Best -> O(n + k)
            //                                      -> Avg -> O(n + k)
            //                                      -> Worst -> O(n + k)
            //                  --> Space Complexity -> Worst -> O(k)
            //----------------------------------------------------------

            //int[] arr1 = { 4, 3, 12, 1, 5, 5, 3, 9, 0 };
            ////arr1 = new int[] { 1, 2, 3, 4, 5 }; //sorted array
            //int largest = int.MinValue;
            //for(int i = 0; i < arr1.Length; i++)
            //{
            //    largest = Math.Max(largest, arr1[i]);
            //}

            //int[] frequency = new int[largest + 1];
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    frequency[arr1[i]]++;
            //}

            ////sorting
            //int j = 0;
            //for (int i = 0;i < frequency.Length; i++)
            //{
            //    while (frequency[i] > 0)
            //    {
            //        arr1[j] = i;
            //        j++;
            //        frequency[i]--;
            //    }
            //}
            //Console.WriteLine($"Sorted Array : " + string.Join(", ", arr1));





        }


    }
}
