using Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1;

namespace Practice_C_Sharp_ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // ---------------------------------------------------
            // datatypes
            //DataTypes.AllDatatypes();


            Box<List<string>> textBox = new Box<List<string>>();
            textBox.Content = new List<string> { "Hello", "World", "!" };

            Console.WriteLine("Contents of the text box: " + string.Join("", textBox.Content));

            // ---------------------------------------------------
            // working with files
            //WorkingWithFilesInCSharp.WorkingWithFiles();

            // ---------------------------------------------------
            // error handling
            ErrorHandlingInCSharp.ErrorHandling();
        }
    }

    public class Box<T>
    {
        public T Content { get; set; }
    }
}
