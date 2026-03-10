using Microsoft.AspNetCore.Http;
using System.Text;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class WorkingWithFilesInCSharp
    {
        public static string parentDirPath = string.Empty;
        public static string filePath = string.Empty;

        public static void WorkingWithFiles ()
        {
            parentDirPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            filePath = Path.Combine(parentDirPath, "csharp_fundamentals_1", "example.txt");


            //// Writing to a file
            //string contentToWrite = "1). Hello, this is a sample text using File.WriteAllText.";
            //File.WriteAllText(filePath, contentToWrite);
            //Console.WriteLine("Content written to file.");

            //// Reading from a file
            //string contentRead = File.ReadAllText(filePath);
            //Console.WriteLine("Content read from file using File.ReadAllText:");
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

            //using StreamWriter writer = new StreamWriter(filePath);
            //writer.WriteLine("Hello there, I'm writing this using StreamWriter.");
            //writer.Close();

            //using StreamReader reader = new StreamReader(filePath);
            //string content = reader.ReadToEnd();

            //Console.WriteLine("Content read using StreamReader :\n" + content);

            //----------------------------------------------------------------
            //// Async File I/O (Very Important for Modern .NET)

            //// using getawaiter().getresult() to call async method from sync main method
            //AsyncFileIOExample(filePath).GetAwaiter().GetResult();

            //// best for WPF/WinForms/ASP.NET Core where you want to avoid blocking the main thread
            //AsyncHelper.RunSync(() => AsyncFileIOExample(filePath));

            //// or
            //Task.Run(async () => await AsyncFileIOExample(filePath)).GetAwaiter().GetResult();
            //// or
            //Task.Run(() => AsyncFileIOExample(filePath)).GetAwaiter().GetResult();

            //Console.ReadKey();

            //----------------------------------------------------------------
            //// File Upload (ASP.NET Core Must Know)

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes("This is a sample file content for upload.");
            //var stream = new MemoryStream(bytes);

            //IFormFile formFile = new FormFile(
            //    baseStream: stream,
            //    baseStreamOffset: 0,
            //    length: stream.Length,
            //    name: "file",
            //    fileName: "sample.txt"
            //);

            //Upload(formFile);                       
            


        }

        /// <summary>
        /// Async File I/O Example 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<bool> AsyncFileIOExample(string filePath)
        {
            await File.WriteAllTextAsync(filePath, "Hello, this is a sample text written asynchronously.");
            string content = await File.ReadAllTextAsync(filePath);
            Console.WriteLine("Content read asynchronously:\n" + content);
            return content.Length > 0 ? true : false;
        }

        /// <summary>
        /// Example of async file upload in an ASP.NET Core controller
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static void Upload(IFormFile file)
        {
            var path = Path.Combine(parentDirPath, "csharp_fundamentals_1", file.FileName);

            var stream = new FileStream(path, FileMode.Create);
            AsyncHelper.RunSync(() => file.CopyToAsync(stream));
            stream.Close();
            Console.WriteLine("File saved to: " + path + "\n");

            var reader = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[reader.Length];
            reader.Read(buffer, 0, buffer.Length);
            reader.Close();
            Console.WriteLine("Content of uploaded file:\n" + Encoding.UTF8.GetString(buffer));
        }

    }

    /// <summary>
    /// created a helper wrapper class to call async methods from sync code without blocking the main thread, especially useful in WPF/WinForms/ASP.NET Core
    /// </summary>
    public static class AsyncHelper
    {
        public static T RunSync<T>(Func<T> func)
            => Task.Run(func).GetAwaiter().GetResult();
    }
}