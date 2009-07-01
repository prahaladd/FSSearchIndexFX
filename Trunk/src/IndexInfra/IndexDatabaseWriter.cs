using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Taurus.FindFiles.IndexInfra
{

    /// <summary>
    /// Provides a means to write the Index database directly to a binary file on the disk.
    /// </summary>
    public class IndexDatabaseWriter
    {


        #region Constructors

        public IndexDatabaseWriter() { }

        #endregion

        #region Public methods

        public void Persist(IndexDatabase indexDatabase)
        {

            IFormatter binaryFormatter = new BinaryFormatter();

            string appDataFolderPath = Environment.GetEnvironmentVariable("appdata");

            Directory.CreateDirectory(string.Format("{0}\\{1}",appDataFolderPath,"KMPSearcher"));

            string indexDatabaseFileName = string.Format("{0}\\KMPSearcher\\KMPIDB.db", appDataFolderPath);

            using (Stream idbStream = new FileStream(indexDatabaseFileName, FileMode.Create, FileAccess.Write))
            {
                binaryFormatter.Serialize(idbStream, indexDatabase);
                idbStream.Close();
               
            }
        }
        #endregion

    }
}
