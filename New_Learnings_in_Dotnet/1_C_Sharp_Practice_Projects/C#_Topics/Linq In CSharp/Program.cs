using System;
using System.Linq;
using System.Xml.Linq;

namespace LinqInCSharp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void main(string[] args)
        {
            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 1). Filtering operator
            ////--------------------------------------------------------------------------------------------------------------------------------------------

            //var str = new string[] { "humpty", "dumpty", "sat", "on", "a", "wall" };
            //var result = str.Where(x => x.Contains("ty", StringComparison.OrdinalIgnoreCase)).ToList();
            //foreach(var item in result)
            //{
            //    Console.WriteLine(item);
            //}

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 2). Group By
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //List<int> numbers = new List<int>() { 35, 44, 200, 84, 3987, 4, 199, 329, 446, 208 };
            //IEnumerable<IGrouping<int, int>> query = (from number in numbers group number by number % 3);
            //foreach (var group in query)
            //{
            //    Console.WriteLine(group.Key == 0 ? "Even Number : " + group.Key : "Odd Number : " + group.Key);
            //    foreach (var item in group)
            //    {
            //        Console.WriteLine(item);
            //    }
            //}            

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 3). Concat operator
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //var str1 = new string[] { "humpty", "dumpty", "sat", "on", "a", "wall" };
            //var str2 = new string[] { "humpty", "dumpty2", "sat3", "on4", "a5", "wall6" };
            //var str3 = "Abhay";
            //var str4 = "Bindal";
            //var result = str1.Select(x => x).Concat(str2.Select(x => x)).ToList();
            //var result2 = str1.Union(str2).ToList();
            //string res = string.Concat(str3, str4);
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item);
            //}

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 4). Order By operator
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //char[] chr = new char[] { 'd', 'e', 'A', 'B', 'C', 'Y', 'z', 'r', 'a' };
            //var res = (from c in chr orderby c select c).ToList();
            //var res1 = chr.OrderBy(x => x);
            //var res2 = chr.OrderByDescending(x => x);
            //var chrArray = String.Join(", ", res);
            //var chrArray1 = String.Join(", ", res1);
            //var chrArray2 = String.Join(", ", res2);
            //Console.WriteLine("OrderBy using query : " + chrArray);
            //Console.WriteLine("OrderBy : " + chrArray1);
            //Console.WriteLine("OrderByDescending : " + chrArray2);            

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 5). Seqential Equal operator
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //Pet pet1 = new Pet() { Name = "Dog", Age = 12 };
            //Pet pet2 = new Pet() { Name = "cat", Age = 8 };
            //Pet pet3 = new Pet() { Name = "Dinosaur", Age = 10000 };

            //var list1 = new List<Pet>() { pet1, pet2};
            //var list2 = new List<Pet>() { pet1, pet2};
            //var list3 = new List<Pet>() { pet1, pet2, pet3};

            //bool res1 = list1.SequenceEqual(list2);
            //bool res2 = list2.SequenceEqual(list3);

            //Console.WriteLine(" res1 : {0} \n res2 : {1}", res1, res2);

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 6). Data Source Transformation
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //List<Student> students = new List<Student>()
            //{
            //    new Student()
            //    {
            //        First = "Ajey ",
            //        Last = "singh",
            //        ID = 123,
            //        City = "Seattle"
            //    },
            //    new Student()
            //    {
            //        First = "Abhay",
            //        Last = "Bindal",
            //        ID = 456,
            //        City = "New Delhi"
            //    },
            //    new Student()
            //    {
            //        First = "Shally",
            //        Last = "Tyagi",
            //        ID = 789,
            //        City = "New Zersey"
            //    },
            //    new Student()
            //    {
            //        First = "Shubham",
            //        Last = "Dubey",
            //        ID = 852,
            //        City = "UP-70"
            //    },
            //};

            //List<Teacher> teacher = new List<Teacher>()
            //{
            //    new Teacher()
            //    {
            //        First = "Anee ",
            //        Last = "Bee",
            //        ID = 123,
            //        City = "Seattle"
            //    },
            //    new Teacher()
            //    {
            //        First = "Alex",
            //        Last = "Robinson",
            //        ID = 456,
            //        City = "New Delhi"
            //    },
            //    new Teacher()
            //    {
            //        First = "Michio",
            //        Last = "Sato",
            //        ID = 789,
            //        City = "New Zersey"
            //    },
            //    new Teacher()
            //    {
            //        First = "Prabsimar",
            //        Last = "Kaur",
            //        ID = 852,
            //        City = "UP-70"
            //    },
            //};

            //var peopleInSeattle = (from i in students
            //                       where i.City == "Seattle"
            //                       select string.Concat(i.First, i.Last)).Concat(
            //    from t in teacher
            //    where t.City == "Seattle"
            //    select string.Concat(t.First, t.Last)
            //    ).ToList();

            //var Result = String.Join(",", peopleInSeattle);
            //Console.WriteLine(Result);

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 7). Area of a circle
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //double[] radii = new double[] { 1, 2, 3 };
            //IEnumerable<string> res = (from r in radii select String.Format("Area is {0}", (r * r) * 3.14));
            //foreach (string str in res)
            //    Console.WriteLine(str);

            ////----------------------------------------------------------------------------------------------------------------------------------------------
            //// 8). Linq to XMl document
            ////----------------------------------------------------------------------------------------------------------------------------------------------

            //List<Student> students = new List<Student>()
            //{
            //    new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores = new List<int>{97, 92, 81, 60}},
            //    new Student {First="Claire", Last="O'Donnell", ID=112, Scores = new List<int>{75, 84, 91, 39}},
            //    new Student {First="Sven", Last="Mortensen", ID=113, Scores = new List<int>{88, 94, 65, 91}},
            //};
            //// Create the query.
            //var studentsToXML = new XElement("Root",
            //from student in students
            //let x = String.Format("{0},{1},{2},{3}", student.Scores[0],
            //        student.Scores[1], student.Scores[2], student.Scores[3])
            //select new XElement("student",
            //    new XElement("First", student.First),
            //    new XElement("Last", student.Last),
            //    new XElement("Scores", x)
            //    ) // end "student"
            //); // end "Root"
            //   // Execute the query.
            //Console.WriteLine(studentsToXML);
            //// Keep the console open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }
    }

    public class Student
    {
        public string First { get; set; }
        public string Last { get; set; }
        public int ID { get; set; }
        public string City { get; set; }
        public List<int> Scores { get; set; }
    }
    public class Teacher
    {
        public string First { get; set; }
        public string Last { get; set; }
        public int ID { get; set; }
        public string City { get; set; }
    }
    public class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}