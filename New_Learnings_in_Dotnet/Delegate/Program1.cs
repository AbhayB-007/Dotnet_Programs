using System;

namespace Program
{
    //Step 1: Defining a delegate
    public delegate void AddDelegate(int x, int y);
    public delegate string SayDelegate(string str);

    class Program1
    {
        // Defining a delegate and can also be defined inside class
        private delegate void rot13(string str);

        //Method 1:
        public void AddNums(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        //Method 2:
        public static string SayHello(string name)
        {
            return "Hello " + name;
        }

        //Method 3:
        public static void ROT13(string input)
        {
            if (string.IsNullOrEmpty(input)) { return; }            

            char[] buffer = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c >= 97 && c <= 122)
                {
                    int j = c + 13;
                    if (j > 122) j -= 26;
                    buffer[i] = (char)j;
                }
                else if (c >= 65 && c <= 90)
                {
                    int j = c + 13;
                    if (j > 90) j -= 26;
                    buffer[i] = (char)j;
                }
                else
                {
                    buffer[i] = (char)c;
                }
            }
            Console.WriteLine("Given String : " + input  + " | Encoded String (ROT13) : " + new string(buffer));            
        }

        public static void Main()
        {
                        /*Simple way of calling a methods*/
            //-------------------------------------------------------//
            //1). call by creating objects
            Console.WriteLine("\tSimple way of calling a methods");
            Console.WriteLine("-------------------------------------------------------");
            Program1 obj1 = new Program1();
            obj1.AddNums(1, 2);
            //2). call using class name (static methods)
            Console.WriteLine(Program1.SayHello("Abhay"));
            Program1.ROT13("Abhay");
            Console.WriteLine("-------------------------------------------------------\n");

                        /*Using delegate for calling a methods*/
            //-------------------------------------------------------//
            //Step 2: Instantiating the delegate
            AddDelegate ad = new AddDelegate(obj1.AddNums); // non-static | return type void
            SayDelegate sd = new SayDelegate(Program1.SayHello);// static | return type string            
            rot13 rt = new rot13(Program1.ROT13);
            
            //Step 3: calling methods using delegates
            Console.WriteLine("\tUsing delegate for calling a methods");
            Console.WriteLine("-------------------------------------------------------");
            ad(5, 5);
            Console.WriteLine(sd("Delegate"));
            rt("Delegate");
            Console.WriteLine("-------------------------------------------------------\n");
        }
    }
}
