using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Taurus.FindFiles.FileFilter;
using Taurus.FindFiles.IndexInfra;
using Taurus.FindFiles.Utils;
using System.IO;

namespace KMPAlgorithmTestRunner.FilterFrameWorkTest
{
    /// <summary>
    /// Summary description for FilterFrameworkUTest
    /// </summary>
    [TestClass]
    public class FilterFrameworkUTest
    {
        public FilterFrameworkUTest()
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
        public void TestFilterEvaluationSuccess()
        {
            FilterCondition parentDirectoryCondition = new FilterCondition(FileMetaAttributesStringConstants.FileMetaAttributeParentDirectory, FileFilterOperator.Equals,
                                                                   @"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData", FilterAttributeDataType.String);
            
            List<FilterCondition> filterConditions = new List<FilterCondition>();
            filterConditions.Add(parentDirectoryCondition);
            
            //Get information about a test file and then create a dummy filter condition based on the 
            //file attribites of the test file

            FileInfo fileInfo = new FileInfo(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData\Microsoft.txt");

            FilterCondition fileCreateCondition = new FilterCondition(FileMetaAttributesStringConstants.FileMetaAttributeCreationTime, FileFilterOperator.Equals, fileInfo.CreationTime.Ticks.ToString(), FilterAttributeDataType.Long);
            FileMetaAttributes fileAttributes = new FileMetaAttributes(fileInfo.DirectoryName, fileInfo.CreationTime.Ticks, fileInfo.LastWriteTime.Ticks, fileInfo.Length, fileInfo.Extension, @"HORNET\\prahaladd");

            filterConditions.Add(fileCreateCondition);

            FileMetaAttributeFilter fileFilter = new FileMetaAttributeFilter(filterConditions);

            FileFilterEvaluator filterEvaluator = new FileFilterEvaluator(fileFilter, fileAttributes);

            bool result = filterEvaluator.EvaluateFilter();

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void TestFilterEvaluationFailure()
        {
            FilterCondition parentDirectoryCondition = new FilterCondition(FileMetaAttributesStringConstants.FileMetaAttributeParentDirectory, FileFilterOperator.Equals,
                                                                   @"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData", FilterAttributeDataType.String);
            FilterCondition fileExtensionFilterCondition = new FilterCondition(FileMetaAttributesStringConstants.FileMetaAttributeFileExtension, FileFilterOperator.Equals, "xls", FilterAttributeDataType.String);

            List<FilterCondition> filterConditions = new List<FilterCondition>();
            filterConditions.Add(parentDirectoryCondition);
            filterConditions.Add(fileExtensionFilterCondition);


            //Get information about a test file and then create a dummy filter condition based on the 
            //file attribites of the test file

            FileInfo fileInfo = new FileInfo(@"C:\Projects\Classifier\findfilesutil\Trunk\src\KMPAlgorithmTestRunner\KMPAlgorithmTestRunner\TestData\Microsoft.txt");

            FilterCondition fileCreateCondition = new FilterCondition(FileMetaAttributesStringConstants.FileMetaAttributeCreationTime, FileFilterOperator.Equals, fileInfo.CreationTime.Ticks.ToString(), FilterAttributeDataType.Long);
            FileMetaAttributes fileAttributes = new FileMetaAttributes(fileInfo.DirectoryName, fileInfo.CreationTime.Ticks, fileInfo.LastWriteTime.Ticks, fileInfo.Length, fileInfo.Extension, @"HORNET\\prahaladd");

            filterConditions.Add(fileCreateCondition);

            FileMetaAttributeFilter fileFilter = new FileMetaAttributeFilter(filterConditions);

            FileFilterEvaluator filterEvaluator = new FileFilterEvaluator(fileFilter, fileAttributes);

            bool result = filterEvaluator.EvaluateFilter();

            Assert.IsFalse(result);

        }
    }
}
