using System.Security.Cryptography;
using System.Text;
using System.Linq;


namespace Practice // Note: actual namespace depends on the project name.
{
    public class Program
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
            int[] arr1 = { 1, -2, 6, -1, 3 };
            int[] prefix = new int[arr1.Length]; //prefix array
            prefix[0] = arr1[0];
            for (int k = 1; k < arr1.Length; k++)
            {
                prefix[k] = prefix[k - 1] + arr1[k];
            }
            int maxSum = 0;
            for (int i = 0; i < arr1.Length; i++)
            {
                for (int j = i; j < arr1.Length; j++)
                {
                    int sum = i == 0 ? prefix[j] : prefix[j] - prefix[i - 1];
                    if (maxSum < sum) { maxSum = sum; }
                    Console.Write(sum + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("maxSum : " + maxSum);



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

    }
}