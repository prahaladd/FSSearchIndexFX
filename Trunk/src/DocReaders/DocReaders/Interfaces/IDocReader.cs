using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taurus.DocReaders.Interfaces
{
    public interface IDocReader
    {

        /// <summary>
        /// This method initializes the resources used by the reader to read the document
        /// </summary>
        void Initialize();

        /// <summary>
        /// This method returns the content of the document delimited by the tab char.
        /// </summary>
        /// <returns>The content of the document delimited by the tab character</returns>
       string ReadDocument();

        /// <summary>
        /// This method contains all the code for deallocation and cleanup of resources used by the document reader.
        /// </summary>
       void Close();


    }
}
