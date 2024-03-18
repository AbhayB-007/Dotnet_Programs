using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects.TypeComparers;


namespace Practice // Note: actual namespace depends on the project name.
{
    public class Arrays
    {
        public static void Main(string[] args)
        {
            // Console.WriteLine("Sha1 Encryption : " + Sha1Encrypt("Abhay_ServiceUser", "@bhaY007"));
            // BcryptVerify(Sha1Encrypt("Abhay_ServiceUser", "@bhaY007"));

            // ---------------------------------------------------------------------------------------

            //int first = arr.Where(x => x == 6).FirstOrDefault();
            //int single = arr.Where(x => x == 1).SingleOrDefault();
            //Console.WriteLine($"first : {first}\nsingle : {single}");

            // ---------------------------------------------------------------------------------------
            //// 1). Binary Search
            //var AscArr = Enumerable.Range(1, 10).ToArray();
            //var DescArr = Enumerable.Range(1, 10).OrderDescending().ToArray();
            //int resultAsc = BinarySearch(AscArr, 10, true);
            //int resultDesc = BinarySearch(DescArr, 10, false);
            //Console.WriteLine("AscArr : " + String.Join(',', AscArr));
            //if (resultAsc == -1)
            //    Console.WriteLine("Key not found!...");
            //else
            //    Console.WriteLine("AscArr Index for key is  : " + resultAsc);

            //Console.WriteLine("DescArr : " + String.Join(',', DescArr));
            //if (resultDesc == -1)
            //    Console.WriteLine("Key not found!...");
            //else
            //    Console.WriteLine("DescArr Index for key is  : " + resultDesc);

            // ---------------------------------------------------------------------------------------
            //// 2). Print pairs in array
            //var arr1 = Enumerable.Range(1, 5).ToArray();
            //int count = 0, t = 0;

            //Console.WriteLine($"Solve using while loop ");
            //while (t < arr1.Length)
            //{
            //    if (t == arr1.Length - 1 && count <= arr1.Length)
            //    {
            //        t = ++count;
            //        Console.WriteLine();
            //        continue;
            //    }
            //    Console.Write($"({arr1[count]}, {arr1[t + 1]}) | ");
            //    t++;
            //}

            //Console.WriteLine($"Solve using for loop");
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    for (int j = i + 1; j < arr1.Length; j++)
            //    {
            //        Console.Write($"({arr1[i]}, {arr1[j]}) | ");
            //    }
            //    Console.WriteLine();
            //}
            // ---------------------------------------------------------------------------------------
            //// 3). Print sub-arrays in array
            //var arr1 = Enumerable.Range(1, 5).ToArray(); //{1,2,3,4,5}
            //int ts = 0;
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    for (int j = i; j < arr1.Length; j++)
            //    {
            //        int sum = 0;
            //        for (int k = i; k <= j; k++)
            //        {
            //            Console.Write(arr1[k] + " ");
            //            sum += arr1[k];
            //        }
            //        Console.WriteLine($" --> (Sum : {sum})");
            //        ts++;
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine($"Total Subarrays : {ts}");
            // ---------------------------------------------------------------------------------------
            //// 4). Print max sub-arrays sum
            //int[] arr1 = { 1, -2, 6, -1, 3 };
            //int maxSum = 0;
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    for (int j = i; j < arr1.Length; j++)
            //    {
            //        int sum = 0;
            //        for (int k = i; k <= j; k++)
            //        {
            //            sum += arr1[k];
            //            if (maxSum < sum) { maxSum = sum; }
            //        }
            //        Console.Write(sum + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("maxSum : " + maxSum);
            // ---------------------------------------------------------------------------------------
            // 5). Print max sub-arrays sum using prefix array
            //int[] arr1 = { -2, -3, 4, -1, -2, 1, 5, -3 };
            //int[] prefix = new int[arr1.Length]; //prefix array
            //prefix[0] = arr1[0];
            //for (int k = 1; k < arr1.Length; k++)
            //{
            //    prefix[k] = prefix[k - 1] + arr1[k];
            //}
            //int maxSum = 0;
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    for (int j = i; j < arr1.Length; j++)
            //    {
            //        int sum = i == 0 ? prefix[j] : prefix[j] - prefix[i - 1];
            //        if (maxSum < sum) { maxSum = sum; }
            //        Console.Write(sum + " ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine("maxSum : " + maxSum);
            // ---------------------------------------------------------------------------------------
            //// 6). Kadane's Algorithm
            //int[] arr1 = { -2, -3, -4, -1, -2, -1, -5, -3 };
            //int cs = 0, ms = int.MinValue;
            //bool negArr = true;
            ////In case of all -ve numbers          
            //for (int i = 0; i < arr1.Length; i++)
            //{
            //    if (0 < arr1[i])
            //    {
            //        negArr = false;
            //        break;
            //    }                                   
            //}

            //if(negArr)
            //{
            //    int maxNum = arr1[0];
            //    for (int i = 1; i < arr1.Length; i++)
            //    {
            //        if (maxNum < arr1[i])
            //        {
            //            maxNum = arr1[i];                        
            //        }
            //    }
            //    Console.WriteLine("maxSum in -ve array : " + maxNum);
            //}
            //else
            //{
            //    for (int i = 0; i < arr1.Length; i++)
            //    {
            //        cs = cs + arr1[i];
            //        if (ms < cs)
            //            ms = cs;
            //        if (cs < 0) //if the current sum is less than 0 then init cs as 0.
            //            cs = 0;
            //    }
            //    Console.WriteLine("maxSum : " + ms);
            //}
            // ---------------------------------------------------------------------------------------
            //// 6). Trapping Rainwater
            //int[] height = { 4, 2, 0, 3, 2, 5 };
            ////calc left max boundary
            //int[] leftMax = new int[height.Length];
            //leftMax[0] = height[0];
            //for (int i = 1; i < height.Length; i++)
            //{
            //    leftMax[i] = Math.Max(height[i], leftMax[i - 1]);
            //}
            ////calc right max boundary
            //int[] rightMax = new int[height.Length];
            //rightMax[height.Length - 1] = height[height.Length - 1];
            //for (int i = height.Length - 2; i >= 0; i--)
            //{
            //    rightMax[i] = Math.Max(height[i], rightMax[i + 1]);
            //}
            //int trappedWater = 0;

            //for (int i = 1; i < height.Length; i++)
            //{
            //    int waterLevel = Math.Min(leftMax[i], rightMax[i]);
            //    trappedWater += waterLevel - height[i];
            //}
            //Console.WriteLine("Trapped Water : " + trappedWater);
            // OR
            //int n = height.Length;
            //int res = 0, l = 0, r = n - 1;
            //int rMax = height[r], lMax = height[l];
            //while (l < r) 
            //{ 
            //    if (lMax < rMax) 
            //    { 
            //        l++; 
            //        lMax = Math.Max(lMax, height[l]); 
            //        res += lMax - height[l]; 
            //    } else { 
            //        r--; 
            //        rMax = Math.Max(rMax, height[r]); 
            //        res += rMax - height[r]; 
            //    } 
            //}
            //Console.WriteLine("Trapped Water : " + res);
            // ---------------------------------------------------------------------------------------
            //// 7). Buy & Sell Stocks
            //int[] prices = { 7, 1, 5, 3, 6, 4 };
            //int profit = 0;
            //int buyPrice = int.MaxValue;
            //for (int i = 0; i < prices.Length; i++)
            //{
            //    if (buyPrice < prices[i])
            //    {
            //        profit = (prices[i] - buyPrice) > profit ? prices[i] - buyPrice : profit;
            //    }
            //    else
            //        buyPrice = prices[i];
            //}
            //Console.WriteLine("Profit : " + profit);
            // ---------------------------------------------------------------------------------------
            // Assignment Problems

            //// Q1. find if array contains duplicates or not
            //int[] nums = { 1, 2, 3, 1 };
            //// using linq
            //if (nums.Length != nums.Distinct().Count())
            //    Console.WriteLine("true");
            //else
            //    Console.WriteLine("false");

            ////or using for loop and hashset
            //var hashSet = new HashSet<int>();
            //foreach (var x in nums)
            //{
            //    if (!hashSet.Add(x))
            //    {
            //        Console.WriteLine("Contains duplicates");
            //        break;
            //    }
            //}

            // Q2. return index of the target element
            //int[] nums = { 4, 5, 6, 7, 0, 1, 2, 4 };
            //int target = 2;
            ////using linq
            //if (nums.Contains(target))
            //{
            //    int index = nums.Select((v, i) => new { v, i })
            //                                           .Where(r => r.v == target)
            //                                           .Select(p => p.i).FirstOrDefault();
            //    System.Console.Write("Index : " + index);
            //}
            ////convert array to string
            //var strArr = string.Join("", nums);
            //string tg = "4";
            //Console.Write("Index : " + strArr.IndexOf(tg));

            //// Q3. return triplets
            //int[] nums = [-1, 0, 1, 2, -1, -4];
            //List<List<int>> result = new List<List<int>>();
            //for (int i = 0; i < nums.Length; i++)
            //{
            //    for (int j = i + 1; j < nums.Length; j++)
            //    {
            //        for (int k = j + 1; k < nums.Length; k++)
            //        {
            //            if (nums[i] + nums[j] + nums[k] == 0)
            //            {
            //                List<int> triplet = new List<int>();
            //                triplet.Add(nums[i]);
            //                triplet.Add(nums[j]);
            //                triplet.Add(nums[k]);
            //                triplet.Sort();
            //                if (!result.Equals(triplet))
            //                    result.Add(triplet);
            //            }
            //        }
            //    }
            //}
            //string str = "[ ";
            //foreach (var i in result)
            //{
            //    str += "[" + string.Join(", ", i.ToArray()) + "] ";
            //}
            //str += "]";
            //Console.Write("Output : " + str);            
        }

        public static int BinarySearch(int[] arr, int key, bool type)
        {
            if (type)
            {
                int start = 0, end = arr.Length - 1;

                while (start <= end)
                {
                    int mid = (start + end) / 2;

                    if (arr[mid] == key)
                        return mid;

                    if (arr[mid] < key)
                        start = mid + 1;
                    else
                        end = mid - 1;
                }
            }
            else
            {
                int start = 0, end = arr.Length - 1;

                while (start <= end)
                {
                    int mid = (start + end) / 2;

                    if (arr[mid] == key)
                        return mid;

                    if (arr[mid] < key)
                        end = mid - 1;
                    else
                        start = mid + 1;
                }
            }
            return -1;
        }

        public static string Sha1Encrypt(string userName, string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            ASCIIEncoding encoding = new ASCIIEncoding();

            string tmp = $"{password}{userName.ToUpper()}";
            return BitConverter.ToString(sha.ComputeHash(encoding.GetBytes(tmp))).Replace("-", string.Empty);
        }

        public static void BcryptVerify(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine("Hashed Password: " + hashedPassword);

            //// Verifying a password
            //string userEnteredPassword = "mySecurePassword";
            //bool passwordMatches = BCrypt.Net.BCrypt.Verify(userEnteredPassword, hashedPassword);

            //if (passwordMatches)
            //{
            //    Console.WriteLine("Password is correct!");
            //}
            //else
            //{
            //    Console.WriteLine("Password is incorrect.");
            //}
        }

        public static void Hashset()
        {
            // Create a HashSet to store arrays
            HashSet<ArrayContainer> arrayHashSet = new HashSet<ArrayContainer>();

            // Example arrays
            int[] array1 = { 1, 2, 3 };
            int[] array2 = { 4, 5, 6 };
            int[] array3 = { 1, 2, 3 }; // Duplicate of array1

            // Create instances of ArrayContainer and add them to the HashSet            
            arrayHashSet.Add(new ArrayContainer(array1));
            arrayHashSet.Add(new ArrayContainer(array2));
            arrayHashSet.Add(new ArrayContainer(array3));

            // Display the unique arrays stored in the HashSet
            foreach (var arrayContainer in arrayHashSet)
            {
                Console.Write(string.Join(", ", arrayContainer.Array));
                Console.Write(" | Hashset --> " + arrayContainer.Array.GetHashCode());
                Console.WriteLine(" | Equals --> " + arrayContainer.Equals(arrayContainer.Array));
            }
        }


    }

    public class ArrayContainer
    {
        public int[] Array { get; }

        public ArrayContainer() { }

        public ArrayContainer(int[] array)
        {
            Array = array;
        }

        // Implement GetHashCode and Equals for proper HashSet behavior
        public override int GetHashCode()
        {
            return Array.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ArrayContainer other)
            {
                return Array.SequenceEqual(other.Array);
            }
            return false;
        }
    }
}