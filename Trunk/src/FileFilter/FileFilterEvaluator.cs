using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.FindFiles.IndexInfra;
using Taurus.FindFiles.Utils;

namespace Taurus.FindFiles.FileFilter
{

    /// <summary>
    /// This class takes on the responsibility of evaluating the filters specified in the search criteria
    /// on the file attributes
    /// </summary>
    public class FileFilterEvaluator
    {

        static FileFilterEvaluator()
        {
            if (_filterAttributeDataTypeToFQCN.Count == 0)
            {
                _filterAttributeDataTypeToFQCN.Add(FilterAttributeDataType.String, FilterEvaluatorsFQCN.StringEvaluatorFQCN);
                _filterAttributeDataTypeToFQCN.Add(FilterAttributeDataType.Integer, FilterEvaluatorsFQCN.LongEvaluatorFQCN);
                _filterAttributeDataTypeToFQCN.Add(FilterAttributeDataType.Long, FilterEvaluatorsFQCN.LongEvaluatorFQCN);
            }
            
        }

        #region Constructors
        /// <summary>
        /// Creates an instance of a file filter evaluator
        /// </summary>
        /// <param name="fileFilter">The file filter to be evaluated</param>
        /// <param name="fileAttributes">The attributes of the file against which the filter will be evaluated</param>
        public FileFilterEvaluator(FileMetaAttributeFilter fileFilter, FileMetaAttributes fileAttributes)
        {
            _fileFilter = fileFilter;
            _fileAttributes = fileAttributes;
        }

        #endregion


        #region Public methods

        //currently we will only be ANDing the various filter conditions.It does not make sense to 
        //explicitly OR the filters for a file search based on attributes as this may result in a very unpredeictable
        //resultset.

        public bool EvaluateFilter()
        {
            bool result = true;
            foreach (FilterCondition filterCondition in _fileFilter.FilterConditions)
            {

                if (!filterCondition.IsValid)
                {
                    throw new ApplicationException("Specified filter contains invalid filter conditions");
                }
                ReplaceAttributeNamesInFilterWithValues(filterCondition);

                Type metaAttributeEvaluatorType = Type.GetType(_filterAttributeDataTypeToFQCN[filterCondition.AttributeDataType]);
                object evaluator = Activator.CreateInstance(metaAttributeEvaluatorType, filterCondition.FilterAttributeValueLHS, filterCondition.FilterOperator, filterCondition.FilterAttributeValueRHS);
                if (!(evaluator is IFileMetaAttributeEvaluator))
                    throw new ApplicationException("Error while instantiating a filter attribute evaluator type");
                
                //OK... we have a valid evaluator instance, and hence the filter condition can be evaluated
                IFileMetaAttributeEvaluator metaAttributeEvaluator = (IFileMetaAttributeEvaluator)(evaluator);
                result = (result && metaAttributeEvaluator.Evaluate());
            }
            return result;
        }

        #endregion


        #region Private methods

        private void ReplaceAttributeNamesInFilterWithValues(FilterCondition condition)
        {

            if(0 == string.Compare(condition.FilterAttributeValueLHS, FileMetaAttributesStringConstants.FileMetaAttributeParentDirectory, true))
                condition.FilterAttributeValueLHS = _fileAttributes.ParentDirectory;
            if(0 == string.Compare(condition.FilterAttributeValueLHS,FileMetaAttributesStringConstants.FileMetaAttributeCreationTime,true))
                condition.FilterAttributeValueLHS = _fileAttributes.CreatedDate.ToString();
            if(0 == string.Compare(condition.FilterAttributeValueLHS, FileMetaAttributesStringConstants.FileMetaAttributeModificationTime, true))
                condition.FilterAttributeValueLHS = _fileAttributes.ModfiedDate.ToString();
            if(0 == string.Compare(condition.FilterAttributeValueLHS, FileMetaAttributesStringConstants.FileMetaAttributeFileExtension, true))
                condition.FilterAttributeValueLHS = _fileAttributes.FileExtension;
            if(0 == string.Compare(condition.FilterAttributeValueLHS, FileMetaAttributesStringConstants.FileMetaAttributesFileOwner, true))
                condition.FilterAttributeValueLHS = _fileAttributes.FileOwner;
            if (0 == string.Compare(condition.FilterAttributeValueLHS, FileMetaAttributesStringConstants.FileMetaAttributeFileSize, true))
                condition.FilterAttributeValueLHS = _fileAttributes.FileSize.ToString();

        }

        #endregion

        #region Private members

        private FileMetaAttributeFilter _fileFilter;
        private FileMetaAttributes _fileAttributes;


        #endregion


        #region Static private members
        private static Dictionary<FilterAttributeDataType, string> _filterAttributeDataTypeToFQCN = new Dictionary<FilterAttributeDataType, string>();
        #endregion
    }
}
