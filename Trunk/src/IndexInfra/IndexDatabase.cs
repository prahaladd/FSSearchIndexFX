using System;
using System.Collections.Generic;
using System.Text;

namespace Taurus.FindFiles.IndexInfra
{
    /// <summary>
    /// Represents a collection of IndexRecord entries.
    /// Currently we will limit to using basic serialiazation.
    /// </summary>

    [Serializable]
    public class IndexDatabase
    {

        #region Constructors

        public IndexDatabase(Dictionary<string, List<IndexRecord>> searchPatternToFileNamePostionMap, DateTime dateGenerated)
        {
            _searchPatternToFileNamePositionMap = searchPatternToFileNamePostionMap;

            _dateGenerated = dateGenerated;

        }
        #endregion

        #region Public properties

        public Dictionary<string,List<IndexRecord>> IndexRecords
        {
            get { return _searchPatternToFileNamePositionMap; }
        }

        public DateTime IndexGeneratedDate
        {
            get { return _dateGenerated; }
        }

        #endregion


        #region Private members

        private Dictionary<string, List<IndexRecord>> _searchPatternToFileNamePositionMap = new Dictionary<string, List<IndexRecord>>();

        private DateTime _dateGenerated;


        #endregion


    }
}
