/*KMPEngine.cs - The core engine that performs keyword based search. Implements the Knuth Morris Pratt algorithm
 * to perform keyword based search.
*/


using System;
using System.Collections.Generic;
using System.Text;

namespace Taurus.FindFiles
{
    public  class KMPEngine
    {

        #region Constructors

        public KMPEngine(string sourceText, string searchPattern)
        {
            //TODO: perform argument validation

            _sourceText = sourceText;
            _searchPattern = searchPattern;
            _patternIndex = -1;

        }

        #endregion

        #region Public methods

        public List<Int32> Find(bool searchAllOccurences, bool ignoreCase)
        {
            int sourceTextLength = SourceText.Length;
            int patternLength = SearchPattern.Length;

            List<Int32> presenceIndices = new List<Int32>();

            //build the prefix table
            BuildPrefixTable();

            int matchCounter = 0;

            if (ignoreCase)
            {
                _searchPattern = _searchPattern.ToUpper();
                _sourceText = _sourceText.ToUpper();
            }

            for (int index = 0; index < sourceTextLength; index++)
            {
                while (matchCounter > 0 && SearchPattern[matchCounter] != SourceText[index])
                {
                    int tempMatchCounter = PrefixTable[matchCounter];

                    if(tempMatchCounter == matchCounter)
                    {
                        matchCounter = 0;
                    }
                    else
                    {

                        matchCounter = PrefixTable[matchCounter];
                    }
                }

                if (SearchPattern[matchCounter] == SourceText[index])
                {
                    matchCounter++;
                }

                //check if we have covered the entire length of the pattern string
                if (matchCounter == patternLength)
                {
                    presenceIndices.Add(index - matchCounter + 1);

                    if (searchAllOccurences)
                        matchCounter = PrefixTable[matchCounter - 1]; //start the next match
                    else
                        break;  //we need to search for only one occurence and hence we can break out of the loop
                }

            }
            return presenceIndices;
            
        }

        #endregion

        #region Public properties


        public Dictionary<int, int> PrefixTable
        {

            get { return _prefixFunctionTable; }
            set { _prefixFunctionTable = value; }
        }

        public string SearchPattern
        {
            get { return _searchPattern; }
            set { _searchPattern = value; }
        }

        public string SourceText
        {
            get { return _sourceText; }
            set { _sourceText = value; }
        }

        #endregion

        #region Private methods

        private void BuildPrefixTable()
        {
            //initialize the position 0 in the prefix table to 0. This means that there is no backtracking 
            // if there is a mismatch at position 0. you continue matching from the first character.

            int prefixIndex = 0;

            PrefixTable[0] = 0;

            int patternLength = SearchPattern.Length;

            //PrefixTable[prefixIndex] = lenght of the longest  proper suffix of SearchPattern[i] which is also a
            //prefix of SearchPattern(overlap)

            for (int i = 1; i < patternLength; i++)
            {
                while (prefixIndex > 0 && SearchPattern[prefixIndex] != SearchPattern[i])
                {
                    //find the appropriate index in the pattern which ensures that we have the maximum
                    //overlap between SearchPattern[i] and the SearchPattern

                    //Prahaladd -  06/28/09 - Discovered a particular string sequence which if
                    //present in the pattern can result in the current prefix table build logic 
                    //go away in an infinite loop. 
                    //This condition typically occurs when the index I in the pattern used to find the max overlap
                    //equals PrefixTable[I] ==> I = PrefixTable[I] ( I > 0)

                    int tempPrefixIndex = PrefixTable[prefixIndex];

                    if (prefixIndex == tempPrefixIndex)
                    {
                        prefixIndex = 0;
                    }
                    else
                    {
                        prefixIndex = PrefixTable[prefixIndex];
                    }
                }

                //we are currently building on the substring.

                if (SearchPattern[prefixIndex] == SearchPattern[i])
                {
                    prefixIndex = prefixIndex + 1;
                }

                PrefixTable[i] = prefixIndex;
            }



        }



        #endregion

        #region Private members

        private string _sourceText;
        private string _searchPattern;
        private int _patternIndex;

        private Dictionary<int, int> _prefixFunctionTable = new Dictionary<int,int>();
        #endregion
    }
}
