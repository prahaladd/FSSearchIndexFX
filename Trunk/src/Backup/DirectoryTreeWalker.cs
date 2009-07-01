using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Taurus.FindFiles
{
    internal class DirectoryTreeWalker
    {

        #region Constructors

        public DirectoryTreeWalker()
        {
            
        }

        /// <summary>
        /// This method walks the directory tree in the depth first fashion.
        /// </summary>
        public void WalkTheTree(string rootPath)
        {
           //assert that the specified root is a directory. In case it is not throw up an argument exception

            if (!Directory.Exists(rootPath))
            {
                throw new ArgumentException("Invalid directory specified as root path");
            }

            
            //enumerate all files in the current directory first

            EnumerateFiles(rootPath);

            //loop through the sub directories only if needed. Hence we enumerate the sub directories accordingly.
            
            if (SearchSubDirectories)
            {
                List<string> subDirectories = new List<string>();

                string[] directoryNames = Directory.GetDirectories(rootPath);

                if (directoryNames.Length == 0)
                    return;

                subDirectories.AddRange(directoryNames);

                foreach (string subDirectory in subDirectories)
                {
                    
                    WalkTheTree(subDirectory);

                }
            }
            return;
        }


        #endregion


        #region Public properties

        public List<string> Files
        {
            get { return _fileObjects; }

            set { _fileObjects = value; }
        }

        public bool SearchSubDirectories
        {
            get { return _searchSubDirectories; }
            set { _searchSubDirectories = value; }
        }


        #endregion

        #region Private methods

        public void EnumerateFiles(string fullyQualifiedDirectoryName)
        {

            _fileObjects.AddRange(Directory.GetFiles(fullyQualifiedDirectoryName));

        }

        #endregion
        #region Private members

        private List<string> _fileObjects = new List<string>();

        private bool _searchSubDirectories = false;


        #endregion
    }
}
