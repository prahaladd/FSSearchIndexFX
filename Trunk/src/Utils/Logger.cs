using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Taurus.FindFiles.Utils
{
    public class Logger : IDisposable
    {
        #region Constructors
        public  Logger()
        {

            string appFolderPath = Environment.GetEnvironmentVariable("appdata");

            _logDirectory = string.Format("{0}\\KMPSearcherLogs",appFolderPath);

            _fileName = string.Format("{0}\\KMPSearcher.log{1}", _logDirectory, System.DateTime.Now.Date.Ticks);
        }
        #endregion

        #region Public methods
        public  void WriteToLogFile(string message)
        {
            try
            {
                

                LogFileWriter.WriteLine(message);
                LogFileWriter.Flush();
            }
            catch (Exception ex)
            {
                //gulp the exception as no logging exception should halt processing

            }

        }
        #endregion

        #region Public properties

        public string LogFileName
        {
            get
            {
                return _fileName;
            }
        }

        #endregion

        #region Private properties

        private  TextWriter LogFileWriter
        {

            get
            {
                if (null == _logFileWriter)
                {
                    try
                    {
                        if (!Directory.Exists(_logDirectory))
                        {
                            Directory.CreateDirectory(_logDirectory);
                        }

                        _logFileWriter = new StreamWriter(File.Open(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read));
                    }
                    catch (IOException iex)
                    {
                        // we ignore the iex here as we dont want to halt search simply
                        //because we cannot open a log file for writing.
                    }

                }

                return _logFileWriter;
            }


        }


        #endregion

        #region Private members

        private string _fileName;

        private TextWriter _logFileWriter;

        private string _logDirectory;

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposing)
        {

            if (!isDisposing)
            {
                try
                {
                    _logFileWriter.Flush();
                    _logFileWriter.Close();
                }
                catch (Exception ex)
                {
                    //gulp the exception 
                }
            }

        }

        #endregion
    }
}
