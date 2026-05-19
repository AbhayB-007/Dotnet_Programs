using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class Indexers
    {
        // indexers
        // 1). Allows an object to be indexed like an array.KD
        // 2). Instance of a class that defines an indexer can be accessed by array access operator [].
        // 3). Use get and set accessors to define the indexer.
        // 4). Index can only be defined by this keyword.

        public static void Main(string[] args)
        {
            Department dept = new Department();
            Console.WriteLine("Id: " + dept[1].ID);
            Console.WriteLine("EmpName: " + dept[1].EmpName);
            Console.WriteLine("Salary: " + dept[1].Salary);
        }
    }


    class Employee
    {
        public int ID { get; set; }
        public string EmpName { get; set; } = string.Empty;
        public double Salary { get; set; }
    }

    class Department
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; } = string.Empty;
        Employee[] EmpList;

        public Department()
        {
            DeptID = 10;
            DeptName = "Sales";
            EmpList = new Employee[3]
            {
                new Employee{ID=1, EmpName="John", Salary=50000},
                new Employee{ID=2, EmpName="Jane", Salary=60000},
                new Employee{ID=3, EmpName="Bob", Salary=55000}
            };
        }

        public Employee this[int index]
        {
            get
            {
                foreach (Employee emp in EmpList)
                {
                    if (emp.ID == index)
                        return emp;
                }
                return null;
            }            
        }

        public Employee this[string name]
        {
            get
            {
                foreach (Employee emp in EmpList)
                {
                    if (emp.EmpName == name)
                        return emp;
                }
                return null;
            }
        }
    }




}
