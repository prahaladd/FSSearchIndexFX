using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.DocReaders.Interfaces;
using org.pdfbox.pdmodel;
using org.pdfbox.util;

namespace Taurus.DocReaders.Implementation
{
    public class PDFReader : IDocReader
    {

        #region Constructors

        internal PDFReader(string pdfDocumentName)
        {

            _pdfDocumentName = pdfDocumentName;
        }

        #endregion


        #region IDocReader Members

        public void Initialize()
        {
            _pdfDocument = PDDocument.load(_pdfDocumentName);

        }

        public string ReadDocument()
        {
            PDFTextStripper pdfTextStripper = new PDFTextStripper();
            return pdfTextStripper.getText(_pdfDocument);
        }

        public void Close()
        {
            _pdfDocument.close();
        }

        #endregion


        #region Private members

        private string _pdfDocumentName;
        PDDocument _pdfDocument;

        #endregion


        
    }
}
