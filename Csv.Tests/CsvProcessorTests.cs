using Csv.ConsoleApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv.Tests
{
    public class CsvProcessorTests : CsvProcessor
    {
        public CsvProcessorTests() : base("", "", "")
        {
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

    }
}
