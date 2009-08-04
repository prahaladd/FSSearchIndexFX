using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.DocReaders.Interfaces;
using System.IO;

namespace Taurus.DocReaders.Implementation
{
    class TextFileReader : IDocReader
    {

        #region Constructors

        internal TextFileReader(string textFileName)
        {
            _textFileName = textFileName;

        }

        #endregion

        #region IDocReader Members

        public void Initialize()
        {
            try
            {
                _textReader = new StreamReader(File.Open(_textFileName, FileMode.Open, FileAccess.Read));

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to open file");
            }
        }

        public string ReadDocument()
        {
            return _textReader.ReadToEnd();
        }

        public void Close()
        {
            _textReader.Close();
        }

        #endregion

        #region Private members

        private string _textFileName;

        private TextReader _textReader;

        #endregion
    }
}
