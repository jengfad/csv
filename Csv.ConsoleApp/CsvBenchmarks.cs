using BenchmarkDotNet.Attributes;

namespace Csv.ConsoleApp
{
    [MemoryDiagnoser]
    public class CsvCheckerBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void ProcessCsv()
        {
            var input = "C:\\Repos\\CsvChecker\\CsvChecker\\input.csv";
            var output = "C:\\Repos\\CsvChecker\\CsvChecker\\output.csv";
            var error = "C:\\Repos\\CsvChecker\\CsvChecker\\errorLogs.txt";
            var processor = new CsvProcessor(input, output, error);
            processor.ProcessCsv();
        }
    }
}
