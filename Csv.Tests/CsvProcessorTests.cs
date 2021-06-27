using Csv.ConsoleApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Csv.Tests
{
    public class CsvProcessorTests : CsvProcessor
    {
        private readonly string _projectDirectory;
        public CsvProcessorTests() : base("", "", "")
        {
            var workingDirectory = Environment.CurrentDirectory;
            _projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        }

        [Test]
        public void Generates_Valid_Client_Policy_Key()
        {
            var value = GetPolicyClientKey(61, "policy333");

            Assert.AreEqual("61-policy333", value);
        }

        [Test]
        public void PolicyID_Exists_In_ClientLookup()
        {
            var clientLookup = new HashSet<string> {
                "61-policy1",
                "62-policy2"
            };

            var value = IsPolicyIdExistingForClient(clientLookup, 61, "policy1");

            Assert.AreEqual(true, value);
        }

        [Test]
        public void PolicyID_Not_Exists_In_ClientLookup()
        {
            var clientLookup = new HashSet<string> {
                "61-policy1",
                "62-policy2"
            };

            var value = IsPolicyIdExistingForClient(clientLookup, 61, "policy123");

            Assert.AreEqual(false, value);
        }

        [Test]
        public void Generates_Valid_Output_File()
        {
            var dir = "Generates_Valid_File";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedResultFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedOutput.csv");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.OutputPath, expectedResultFilePath));
        }

        [Test]
        public void Generates_Valid_Output_File_No_Header()
        {
            var dir = "Generates_Valid_Output_File_No_Header";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedResultFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedOutput.csv");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.OutputPath, expectedResultFilePath));
        }

        [Test]
        public void Generates_Valid_Output_File_UpperCase_Content()
        {
            var dir = "Generates_Valid_Output_File_UpperCase_Content";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedResultFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedOutput.csv");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.OutputPath, expectedResultFilePath));
        }

        [Test]
        public void Generates_Error_File_Invalid_Header_Invalid_ClientId()
        {
            var dir = "Generates_Error_File_Invalid_Header_Invalid_ClientId";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedErrorFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedErrors.txt");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.ErrorFilePath, expectedErrorFilePath));
        }

        [Test]
        public void Generates_Error_File_PolicyID_Not_Unique_To_Client()
        {
            var dir = "Generates_Error_File_PolicyID_Not_Unique_To_Client";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedErrorFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedErrors.txt");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.ErrorFilePath, expectedErrorFilePath));
        }

        [Test]
        public void Generates_Error_File_Missing_Columns()
        {
            var dir = "Generates_Error_File_Missing_Columns";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedErrorFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedErrors.txt");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.ErrorFilePath, expectedErrorFilePath));
        }


        [Test]
        public void Generates_Error_File_Multiple_Errors_In_Single_Line()
        {
            var dir = "Generates_Error_File_Multiple_Errors_In_Single_Line";
            var files = new CsvGeneratedFiles(dir, _projectDirectory);
            var expectedErrorFilePath = Path.Combine(_projectDirectory, "Fixtures", dir, "expectedErrors.txt");
            var csv = new CsvProcessor(files.InputPath, files.OutputPath, files.ErrorFilePath);
            csv.ProcessCsv();

            Assert.AreEqual(true, AreFilesEqual(files.ErrorFilePath, expectedErrorFilePath));
        }

        private static bool AreFilesEqual(string file1, string file2)
        {
            var file1Content = File.ReadAllLines(file1);
            var file2Content = File.ReadAllLines(file2);
            return Enumerable.SequenceEqual(file1Content, file2Content);
        }
    }

    class CsvGeneratedFiles
    {
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string ErrorFilePath { get; set; }

        public CsvGeneratedFiles(string testDir, string projectDir)
        {
            InputPath = Path.Combine(projectDir, "Fixtures", testDir, "input.csv");
            OutputPath = Path.Combine(projectDir, "Fixtures", testDir, "output.csv");
            ErrorFilePath = Path.Combine(projectDir, "Fixtures", testDir, "errorLogs.txt");
        }
    }
}
