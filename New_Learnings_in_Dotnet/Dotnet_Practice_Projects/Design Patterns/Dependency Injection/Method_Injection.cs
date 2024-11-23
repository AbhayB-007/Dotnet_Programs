using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Practice_Project.Dependency_Injection
{
    //Here we are implementing DI using Method Injection.
    //---------------------------------------------------------------------
    //Method Injection is injecting dependent class object through a class method.
    //What is mean by this?
    //In the given example, Account class has a dependency on SavingAccount and CurrentAccount classes.
    //So the method Injection means, injecting SavingAccount & CurrentAccount class objects directly into the Account class method using interface

    class Account3
    {     
        public void PrintAccounts(IAccount account)
        {
            account.PrintDetails();
        }
    }

    public class Method_Injection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("This is Method Injection.\n");
            Account3 a = new Account3();
            
            // calling CurrentAcoount method            
            a.PrintAccounts(new CurrentAcoount());

            // calling SavingAcoount method                     
            a.PrintAccounts(new SavingAccount());


            Console.ReadLine();
        }
    }
}
