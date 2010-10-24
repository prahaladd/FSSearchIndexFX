using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taurus.FindFiles.FileFilter
{
    internal class LongMetaAttributeEvaluator : IFileMetaAttributeEvaluator
    {

        #region Constructors

        public LongMetaAttributeEvaluator(string lhsValue, FileFilterOperator filterOperator, string rhsValue)
        {
            //we can directly use the Int64.Parse methods here as the filter conditions are validated prior to
            //their evaluations. Hence malformed filter conditions will never be present
            _lhsValue = Int64.Parse(lhsValue);
            _filterOperator = filterOperator;
            _rhsValue = Int64.Parse(rhsValue); ;

        }

        #endregion

        #region IFileMetaAttributeEvaluator Members

        public bool Evaluate()
        {
            bool evaluationResult = false;

            if (FileFilterOperator.Equals != _filterOperator && FileFilterOperator.NotEquals != _filterOperator &&
               FileFilterOperator.GreaterThan != _filterOperator && FileFilterOperator.LessThan != _filterOperator)
                throw new ArgumentException("Long value filters support only equality, greater than and less than operators");

            switch (_filterOperator)
            {
                case FileFilterOperator.Equals:

                    evaluationResult = _lhsValue == _rhsValue;
                    break;

                case FileFilterOperator.NotEquals:

                    evaluationResult = _lhsValue != _rhsValue;
                    break;

                case FileFilterOperator.GreaterThan:

                    evaluationResult = _lhsValue > _rhsValue;
                    break;

                case FileFilterOperator.LessThan:

                    evaluationResult = _lhsValue < _rhsValue;
                    break;

                case FileFilterOperator.GreaterThanEquals:

                    evaluationResult = _lhsValue >= _rhsValue;
                    break;

                case FileFilterOperator.LessThanEquals:

                    evaluationResult = _lhsValue <= _rhsValue;
                    break;
            }
            return evaluationResult;
        }

        #endregion

        #region Private members

        private long _lhsValue;

        private FileFilterOperator _filterOperator;

        private long _rhsValue;

        #endregion
    }
}
