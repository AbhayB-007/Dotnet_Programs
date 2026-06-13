using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    internal class Regex_In_CSharp
    {
        // Is a pattern that could be matched against an input text
        // .Net provides a regular expression engine that allows matching.
        // Pattern consists of one or more character literals, operators, or constructs.
        // System.Text.RegularExpressions provides Regex class which handles regular express tasks

        static void Main(string[] args)
        {
            string input = "The quick brown fox jumps over the lazy dog.";
            string pattern = @"\b\w{4}\b"; // Matches words with exactly 4 letters
            MatchCollection matches = Regex.Matches(input, pattern);
            Console.WriteLine("Words with exactly 4 letters:");
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }
        }
    }
}
