using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taurus.FindFiles.FileFilter
{
    public class FileMetaAttributeFilter
    {

        #region Constructors

        public FileMetaAttributeFilter()
        {
        }

        public FileMetaAttributeFilter(List<FilterCondition> filterConditions)
        {
            _filterConditions = filterConditions;
        }

        #endregion


        #region Public methods

        public void AddFilterCondition(FilterCondition filterCondition)
        {
            if (null == _filterConditions)
                _filterConditions = new List<FilterCondition>();

            _filterConditions.Add(filterCondition);
        }


        #endregion


        #region Public properties

        public List<FilterCondition> FilterConditions
        {
            get { return _filterConditions; }
        }


        #endregion
        
        #region Private members

        private List<FilterCondition> _filterConditions;

        #endregion
    }
}
