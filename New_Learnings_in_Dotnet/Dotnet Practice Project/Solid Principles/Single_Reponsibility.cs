using Dotnet_Practice_Project.Dependency_Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Practice_Project.Solid_Principles
{
    public class Single_Reponsibility
    {
        public static void Notes()
        {
            Console.WriteLine("Single Responsibility Principle" +
                "\n1). Every software component should have one and only one responsibility." +
                "\n2). Aim for High Cohesion & Loose Coupling." +
                "\n\ti). Cohesion :- It is the degree to which the various parts of a software components are related." +
                "\n\tii). Coupling :- It is defined as the level of inter dependency between various software components." +
                "\n3). Following Single Responsiblity Principle can lead to considerable software maintenance costs."
                );
        }

        public static void Main(string[] args)
        {
            Notes();
        }
    }
}
