using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Taurus.FindFiles.Utils
{
    internal class Utilities
    {

        public static string ReadTextFiles(string fileName)
        {
            string fileText = string.Empty;

            using (TextReader textReader = new StreamReader(File.Open(fileName, FileMode.Open, FileAccess.Read)))
            {

                fileText = textReader.ReadToEnd();

                textReader.Close();
            }
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
            if (fileName.LastIndexOf(".dll", StringComparison.OrdinalIgnoreCase) != -1 ||
                fileName.LastIndexOf(".exe", StringComparison.OrdinalIgnoreCase) != -1 ||
                fileName.LastIndexOf(".sys", StringComparison.OrdinalIgnoreCase) != -1||
                fileName.LastIndexOf(".dat", StringComparison.OrdinalIgnoreCase) != -1 ||
                fileName.LastIndexOf(".com", StringComparison.OrdinalIgnoreCase) != -1 ||
                fileName.LastIndexOf(".") == -1)
                return false;

            return true;
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


    }
}
