using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Taurus.FindFiles.IndexInfra
{
    /// <summary>
    /// This class represents a single index record in the list of indexes for the search term.
    /// Currently we will limit to using basic serialization.
    /// </summary>
    /// 

    [Serializable]
    public class IndexRecord
    {

        #region Constructor

        public IndexRecord(string fileName, List<Int32> occurencePositions)
        {
            _fileName = fileName;
            _occurencePositions = occurencePositions;
        }

        #endregion

        #region Public properties

        public string FileName
        {
            get { return _fileName; }
        }

        public List<Int32> OccurencePositions
        {
            get { return _occurencePositions; }
        }

        #endregion



        #region Private members

        private string _fileName;

        

        private List<Int32> _occurencePositions = new List<Int32>();

        #endregion
    }
}
