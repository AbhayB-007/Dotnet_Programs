using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class Properties
    {
        static void Main (string[] args)
        {
            Accounts acc1 = new Accounts();
            Accounts acc2 = new Accounts(500);

            Console.WriteLine($"Account 1: Initial Amount = {acc1.InitialAmount}, Interest Rate = {Accounts.InterestRate}");
            Console.WriteLine($"Account 2: Initial Amount = {acc2.InitialAmount}, Interest Rate = {Accounts.InterestRate}");
        }
    }

    public class Accounts
    {
        float init_amount;
        static float interest;

        public float InitialAmount
        {
            set
            {
                if (value < 1000)
                {
                    Console.WriteLine("Initial amount must be greater than 1000");
                }
                else
                {
                    init_amount = value;
                }
            }
            get { return init_amount; }
        }

        public static float InterestRate
        {
            private set { interest = value; }
            get { return interest; }
        }

        // default constructor
        public Accounts()
        {
            this.InitialAmount = 1000;
        }

        // parameterized constructor
        public Accounts(float amt)
        {
            this.InitialAmount = amt;
        }

        // static constructor
        static Accounts()
        {
            Accounts.InterestRate = 9.5f;
        }


    }
}
