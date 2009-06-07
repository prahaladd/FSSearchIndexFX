using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Taurus.FindFiles
{
    public class FileContentFinder
    {


        public FileContentFinder(string rootPath, string pattern)
        {
            _searchRootPath = rootPath;
            _searchPattern = pattern;
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
                fileContent = ReadTextFile(file);

                KMPEngine kmpEngine = new KMPEngine(fileContent, SearchPattern);

                List<int> occurences = kmpEngine.Find(false, true);

                if (occurences.Count == 0)
                    continue;

                if (occurences[0] != -1)
                {
                    _fileNamesWithContentPresent.Add(file);
                }
            }
        }

        #region Public properties

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

        #endregion
    }
}
