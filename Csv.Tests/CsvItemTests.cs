using Csv.ConsoleApp;
using NUnit.Framework;

namespace Csv.Tests
{
    public class CsvItemTests : CsvItem
    {
        private CsvItem _csvItem;
        private static readonly string[] _referenceSortedColumns = new string[]
        {
            "policyid",
            "clientid",
            "countrycode",
            "cost",
            "income"
        };

        const short NUMBER_OF_COLUMNS = 5;

        public CsvItemTests() : base("", _referenceSortedColumns)
        { 
        } 

        [SetUp]
        public void Setup()
        {
            _csvItem = new CsvItem("", _referenceSortedColumns);
        }

        [Test]
        public void Valid_Header_Should_Return_True()
        {
            var columns = new string[] {
                "policyid",
                "clientid",
                "countrycode",
                "cost",
                "income"
            };

            var value = IsValidHeader(columns, _referenceSortedColumns);

            Assert.AreEqual(true, value);
        }

        [Test]
        public void Missing_Header_Should_Return_False()
        {
            var columns = new string[] {
                "policyid",
                "clientid",
                "countrycode",
                "cost"
            };

            var value = IsValidHeader(columns, _referenceSortedColumns);

            Assert.AreEqual(false, value);
        }

        [Test]
        public void Wrong_Header_Name_Should_Return_False()
        {
            var columns = new string[] {
                "policyid-123",
                "clientid",
                "countrycode",
                "cost",
                "income-abc"
            };

            var value = IsValidHeader(columns, _referenceSortedColumns);

            Assert.AreEqual(false, value);
        }

        [Test]
        public void All_Columns_Have_Content_Should_Return_True()
        {
            var columns = new string[] {
                "policyId123",
                "12",
                "de",
                "1.2",
                "1.2"
            };

            var value = HasValidColumns(columns, NUMBER_OF_COLUMNS);

            Assert.AreEqual(true, value);
        }

        [Test]
        public void Some_Columns_Are_Null_Should_Return_False()
        {
            var columns = new string[] {
                "policyId123",
                "12",
                "",
                "1.2",
                ""
            };

            var value = HasValidColumns(columns, NUMBER_OF_COLUMNS);

            Assert.AreEqual(false, value);
        }

        [Test]
        public void Some_Columns_Are_Missing_Should_Return_False()
        {
            var columns = new string[] {
                "policyId123",
                "12",
                "1.2"
            };

            var value = HasValidColumns(columns, NUMBER_OF_COLUMNS);

            Assert.AreEqual(false, value);
        }

        [Test]
        public void Valid_Line_Text_Should_Return_Valid_Csv_Item()
        {
            var line = "123def45,68,de,3.43,5.13";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual("123def45", csvItem.PolicyId);
            Assert.AreEqual(68, csvItem.ClientId);
            Assert.AreEqual("de", csvItem.CountryCode);
            Assert.AreEqual(3.43, csvItem.Cost);
            Assert.IsNull(csvItem.Errors);
        }

        [Test]
        public void Valid_Header_Should_Return_Valid_Csv_Item()
        {
            var line = "PolicyID,ClientID,CountryCode,Cost,Income";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.IsHeader, true);
            Assert.IsNull(csvItem.Errors);
        }

        [Test]
        public void Has_Extra_Header_Column_Should_Return_Errors()
        {
            var line = "PolicyID,ClientID,CountryCode,Cost,Income,Extra";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.IsHeader, true);
            Assert.AreEqual(csvItem.Errors, "Invalid Header - Should be (and in order of) PolicyID,ClientID,CountryCode,Cost,Income");
        }

        [Test]
        public void Has_Missing_Header_Column_Should_Return_Errors()
        {
            var line = "PolicyID,Cost,Income";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.IsHeader, true);
            Assert.AreEqual(csvItem.Errors, "Invalid Header - Should be (and in order of) PolicyID,ClientID,CountryCode,Cost,Income");
        }

        [Test]
        public void ClientID_Is_Negative_Should_Return_Errors()
        {
            var line = "123def45,-68,de,3.43,5.13";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "ClientId should be a whole number; ");
        }

        [Test]
        public void Cost_Is_Invalid_Number_Should_Return_Errors()
        {
            var line = "123def45,68,de,3.43a,5.13";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "Cost should be numeric; ");
        }

        [Test]
        public void Income_Is_Invalid_Number_Should_Return_Errors()
        {
            var line = "123def45,68,de,3.43,5s.13";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "Income should be numeric; ");
        }

        [Test]
        public void Has_Extra_Columns_Should_Return_Errors()
        {
            var line = "123def45,68,de,3.43,5.13,abc";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "Extra column/s found; ");
        }

        [Test]
        public void Has_Missing_Columns_Should_Return_Errors()
        {
            var line = "123def45,68,de";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "All 5 columns are mandatory; ");
        }

        [Test]
        public void Income_Is_Invalid_And_ClientID_Is_Invalid_Should_Return_Errors()
        {
            var line = "123def45,gh68,de,3.43,yy.13";
            var csvItem = new CsvItem(line, _referenceSortedColumns);

            Assert.AreEqual(csvItem.Errors, "ClientId should be a whole number; Income should be numeric; ");
        }
    }
}
