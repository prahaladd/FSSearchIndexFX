using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Taurus.FindFiles.IndexInfra;
using Taurus.FindFiles.Engines;
using Taurus.FindFiles;

namespace KMPAlgorithmTestRunner.SearchEngineTests
{
    /// <summary>
    /// Summary description for SearchEngineUTest
    /// </summary>
    [TestClass]
    public class SearchEngineUTest
    {
        public SearchEngineUTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestNonIndexBasedSearchLogic()
        {
            FileSearchEngine fileSearchEngine = new FileSearchEngine(@"C:\Documents and Settings\prahaladd\My Documents\PeePaal help");

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Dictionary<string, List<int>> fileNamesToOccurencePositions =
                fileSearchEngine.SearchFilesForPattern("excel", true, false);

            stopWatch.Stop();

        }

        [TestMethod]
        public void TestIndexBasedSearchLogic()
        {
            FileSearchEngine fileSearchEngine = new FileSearchEngine(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Dictionary<string, List<int>> searchResults = fileSearchEngine.SearchFilesForPattern("Hodgkin's", true, false);
            stopWatch.Stop();

            Assert.IsTrue(searchResults.Count > 0);
            Assert.IsTrue(searchResults.ContainsKey(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData\Microsoft.txt"));
            Console.WriteLine("Total time taken for index based search:{0} milliseconds", stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void TestIndexBasedSearchLogicRecursive()
        {
            FileSearchEngine fileSearchEngine = new FileSearchEngine(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Dictionary<string, List<int>> searchResults = fileSearchEngine.SearchFilesForPattern("SafeBoot", true, true);
            stopWatch.Stop();

            Assert.IsTrue(searchResults.Count > 0);
            Assert.IsTrue(searchResults.ContainsKey(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData\Technology\TechTarget Encryption Technologies.docx"));
            Console.WriteLine("Total time taken for index based search:{0} milliseconds", stopWatch.ElapsedMilliseconds);

        }
    }
}
