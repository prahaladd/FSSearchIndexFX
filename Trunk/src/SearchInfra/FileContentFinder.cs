using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Taurus.FindFiles.Utils;


namespace Taurus.FindFiles
{
    public class FileContentFinder
    {

        //TODO: this constructor may have to be deprecated going further.
        public FileContentFinder(string rootPath, string pattern)
        {
            _searchRootPath = rootPath;
            _searchPattern = pattern;
        }

        public FileContentFinder(string searchPattern)
        {
            _searchRootPath = string.Empty;
            _searchPattern = searchPattern;


        }

        public void  SearchInFiles()
        {
            DirectoryTreeWalker directoryTreeWalker = new DirectoryTreeWalker();
            directoryTreeWalker.SearchSubDirectories = true;
            directoryTreeWalker.WalkTheTree(SearchRootPath);

            //ok now the tree has been walked. Get the list of all the text files and then determine the presence of the strings
            string fileContent = string.Empty;


            foreach (string file in directoryTreeWalker.Files)
            {
                if (!Utilities.IsTextFile(file))
                    continue;

                fileContent = Utilities.ReadTextFromFiles(file);

                KMPEngine kmpEngine = new KMPEngine(fileContent, SearchPattern);

                List<int> occurences = kmpEngine.Find(true, true);

                if (occurences.Count == 0)
                    continue;

                if (occurences[0] != -1)
                {
                    
                    _fileNamesWithContentPresent.Add(file);

                    _fileNameToOccurencePositionsMap.Add(file, occurences);
                }
            }
        }

        #region Internal methods
        
        internal void SearchInFiles(List<string> filesToSearch)
        {
            if (filesToSearch.Count == 0)
            {
                throw new ArgumentException("The list of files specified for search cannot be empty.");
            }

            foreach (string file in filesToSearch)
            {
                if (!Utilities.IsTextFile(file))
                    continue;

                string fileContent = Utilities.ReadTextFromFiles(file);

                KMPEngine kmpEngine = new KMPEngine(fileContent, SearchPattern);

                List<int> occurences = kmpEngine.Find(true, true);

                if (occurences.Count == 0)
                    continue;

                if (occurences[0] != -1)
                {

                    _fileNamesWithContentPresent.Add(file);

                    _fileNameToOccurencePositionsMap.Add(file, occurences);
                }
            }
        }

        #endregion

        #region Public properties

        //TODO: This member may have to be deprecated going further
        public string SearchRootPath
        {
            get { return _searchRootPath; }
            
        }

        public string SearchPattern
        {
            get { return _searchPattern; }
        }

        public List<string> FileNames
        {
            get { return _fileNamesWithContentPresent; }
        }

        public Dictionary<string, List<int>> FileNameToOccurencePositions
        {
            get { return _fileNameToOccurencePositionsMap; }

        }


        #endregion

        #region Private methods

        private  string ReadTextFile(string textFileName)
        {

            TextReader textReader = new StreamReader(File.Open(textFileName, FileMode.Open, FileAccess.Read));

            return textReader.ReadToEnd();
        }

        #endregion

        #region Private members

        private string _searchRootPath;
        private string _searchPattern;

        List<string> _fileNamesWithContentPresent = new List<string>();

        Dictionary<string, List<int>> _fileNameToOccurencePositionsMap = new Dictionary<string, List<int>>();


        #endregion
    }
}
