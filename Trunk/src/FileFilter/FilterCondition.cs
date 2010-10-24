using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taurus.FindFiles.FileFilter
{

    public enum FilterAttributeDataType
    {
        String = 0,
        Integer = 1,
        Long = 2

    }
    public class FilterCondition
    {

        #region Constructor

        public FilterCondition(string filterAttributeValueLHS, FileFilterOperator filterOperator, string filterAttributeValueRHS, FilterAttributeDataType filterAttibuteDataType)
        {
            _filterAttributeValueLHS = filterAttributeValueLHS;
            _filterOperator = filterOperator;
            _filterAttributeValueRHS = filterAttributeValueRHS;
            _filterAttributeDataType = filterAttibuteDataType;
            _isValid = ValidateFilter();
           
        }



        

        #endregion


        #region Internal properties

        public string FilterAttributeValueLHS
        {
            get { return _filterAttributeValueLHS; }
            set { _filterAttributeValueLHS = value; }
        }

        public string FilterAttributeValueRHS
        {
            get { return _filterAttributeValueRHS; }
            set { _filterAttributeValueRHS = value; }


        }

        public FileFilterOperator FilterOperator
        {
            get { return _filterOperator; }
        }

        public FilterAttributeDataType AttributeDataType
        {
            get { return _filterAttributeDataType; }
        }

        public bool IsValid
        {

            get { return _isValid; }
        }
        #endregion

        #region Private methods

        private bool ValidateFilter()
        {
            //for this release we will only support the following filter data types:
            //String, Integer and Long.
            //Since an Integer can anyways be cast to a long, we can currently live with a single attribute evaluator:
            //LongMetaAttributeEvaluator.
            bool isValid = true;
            long attributeValueTest = 0L;

            if (_filterAttributeDataType != FilterAttributeDataType.String)
            {
                isValid = Int64.TryParse(_filterAttributeValueRHS, out attributeValueTest);


            }
            return isValid;
        }

        #endregion

        #region Private data members

        private string _filterAttributeValueLHS;
        
        private FileFilterOperator _filterOperator;

        private string _filterAttributeValueRHS;

        private FilterAttributeDataType _filterAttributeDataType;

        private bool _isValid;

        #endregion


    }
}
