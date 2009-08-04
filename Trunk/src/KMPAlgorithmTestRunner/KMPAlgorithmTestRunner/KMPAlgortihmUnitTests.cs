using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taurus.FindFiles.IndexInfra;
using Taurus.FindFiles.Engines;
using Taurus.FindFiles;

namespace KMPAlgorithmTestRunner
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class KMPAlgortihmUnitTests
    {
        public KMPAlgortihmUnitTests()
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
        public void TestIndexDatabaseSerialization()
        {
            IndexRecord indexRecord1 = new IndexRecord(@"C:\DummyFile.txt",new List<int>() { 1,2,3,4,5 });
            IndexRecord indexRecord2 = new IndexRecord(@"C:\DummyFile1.txt", new List<int>() { 1, 2, 3 });

            List<IndexRecord> indexRecords = new List<IndexRecord>() { indexRecord1, indexRecord2 };

            Dictionary<string,List<IndexRecord>> searchPatternToIndexRecords = new Dictionary<string,List<IndexRecord>>();

            

            searchPatternToIndexRecords.Add("Test",indexRecords);



            IndexDatabase indexDatabase = new IndexDatabase(searchPatternToIndexRecords,DateTime.Now);


            //serialize the index database
            IndexDatabaseWriter writer = new IndexDatabaseWriter();
            writer.Persist(indexDatabase);



            
        }

        [TestMethod]

        public void TestIndexDatabaseDeserialization()
        {
            IndexDatabaseReader reader = new IndexDatabaseReader();

            
            
            IndexDatabase database = reader.Read();

        }

        [TestMethod]

        public void TestIndexFunctionality()
        {
            IndexingEngine indexEngine = new IndexingEngine();

            //profile the time that is needed for the indexing of the C drive apprx
            //14 gigs in size

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            indexEngine.Index();

            stopWatch.Stop();

            Console.WriteLine("Indexed all drives in {0} minutes", stopWatch.Elapsed.Minutes);
        }

        [TestMethod]
        public void TestKMPAlgorithmLogic()
        {
            KMPEngine engine = new KMPEngine("ssl rocks!!", "ssl");
            List<int> occurences = engine.Find(true, true);


        }

        [TestMethod]
        public void TestIndexBasedSearchLogic()
        {
            FileSearchEngine fileSearchEngine = new FileSearchEngine(@"C:\Documents and Settings\prahaladd\My Documents\PeePaal help");

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Dictionary<string,List<int>> fileNamesToOccurencePositions = 
                fileSearchEngine.SearchFilesForPattern("excel", false);

            stopWatch.Stop();



        }
    }
}
