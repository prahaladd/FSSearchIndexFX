using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Taurus.FindFiles.FileFilter
{
    internal class StringMetaAttributeEvaluator : IFileMetaAttributeEvaluator
    {


        #region Constructors

        public StringMetaAttributeEvaluator(string lhsValue, FileFilterOperator filterOperator, string rhsValue)
        {
            _lhsValue = lhsValue;
            _filterOperator = filterOperator;
            _rhsValue = rhsValue;
        }



        #endregion


        #region IFileMetaAttributeEvaluator Members

        public bool Evaluate()
        {
            bool evaluationResult = false;

            if (string.IsNullOrEmpty(_lhsValue) || string.IsNullOrEmpty(_rhsValue))
                throw new ArgumentException("The LHS and RHS value for the filter operator cannot be null or empty");

            if(FileFilterOperator.Equals != _filterOperator && FileFilterOperator.Contains != _filterOperator && FileFilterOperator.Like !=_filterOperator)
                throw new ArgumentException("String filters support only Equality, Contains and Like comparisons");

            switch (_filterOperator)
            {
                case FileFilterOperator.Equals:

                    evaluationResult = (0 == string.Compare(_lhsValue,_rhsValue,true));
                    break;

                case FileFilterOperator.NotEquals:

                    evaluationResult = ( 0 != string.Compare(_lhsValue,_rhsValue,true));
                    break;

                case FileFilterOperator.Contains:

                    evaluationResult = _lhsValue.Contains(_rhsValue);
                    break;

                case FileFilterOperator.Like:

                    string pattern = string.Format("^{0}(.)*$",_lhsValue);
                    Match m = Regex.Match(_rhsValue,pattern);
                    evaluationResult = m.Success;
                    break;

            }
            return evaluationResult;
        }

        #endregion

        #region Private members

        private string _lhsValue;

        private FileFilterOperator _filterOperator;

        private string _rhsValue;

        #endregion
    }
}
