using BenchmarkDotNet.Attributes;

namespace Csv.ConsoleApp
{
    [MemoryDiagnoser]
    public class CsvCheckerBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void ProcessCsv()
        {
            var input = "C:\\Repos\\input.csv";
            var output = "C:\\Repos\\Csv\\Csv.ConsoleApp\\output.csv";
            var error = "C:\\Repos\\Csv\\Csv.ConsoleApp\\errorLogs.txt";
            var processor = new CsvProcessor(input, output, error);
            processor.ProcessCsv();
        }
    }
}
