using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace Dotnet_Practice_Project.Dependency_Injection
{
    public class DI_Notes
    {
        static void DI()
        {
            Console.Write("--------------------------------------------------------------------------------------------------------------------------------------------------------" +
                          "\nWhat is Tight Coupling?" +
                          "\nAns => Tight coupling is when a group of classes are highly dependent on one another." +

                          "\n\nDisadvantages of Tight Coupling" +
                          "\n1). Its defination itself a disadvantage." +
                          "\n2). This scenario arises when a class assumes too many responsibilities, or when one concern " +
                          "is spread over many classes rather than having its own class." +
                          "\n3). Difficult to maintain." +
                          "\n4). Difficult to test." +

                          "\n\nLosse Coupling" +
                          "\nAns => Loose coupling means that the classes are independent or have very less dependency of each other." +

                          "\n\nAdvantages of Loose Coupling" +
                          "\n1). Loose coupling is achieved by means of a design that promotes single-responsibility and seperation of concerns." +
                          "\n2). Easy to maintain." +
                          "\n3). Easy to test." +

                          "\n\nImportant Questions ?" +
                          "\nNow the question is, how to avoid this tightly coupled state ?" +
                          "\n1). The answer is by using Dependency injection." +
                          "\n2). Dependency injection is achieved using interfaces." +
                          "\n3). Interfaces are a powerful tool to use for decoupling." +
                          "\n4). Classes can communicate through interfaces rather than other concrete classes." +

                          "\n\nWhat is Dependency Injection ?" +
                          "\n1). Dependency Injection(DI) is a software pattern." +
                          "\n2). Dependency Injection is basically providing the objects that an object needs, instead of having it construct the objects themselves." +
                          "\n3). DI is a technique whereby one object supplies the dependencies of another object." +
                          "\n4). With the help of DI, we can write loosely coupled code." +
                          "\n5). DI is achieved by writing loosely couple code." +
                          "\n6). A loosely-coupled code is a code where all your classes can work independently without relying on each other." +

                          "\n\nTYPES OF DEPENDENCY INJECTION ?" +
                          "\nThere are 3 types of DI in C#" +
                          "\n1). Constructor Injection -> It is nothing but the process of injecting dependent class object through the constructor." +
                          "\n2). Setter or property Injection -> " +
                          "\n3). Method Injection" +                                                    
                          "\n--------------------------------------------------------------------------------------------------------------------------------------------------------"
                    );
        }

        public static void Main(string[] args)
        {
            DI();            
        }
    }
}
