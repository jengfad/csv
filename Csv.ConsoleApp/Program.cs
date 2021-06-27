using System;
using System.IO;

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
            var inputPath = GetInputPath();
            var outputPath = GetOutputPath();
            var errorFilePath = GetErrorFilePath(outputPath);

            var csv = new CsvProcessor(inputPath, outputPath, errorFilePath);
            csv.ProcessCsv();

            if (csv.HasErrors)
                Console.WriteLine($"CSV Errors detected. See {csv.ErrorFilePath}");
            else
                Console.WriteLine($"CSV Processing Successful. See {csv.OutputFilePath}");
        }

        static string GetInputPath()
        {
            string inputPath;
            bool isValidInput;
            do
            {
                Console.WriteLine("Enter INPUT file path (relative or full file path):");
                inputPath = Console.ReadLine();
                isValidInput = inputPath.IsValidPath();

                if (!isValidInput)
                    Console.WriteLine("You entered an invalid input file path.");
            } while (!isValidInput);

            return inputPath.GetFilepath();
        }

        static string GetOutputPath()
        {
            string outputPath;
            bool isValidOutput;
            do
            {
                Console.WriteLine("Enter OUTPUT file path (relative or full file path):");
                outputPath = Console.ReadLine();
                isValidOutput = Path.IsPathRooted(outputPath) 
                    ? outputPath.HasValidDirectory()
                    : true;

                if (!isValidOutput)
                    Console.WriteLine("You entered an invalid output file path.");
            } while (!isValidOutput);

            return outputPath.GetFilepath();
        }

        static string GetErrorFilePath(string outputFilePath)
        {
            var outputFileName = Path.GetFileName(outputFilePath);
            return outputFilePath.Replace(outputFileName, "errorLogs.txt");
        }
    }
}
