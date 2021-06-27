using System;
using System.Diagnostics;

namespace Csv.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string answer;
            do
            {
                Console.WriteLine("-----------------------------");
                ProcessCsv();
                Console.Write("Try again? (Y/N): ");
                answer = Console.ReadLine().ToUpper();
                Console.WriteLine("-----------------------------");
            } while (answer == "Y");
        }

        static void ProcessCsv()
        {
            Console.WriteLine("Enter input file path:");
            var inputPath = Console.ReadLine();
            Console.WriteLine("Enter output file path:");
            var outputPath = Console.ReadLine();
            var errorFilePath = ("errorLogs.txt");

            var timer = new Stopwatch();
            timer.Start();

            var csv = new CsvProcessor(inputPath, outputPath, errorFilePath);
            csv.ProcessCsv();

            timer.Stop();
            var timeTaken = timer.Elapsed;

            if (csv.HasErrors)
                Console.WriteLine($"CSV Errors detected. See {errorFilePath}");
            else
                Console.WriteLine($"CSV Processing Successful. See output file. Time Elapsed - {timeTaken:m\\:ss\\.fff}");
        }
    }
}
