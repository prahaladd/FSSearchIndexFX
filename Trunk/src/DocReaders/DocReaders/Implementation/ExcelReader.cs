using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Excel.Core;
using ICSharpCode.SharpZipLib;

using Taurus.DocReaders.Interfaces;
using Excel;
using System.IO;
using System.Data;


namespace Taurus.DocReaders.Implementation
{
    class ExcelReader : IDocReader
    {

        #region Constructors

        internal ExcelReader(string workBookName)
        {
            _excelDocumentName = workBookName;
        }


        #endregion

        #region IDocReader Members

        public void Initialize()
        {

            try
            {

                FileStream excelWorkBookStream = File.Open(_excelDocumentName, FileMode.Open, FileAccess.Read);

                int lastIndexOfExtension = _excelDocumentName.LastIndexOf(".");

                string workBookExtension = _excelDocumentName.Substring(lastIndexOfExtension + 1);

                //the mode in which the workbook needs to be opened is dependent on the version of MS Excel that
                //created the workbook. Excel 97-2003 write the excel in binary format while Excel 2007 writes
                //out in the OpenXML format.
                if (0 == string.Compare(workBookExtension, "xls", StringComparison.InvariantCultureIgnoreCase))
                    _excelDataReader = Factory.CreateReader(excelWorkBookStream, ExcelFileType.Binary);
                else if (0 == string.Compare(workBookExtension, "xlsx", StringComparison.InvariantCultureIgnoreCase))
                    _excelDataReader = Factory.CreateReader(excelWorkBookStream, ExcelFileType.OpenXml);
                else
                    throw new ApplicationException("Invalid or unsupported workbook format");

            }
            catch (Exception ex)
            {
                Close();
                throw new ApplicationException(ex.Message);
            }
            
        }

        public string ReadDocument()
        {
            StringBuilder workBookContents = new StringBuilder();

            //obtain the contents of the workgroup as a DataSet
            DataSet workBookAsDataSet = _excelDataReader.AsDataSet();

            //loop through all the tables in the data set. Each table corresponds to a worksheet and 
            //the table contains only those rows/column that have data in them.

            foreach (DataTable workSheet in workBookAsDataSet.Tables)
            {
                foreach (DataRow row in workSheet.Rows)
                {
                    foreach (DataColumn column in workSheet.Columns)
                    {
                        if (null != row[column])
                        {
                            if (workBookContents.Length > 0)
                                workBookContents.Append(" ");

                            workBookContents.Append(row[column].ToString());

                            
                        }

                    }

                }


            }
            

            return workBookContents.ToString();
        }

        public void Close()
        {
            _excelDataReader.Close();
        }

        #endregion

        
        #region Private members

        private string _excelDocumentName;

        IExcelDataReader _excelDataReader;

        #endregion
    }
}
