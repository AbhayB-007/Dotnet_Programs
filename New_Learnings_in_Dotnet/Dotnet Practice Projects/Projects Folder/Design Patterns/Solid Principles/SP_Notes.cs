using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid_Principles
{
    public class SP_Notes
    {
        public static void Notes()
        {
            Console.WriteLine("SOLID Principle" +
                "\n1). The SOLID principles are design principles that help create maintainable, scalable, and flexible software systems. These principles, coined by Robert C. Martin, provide guidelines for writing clean, modular, and loosely coupled code." +
                "\n2). Solid principles are interwind and interdependent." +
                "\n3). Solid principles are most effective when they are combined together." +
                "\n4). It is important to get a wholesome view of all the SOLID principles."
                );

            Console.WriteLine("\n\nSingle Responsibility Principle" +
                "\n1). Every software component should have one and only one responsibility." +
                "\n2). Aim for High Cohesion & Loose Coupling." +
                "\n\ti). Cohesion :- It is the degree to which the various parts of a software components are related." +
                "\n\tii). Coupling :- It is defined as the level of inter dependency between various software components." +
                "\n3). Following Single Responsiblity Principle can lead to considerable software maintenance costs."
                );

            Console.WriteLine("\n\nOpen Closed Principle" +
                "\n1). Software components should be closed for modification, but open for extension." +
                "\n\ti). Closed for modification :- new feature getting added to the software component, should NOT have to modify existing code." +
                "\n\tii). Open for extension :- A software component should be extendable to add a new feature or to add a new behaviour to it." +
                "\n2). Ease of adding new features." +
                "\n3). Leads to minimal cost if developing and testing software." +
                "\n4). Open closed principle often requires decoupling, which, in turn, automatically folow the Single responsibility principle." +
                "\n\nCaution" +
                "\n1). Do not follow the open closed priciple blindly." +
                "\n2). You will end up with a huge number of classes that can complicate your overall design." +
                "\n3). Make a subjective, rather than an objective decision."
                );

            Console.WriteLine("\n\nLiskov Substitution Principle" +
                "\nObjects should be replacable with their subtypes without affecting the correctness of the program."
                );

            Console.WriteLine("\n\nInterface Segration Principle" +
                "\n1). No client/class should be forced to depend on methods it does not use." +
                "\n\nTechniquess to Identify Voilation of ISP" +
                "\n1). Fat Interfaces." +
                "\n2). Interfaces with Low Cohesion." +
                "\n3). Empty Method Implementations."
                );

            Console.WriteLine("\n\nDependency Inversion Principle" +
                "\n1). High-level modules should not depend on low-level modules. Both should depend on abstractions." +
                "\n\tOR" +
                "\n2). Abstraction should not depend on details. Details should depend on abstractions."
                );
        }

        public static void Main(string[] args)
        {
            Notes();
        }
    }
}
