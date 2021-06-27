using System.Linq;
using System.Text;

namespace Csv.ConsoleApp
{
    public class CsvItem
    {
        public string PolicyId { get; set; }
        public int ClientId { get; set; }
        public string CountryCode { get; set; }
        public decimal Cost { get; set; }
        public bool IsHeader { get; set; }
        public string Errors { get; set; }
        public bool HasErrors => !string.IsNullOrWhiteSpace(Errors);

        const short NUMBER_OF_COLUMNS = 5;

        public CsvItem(string lineText, string[] referenceHeaderColumns)
        {
            var inputColumns = lineText.Split(',').Select(text => text.ToLowerInvariant().Trim()).ToArray();

            if (inputColumns.Intersect(referenceHeaderColumns).Any())
            {
                IsHeader = true;
                if (!IsValidHeader(inputColumns, referenceHeaderColumns))
                    Errors = "Invalid Header - Should be (and in order of) PolicyID,ClientID,CountryCode,Cost,Income";

                return;
            }

            if (!HasValidColumns(inputColumns, NUMBER_OF_COLUMNS))
            {
                Errors = "Invalid columns - should have 5 non-blank columns;";
                return;
            }

            var errors = new StringBuilder();

            PolicyId = inputColumns[0];
            if (!int.TryParse(inputColumns[1], out int clientId))
                errors.Append("ClientId should be a whole number; ");
            if (!decimal.TryParse(inputColumns[3], out decimal cost))
                errors.Append("Cost should be decimal; ");
            if (!decimal.TryParse(inputColumns[4], out _))
                errors.Append("Income should be decimal; ");

            if (errors.Length > 0)
            {
                Errors = errors.ToString();
                return;
            }

            // Successful Validation
            ClientId = clientId;
            Cost = cost;
            CountryCode = inputColumns[2];
        }

        private static bool IsValidHeader(string[] inputColumns, string[] fileColumns)
        {
            return Enumerable.SequenceEqual(inputColumns, fileColumns);
        }

        private static bool HasValidColumns(string[] textList, short numOfColumns)
        {
            return textList.All(text => !string.IsNullOrWhiteSpace(text)) && textList.Length == numOfColumns;
        }
    }
}
