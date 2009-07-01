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
    /// Provides a means to read the serialized IndexDatabase
    /// </summary>
    public class IndexDatabaseReader
    {

        #region Constructor

        public IndexDatabaseReader() { }

        #endregion


        #region Public methods

        public IndexDatabase Read()
        {
            IFormatter binaryFormatter = new BinaryFormatter();

            string appDataFolderPath = Environment.GetEnvironmentVariable("appdata");

            string indexDatabaseFileName = string.Format("{0}\\KMPSearcher\\KMPIDB.db",appDataFolderPath);

            Stream indexDatabaseStream = File.Open(indexDatabaseFileName, FileMode.Open, FileAccess.Read);

            IndexDatabase database = (IndexDatabase)(binaryFormatter.Deserialize(indexDatabaseStream));

            indexDatabaseStream.Close();

            return database;
        }

        #endregion

    }
}
