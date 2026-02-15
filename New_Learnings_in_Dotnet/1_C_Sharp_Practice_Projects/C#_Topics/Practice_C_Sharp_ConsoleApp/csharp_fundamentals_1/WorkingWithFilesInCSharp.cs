using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class WorkingWithFilesInCSharp
    {
        static void Main(string[] args)
        {
            string parentDirPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            string filePath = Path.Combine(parentDirPath, "csharp_fundamentals_1", "example.txt");


            //// Writing to a file
            //string contentToWrite = "1). Hello, this is a sample text.";
            //File.WriteAllText(filePath, contentToWrite);
            //Console.WriteLine("Content written to file.");

            //// Reading from a file
            //string contentRead = File.ReadAllText(filePath);
            //Console.WriteLine("Content read from file:");
            //Console.WriteLine("\n" + contentRead + "\n");

            //// Appending to a file
            //string contentToAppend = "\n2). This is an appended line.";
            //File.AppendAllText(filePath, contentToAppend);
            //Console.WriteLine("Content appended to file.");

            //// Reading the updated content
            //string updatedContent = File.ReadAllText(filePath);
            //Console.WriteLine("Updated content read from file:");
            //Console.WriteLine("\n" + updatedContent + "\n");

            //// 2nd time Appending to a file
            //contentToAppend = "\n3). This is an 2nd appended line.";
            //File.AppendAllText(filePath, contentToAppend);
            //Console.WriteLine("2nd time Content appended to file.");

            //// 2nd time Reading the updated content
            //updatedContent = File.ReadAllText(filePath);
            //Console.WriteLine("2nd time Updated content read from file:");
            //Console.WriteLine("\n" + updatedContent + "\n");

            //----------------------------------------------------------------
            //// Path Handling

            //Console.WriteLine($"File Path: {filePath}\n");

            //string fileName = Path.GetFileName(filePath);
            //Console.WriteLine($"File Name: {fileName}\n");

            //fileName = Path.GetExtension(filePath);
            //Console.WriteLine($"File Extension: {fileName}\n");

            //fileName = Path.GetDirectoryName(filePath)!;
            //Console.WriteLine($"Directory Name: {fileName} \n");

            //fileName = Path.GetFullPath(filePath);
            //Console.WriteLine($"Full Path: {fileName} \n");

            //----------------------------------------------------------------
            //// FileStream

            //FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            //byte[] data = Encoding.UTF8.GetBytes("This is some text written using FileStream.\n");
            //fs.Write(data, 0, data.Length);
            //fs.Close();
            

            //FileStream fsr = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //byte[] buffer = new byte[fsr.Length];
            //fsr.Read(buffer, 0, buffer.Length);
            
            //Console.WriteLine(
            //    "Content read from file using FileStream:\n" + Encoding.UTF8.GetString(buffer)
            //);


            //----------------------------------------------------------------
            //// Using StreamReader with FileStream

            //using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //using (StreamReader reader = new StreamReader(fs))
            //{
            //    // Read all at once
            //    string content = reader.ReadToEnd();
            //    //Console.WriteLine(content);

            //    // OR read line by line
            //    string line;
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        Console.WriteLine(line);                    
            //    }
            //}

            //----------------------------------------------------------------
            //// StreamReader & StreamWriter

            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("Hello there, I'm writing this using StreamWriter.");
            writer.Close();            

            using StreamReader reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();

            Console.WriteLine("Content read using StreamReader :\n" + content);

            //----------------------------------------------------------------



        }
    }
}