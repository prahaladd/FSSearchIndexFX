using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.DocReaders.Implementation;
using Taurus.DocReaders.Interfaces;

namespace Taurus.DocReaders
{
    public class DocReaderFactory
    {

        public static IDocReader Make(string fileName)
        {
            string documentExtension = string.Empty;


            int indexOfDot = fileName.LastIndexOf(".");

            documentExtension = fileName.Substring(indexOfDot + 1);

            if (-1 != documentExtension.IndexOf("doc"))
            {
                return new WordReader(fileName);    
            }

            if (-1 != documentExtension.IndexOf("xls"))
            {
                return new ExcelReader(fileName);
            }

            if (-1 != documentExtension.IndexOf("pdf"))
            {
                return new PDFReader(fileName);
            }

            return new TextFileReader(fileName);
        }


    }
}
