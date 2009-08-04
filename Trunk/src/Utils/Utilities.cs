using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Taurus.DocReaders;
using Taurus.DocReaders.Interfaces;

namespace Taurus.FindFiles.Utils
{
    internal class Utilities
    {

        static Utilities()
        {
            _fileExtensionsToExclude.Add("dll");
            _fileExtensionsToExclude.Add("exe");
            _fileExtensionsToExclude.Add("sys");
            _fileExtensionsToExclude.Add("com");
            _fileExtensionsToExclude.Add("pdb");
            _fileExtensionsToExclude.Add("dat");
            _fileExtensionsToExclude.Add("bin");
            _fileExtensionsToExclude.Add("class");
            _fileExtensionsToExclude.Add("jar");


        }

        public static string ReadTextFromFiles(string fileName)
        {
            string fileText = string.Empty;

            IDocReader reader = DocReaderFactory.Make(fileName);

            reader.Initialize();

            fileText = reader.ReadDocument();

            reader.Close();

            return fileText;
        }

        public static List<string> ExtractWordsInText(string text,List<string> wordsToFilter)
        {

            List<string> wordsInFile = new List<string>();

            string[] linesInFile = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in linesInFile)
            {
                wordsInFile.AddRange(line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
            }

            return MakeUniqueWordList(wordsInFile.ToArray());
        }

        public static bool IsTextFile(string fileName)
        {
            //if (fileName.LastIndexOf(".dll", StringComparison.OrdinalIgnoreCase) != -1 ||
            //    fileName.LastIndexOf(".exe", StringComparison.OrdinalIgnoreCase) != -1 ||
            //    fileName.LastIndexOf(".sys", StringComparison.OrdinalIgnoreCase) != -1||
            //    fileName.LastIndexOf(".dat", StringComparison.OrdinalIgnoreCase) != -1 ||
            //    fileName.LastIndexOf(".com", StringComparison.OrdinalIgnoreCase) != -1 ||
            //    fileName.LastIndexOf(".") == -1)
            //    return false;

            //return true;

            string fileExtension = fileName.Substring(fileName.LastIndexOf(".") + 1);

            return (!IsExtensionExcluded(fileExtension));
           
        }

        //this function ensures that each word in the file is counted EXACTLY once. Hence 
        //any occurences of a word other than the first one are pruned off.
        //This extra processing is beneficial in the later stages of indexing as:
        //1.) It saves time to index data.
        //2.)Prevents bloating of the index database due to repeated occurences of the word.

        private static List<string> MakeUniqueWordList(string[] wordList)
        {
            Dictionary<string, bool> wordToPresenceMap = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            foreach (string word in wordList)
            {
                if (!wordToPresenceMap.ContainsKey(word))
                {
                    wordToPresenceMap.Add(word, true);
                }
            }

            //now loop trhough all the keys and then return back the list of words
            List<string> uniqueWordList = new List<string>();

            foreach (string uniqueWord in wordToPresenceMap.Keys)
            {
                uniqueWordList.Add(uniqueWord);
            }

            //clear of the dictionary
            wordToPresenceMap.Clear();
            return uniqueWordList;
        }


        private static bool IsExtensionExcluded(string inputExtension)
        {
            bool shouldExclude = (_fileExtensionsToExclude.Exists(delegate(string extension) 
                                                    { return ( 0 == string.Compare(extension, inputExtension, 
                                                        StringComparison.OrdinalIgnoreCase)); })) ? true : false;
            return shouldExclude;
        }

        #region Private static members

        private static List<string> _fileExtensionsToExclude = new List<string>();

        #endregion

    }
}
