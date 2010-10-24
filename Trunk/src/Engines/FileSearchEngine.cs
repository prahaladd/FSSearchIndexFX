/*FileSearchEngine.cs - This class acts as a driver to start the keyword based search.
 *
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using Taurus.FindFiles.IndexInfra;
using System.Collections;
using Taurus.FindFiles.Utils;
using Taurus.FindFiles.FileFilter;

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

        public Dictionary<string, List<int>> SearchFilesForPattern(string pattern,bool useIndex,bool searchSubDirectories)
        {
            
            if (!useIndex)
            {
                _fileContentFinder = new FileContentFinder(_rootPath, pattern);

                _fileContentFinder.SearchInFiles(searchSubDirectories,_fileFilter);

                //could have used LINQ but my editor does not allow the syntax to be validated :(
                Dictionary<string, List<int>> fileNamesToOccurencePositions = (from fileEntry in _fileContentFinder.FileNameToOccurencePositions
                                                                               where fileEntry.Key.Contains(_rootPath)
                                                                               select fileEntry).ToDictionary(fileEntry => fileEntry.Key, fileEntry => fileEntry.Value);



                

               // return _fileContentFinder.FileNameToOccurencePositions;
                return fileNamesToOccurencePositions;
            }
            else
            {
                return SearchFilesUsingIndex(pattern,searchSubDirectories);
            }
        }


        
      
        #endregion


        #region Public properties

        /// <summary>
        /// Specify the File attribute filters if any that need to be applied during search.
        /// </summary>
        public FileMetaAttributeFilter FileFilter
        {
            get { return _fileFilter; }
            set { _fileFilter = value; }
        }

        #endregion

        #region Private methods

        private Dictionary<string,List<int>> SearchFilesUsingIndex(string pattern, bool searchSubDirectories)
        {

            Dictionary<string, List<int>> fileNamesToOccurencePostions = new Dictionary<string, List<int>>();


            //determine whether the word is present in the dictionary.
            if (FileIndexDatabase.IndexRecords.ContainsKey(pattern))
            {
                

                List<IndexRecord> indexRecords = FileIndexDatabase.IndexRecords[pattern];

                foreach (IndexRecord record in indexRecords)
                {
                    bool isFileInSpecifiedSearchPath = Utilities.IsFileInSpecifiedSearchPath(_rootPath,record.FileName,searchSubDirectories);
                    bool isFileMatchingFilterCriteria = true;
                    if (null != _fileFilter)
                        isFileMatchingFilterCriteria = Utilities.EvaluateMetaAttributeFilters(record.FileAttributes, _fileFilter);

                    if(isFileMatchingFilterCriteria && isFileInSpecifiedSearchPath)
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

        private FileMetaAttributeFilter _fileFilter;

        #endregion

    }
}
