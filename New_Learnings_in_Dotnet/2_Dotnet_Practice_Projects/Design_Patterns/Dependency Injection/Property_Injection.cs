using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Practice_Project.Dependency_Injection
{
    //Here we are implementing DI using seater or Property Injection.
    //---------------------------------------------------------------------
    //1). Dependency Injection is basically providing the objects that an object needs, instead of having it construct the objects themselves.
    //2). Setter or property injection is injecting dependent class object through the property.
    //3). So setter or property injection means, injecting SavingAccount & CurrentAccount class objects in Account class using property

    class Account2
    {
        public IAccount account { get; set; }

        public void PrintAccounts()
        {
            account.PrintDetails();
        }

    }

    public class Property_Injection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("This is seater or Property Injection.\n");

            // calling SavingAcoount method
            Account2 sa = new Account2();
            sa.account = new SavingAccount();
            sa.PrintAccounts();

            // calling CurrentAcoount method            
            Account2 ca = new Account2();
            ca.account = new CurrentAcoount();
            ca.PrintAccounts();

            Console.ReadLine();
        }
    }
}
