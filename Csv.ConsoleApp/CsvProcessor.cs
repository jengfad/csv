using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Csv.ConsoleApp
{
    public class CsvProcessor
    {
        const string KEY_SEPARATOR = "-";
        const int WRITE_BUFFER_SIZE = 65536;
        private HashSet<string> _clientPolicyLookup;
        private Dictionary<string, decimal> _countryCostLookup;
        private static readonly string[] _referenceSortedColumns = new string[]
        {
            "policyid",
            "clientid",
            "countrycode",
            "cost",
            "income"
        };
        private string _inputFilePath;
        private string _outputFilePath;
        private string _errorFilePath;

        public string ErrorFilePath => _errorFilePath;

        public bool HasErrors { get; set; }

        public CsvProcessor(string inputFilePath, string outputFilePath, string errorFilePath)
        {
            _countryCostLookup = new Dictionary<string, decimal>();
            _clientPolicyLookup = new HashSet<string>();
            _inputFilePath = inputFilePath;
            _outputFilePath = outputFilePath;
            _errorFilePath = errorFilePath;

            ClearFile(_errorFilePath);
        }

        public void ProcessCsv()
        {
            using (StreamWriter streamwriter = new StreamWriter(_errorFilePath, true, Encoding.UTF8, WRITE_BUFFER_SIZE))
            {
                var i = 0;
                foreach (string line in File.ReadLines(_inputFilePath))
                {
                    i++;

                    var csvItem = new CsvItem(line, _referenceSortedColumns);

                    if (csvItem.IsHeader && !csvItem.HasErrors)
                        continue;

                    if (!csvItem.HasErrors && IsPolicyIdExistingForClient(_clientPolicyLookup, csvItem.PolicyId, csvItem.ClientId))
                        csvItem.Errors = $"Policy {csvItem.PolicyId} already exists for Client {csvItem.ClientId}; ";

                    if (csvItem.HasErrors)
                    {
                        //log errors until reaching EOF
                        HasErrors = true;
                        streamwriter.WriteLine($"Error - Line {i}: {csvItem.Errors}");
                        continue;
                    }

                    if (!HasErrors)
                    {
                        _clientPolicyLookup.Add($"{csvItem.ClientId}{KEY_SEPARATOR}{csvItem.PolicyId}");
                        AggregateCountryCost(_countryCostLookup, csvItem);
                    }
                }
            }

            if (!HasErrors)
                WriteOutputToFile(_outputFilePath, _countryCostLookup);
        }

        private static void WriteOutputToFile(string outputPath, Dictionary<string, decimal> countryCostLookup)
        {
            ClearFile(outputPath);
            using (StreamWriter streamwriter = new StreamWriter(outputPath, true, Encoding.UTF8, WRITE_BUFFER_SIZE))
            {
                streamwriter.WriteLine($"CountryCode,TotalCost");
                foreach (var item in countryCostLookup)
                {
                    streamwriter.WriteLine($"{item.Key},{item.Value}");
                }
            }
        }

        private static void AggregateCountryCost(Dictionary<string, decimal> countryCostLookup, CsvItem csvItem)
        {
            if (countryCostLookup.ContainsKey(csvItem.CountryCode))
                countryCostLookup[csvItem.CountryCode] += csvItem.Cost;
            else
                countryCostLookup.Add(csvItem.CountryCode, csvItem.Cost);
        }

        private static void ClearFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static bool IsPolicyIdExistingForClient(HashSet<string> clientLookup, string policyId, int clientId)
        {
            return clientLookup.Contains($"{clientId}-{policyId}");
        }
    }
}
