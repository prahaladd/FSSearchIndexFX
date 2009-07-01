using System;
using System.Collections.Generic;
using System.Text;

using Taurus.FindFiles.IndexInfra;

namespace Taurus.FindFiles.Engines
{


    public class FileSearchEngine
    {

        #region Constructors

        public FileSearchEngine(string rootPath)
        {
            _rootPath = rootPath;
            
        }

        #endregion



        #region Public methods

        public Dictionary<string, List<int>> SearchFilesForPattern(string pattern,bool useIndex)
        {

            if (!useIndex)
            {
                _fileContentFinder = new FileContentFinder(_rootPath, pattern);

                _fileContentFinder.SearchInFiles();


                return _fileContentFinder.FileNameToOccurencePositions;
            }
            else
            {
                return SearchFilesUsingIndex(pattern);
            }
        }

      
        #endregion


        #region Private methods

        public Dictionary<string,List<int>> SearchFilesUsingIndex(string pattern)
        {

            Dictionary<string, List<int>> fileNamesToOccurencePostions = new Dictionary<string, List<int>>();


            //determine whether the word is present in the dictionary.
            if (FileIndexDatabase.IndexRecords.ContainsKey(pattern))
            {
                

                List<IndexRecord> indexRecords = FileIndexDatabase.IndexRecords[pattern];

                foreach (IndexRecord record in indexRecords)
                {
                    fileNamesToOccurencePostions.Add(record.FileName, record.OccurencePositions);
                }

            }
            return fileNamesToOccurencePostions;
        }

        #endregion

        #region Private properties

        private IndexDatabase FileIndexDatabase
        {
            get
            {
                // we will have ONE and ONLY ONE instance of the index database 
                //alive for an instance of the FileSearchEngine.

                if (null == _indexDatabase)
                {
                    IndexDatabaseReader idbReader = new IndexDatabaseReader();

                    _indexDatabase = idbReader.Read();
                         
                }
                return _indexDatabase;

            }

        }
        #endregion

        #region Private members

        private string _rootPath;

        private bool _searchUsingIndex;

        private FileContentFinder _fileContentFinder;

        private IndexDatabase _indexDatabase;

        

        #endregion

    }
}
