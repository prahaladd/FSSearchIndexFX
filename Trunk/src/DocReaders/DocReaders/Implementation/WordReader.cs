using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Taurus.DocReaders.Interfaces;

namespace Taurus.DocReaders.Implementation
{
    class WordReader : IDocReader
    {

        #region Constructors

        internal WordReader(string wordDocumentName)
        {
            _wordDocumentName = wordDocumentName;
        }

        #endregion

        #region IDocReader members

        public void Initialize()
        {
            _wordApplication = new ApplicationClass();

            object wordDocumentName = _wordDocumentName;
            object falseObject = false;
            object trueObject = true;

            object missingObject = System.Reflection.Missing.Value;
            object emptyData = string.Empty;

            try
            {
                _wordDocument = _wordApplication.Documents.Open(ref wordDocumentName,
                    ref falseObject, ref trueObject, ref falseObject, ref missingObject, ref missingObject, ref missingObject,
                    ref missingObject, ref missingObject, ref missingObject, ref missingObject, ref falseObject, ref missingObject,
                    ref missingObject, ref missingObject, ref missingObject);

                

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }

        public string ReadDocument()
        {
            return RemoveGarbleChars( _wordDocument.Content.Text);

        }

        public void Close()
        {
            object nullObject =  System.Reflection.Missing.Value;
            _wordDocument.Close(ref nullObject, ref nullObject, ref nullObject);
        }

        #endregion

        #region Private methods

        private string RemoveGarbleChars(string documentText)
        {
            StringBuilder clearText = new StringBuilder();
             
            for (int i = 0; i < documentText.Length; i++)
            {
                System.Globalization.UnicodeCategory unicodeCategory =  Char.GetUnicodeCategory(documentText[i]);


                //skip punctuation characters
                if (unicodeCategory == System.Globalization.UnicodeCategory.OtherPunctuation || unicodeCategory == System.Globalization.UnicodeCategory.OpenPunctuation
                    || unicodeCategory == System.Globalization.UnicodeCategory.ClosePunctuation)
                    continue;

                //replace any control characters with the whitespace character

                if (unicodeCategory == System.Globalization.UnicodeCategory.Format || unicodeCategory == System.Globalization.UnicodeCategory.Control)
                {
                    clearText.Append(' ');
                    continue;
                }

                clearText.Append(documentText[i]);
            }

            return clearText.ToString().Trim();
        }

        #endregion

        #region Private members

        string _wordDocumentName; 

        Application _wordApplication;

        Document _wordDocument;

        
        #endregion

    }
}
