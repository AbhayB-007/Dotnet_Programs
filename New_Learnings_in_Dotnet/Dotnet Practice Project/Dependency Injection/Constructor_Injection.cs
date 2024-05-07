using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Practice_Project.Dependency_Injection
{
    //1). Here is an example of Tight coupling where Account class is depenndent on Current & Saving Account classes.
    //---------------------------------------------------------------------------------------------------------------

    //class CurrentAcoount
    //{
    //    public void PrintDetails()
    //    {
    //        Console.WriteLine("Details of Current Account.");
    //    }
    //}

    //class SavingAccount
    //{
    //    public void PrintDetails()
    //    {
    //        Console.WriteLine("Details of Saving Account.");
    //    }
    //}

    //class Account
    //{
    //    CurrentAcoount ca = new CurrentAcoount();
    //    SavingAccount sa = new SavingAccount();
    //    public void PrintAccount()
    //    {
    //        ca.PrintDetails();
    //        sa.PrintDetails();
    //    }
    //}

    //public class Constructor_Injection
    //{
    //    public static void Main(string[] args)
    //    {
    //        Account a = new Account();
    //        a.PrintAccount();
    //        Console.ReadLine();
    //    }
    //}

    //---------------------------------------------------------------------------------------------------------------



    //2). Here we are implementing Constructor Injection.
    //---------------------------------------------------------------------------------------------------------------

    public interface IAccount
    {
        void PrintDetails();
    }

    class CurrentAcoount : IAccount
    {
        public void PrintDetails()
        {
            Console.WriteLine("Details of Current Account.");
        }
    }

    class SavingAccount : IAccount
    {
        public void PrintDetails()
        {
            Console.WriteLine("Details of Saving Account.");
        }
    }

    class Account
    {
        private IAccount account;

        public Account(IAccount account) //Paramerterized Constructor
        {
            this.account = account;
        }

        public void PrintAccount()
        {
            account.PrintDetails();
        }
    }

    public class Constructor_Injection
    {
        public static void Main(string[] args)
        {
            // calling CurrentAcoount method
            IAccount ca = new CurrentAcoount();
            Account a1 = new Account(ca);
            a1.PrintAccount();

            // calling SavingAcoount method
            IAccount sa = new SavingAccount();
            Account a2 = new Account(sa);
            a2.PrintAccount();

            Console.ReadLine();
        }
    }
}
