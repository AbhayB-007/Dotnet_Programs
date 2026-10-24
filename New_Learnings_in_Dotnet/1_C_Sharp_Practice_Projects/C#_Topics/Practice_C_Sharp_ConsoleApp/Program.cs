using System.Runtime.InteropServices;

namespace Practice_C_Sharp_ConsoleApp
{
    public class Program
    {
        [DllImport("user32.dll")]
        static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public static void Main(string[] args)
        {
            // MessageBox(IntPtr.Zero, "Hello, World!", "Message Box", 0);      
            Program program = new Program(new int[] { 1, 2, 3, 4, 5 });
            string result = string.Join(", ", program.nums);
            Console.WriteLine($"The numbers are: {result}");
        }

        private int[] nums;

        public Program(int[] nums)
        {
            this.nums = nums;
        }

    }
}
