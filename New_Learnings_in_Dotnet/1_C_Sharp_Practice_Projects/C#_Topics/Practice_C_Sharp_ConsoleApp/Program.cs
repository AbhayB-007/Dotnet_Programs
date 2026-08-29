using System.Runtime.InteropServices;

namespace Practice_C_Sharp_ConsoleApp
{
    public class Program
    {
        [DllImport("user32.dll")]
        static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public static void Main(string[] args)
        {
          
        }

    }
}
