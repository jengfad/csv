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
                if (inputColumns.Length > NUMBER_OF_COLUMNS)
                    Errors = "Extra column/s not allowed; ";
                else
                    Errors = "All 5 columns are mandatory; ";

                return;
            }

            var errors = new StringBuilder();

            PolicyId = inputColumns[0];
            if (!int.TryParse(inputColumns[1], out int clientId) || clientId < 0)
                errors.Append("ClientId should be a whole number; ");
            if (!decimal.TryParse(inputColumns[3], out decimal cost) || cost < 0)
                errors.Append("Cost should be a non-negative number; ");
            if (!decimal.TryParse(inputColumns[4], out _))
                errors.Append("Income should be numeric; ");

            if (errors.Length > 0)
                Errors = errors.ToString();

            ClientId = clientId;
            Cost = cost;
            CountryCode = inputColumns[2];
        }

        protected static bool IsValidHeader(string[] inputColumns, string[] fileColumns)
        {
            return Enumerable.SequenceEqual(inputColumns, fileColumns);
        }

        protected static bool HasValidColumns(string[] textList, short numOfColumns)
        {
            return textList.All(text => !string.IsNullOrWhiteSpace(text)) && textList.Length == numOfColumns;
        }
    }
}
