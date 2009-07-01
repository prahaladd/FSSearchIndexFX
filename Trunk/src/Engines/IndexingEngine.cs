﻿using System;
using System.Collections.Generic;
using System.Text;
using Taurus.FindFiles.Utils;
using Taurus.FindFiles.IndexInfra;
using System.IO;

namespace Taurus.FindFiles.Engines
{
    public class IndexingEngine
    {
        #region Constructor
        
        public IndexingEngine() { }

        #endregion

        #region Public methods

        public void Index()
       { 
           //get the number of logical drives on the system.

            string[] logicalDrives = System.Environment.GetLogicalDrives();

            //each logical drive now forms the parent path.We will enumerate all files that
            //will contain text. We skip the files of the type EXE,DLL or SYS as these
            //are operating system specific binary files and wont be of much value in the 
            //content search.

            foreach (string logicalDrive in logicalDrives)
            {

                _directoryTreeWalker = new DirectoryTreeWalker();
                _directoryTreeWalker.EnumerateFiles(logicalDrive);
                _directoryTreeWalker.SearchSubDirectories = true;

                List<string> files = _directoryTreeWalker.Files;

                IndexInternal(files);



            }
            _indexDatabase = new IndexDatabase(_indexDatabaseEntries, DateTime.UtcNow);
            //persist the entire database to the disk
            IndexDatabaseWriter idbWriter = new IndexDatabaseWriter();
            idbWriter.Persist(_indexDatabase);
        }

        #endregion


        #region Private methods

        private void IndexInternal(List<string> files)
        {
            files.Sort(StringComparer.OrdinalIgnoreCase);



            if (File.Exists(@"C:\searchlog.txt"))
            {
                File.Delete(@"C:\searchLog.txt");
            }

            TextWriter logWriter = new StreamWriter(File.Open(@"C:\searchLog.txt", FileMode.Append | FileMode.OpenOrCreate, FileAccess.Write));


            try
            {
                string fileText = string.Empty;


                for (int i = 0; i < files.Count; i++)
                {

                    if (!Utilities.IsTextFile(files[i]))
                        continue;

                    //read the text in the text file
                    fileText = Utilities.ReadTextFiles(files[i]);

                    //we are not going to filter out any words as of the current release
                    //this may result in large search times, but we are OK with it for now.

                    List<string> wordsInFile = Utilities.ExtractWordsInText(fileText, null);

                    //send the words in the file to the file search engine as a pattern.
                    //since we need to identify all the possible positions of the current word
                    //we directly invoke the KMPEngine functionality on the current file.
                    foreach (string word in wordsInFile)
                    {

                        

                        if (0 == string.Compare(" ", word) || 0 == string.Compare("\r", word)
                            || 0 == string.Compare("\n", word) || 0 == string.Compare("\t", word)
                            || 0 == string.Compare(Environment.NewLine, word))
                            continue;
                        KMPEngine kmpEngine = new KMPEngine(fileText, word);

                        List<int> occurences = kmpEngine.Find(true, true);

                        //since the word is already present in the file, we make an entry
                        //we are sure that there is ATLEAST ONE occurence.We go ahead and create
                        //index record. we add the record only if the number of occurences is 
                        //greater than zero
                        if (occurences.Count == 0)
                            continue;

                        IndexRecord indexRecord = new IndexRecord(files[i], occurences);

                        if (!_indexDatabaseEntries.ContainsKey(word))
                        {
                            List<IndexRecord> indexRecords = new List<IndexRecord>();

                            indexRecords.Add(indexRecord);
                            _indexDatabaseEntries.Add(word, indexRecords);
                            logWriter.WriteLine("Created  first Index record for file {0} containing word {1}", files[i], word);

                        }
                        else
                        {
                            _indexDatabaseEntries[word].Add(indexRecord);
                            logWriter.WriteLine("Appended Index record for file {0} containing word {1}", files[i], word);
                        }
                        logWriter.Flush();

                    }

                }

               logWriter.WriteLine("Created index database with {0} index entries", _indexDatabaseEntries.Count);
                logWriter.Flush();
               
                
            }
            catch (Exception ex)
            {
                logWriter.WriteLine("Exception ex:{0}", ex.ToString());
                logWriter.Flush();
                
            }
            finally
            {
                logWriter.Flush();
                logWriter.Close();
            }
        }
        
        
        

        #endregion



        #region Private members

        private string _searchRootPath;

        private DirectoryTreeWalker _directoryTreeWalker = new DirectoryTreeWalker();

        private FileContentFinder _fileContentFinder;

        private IndexDatabase _indexDatabase;

        private Dictionary<string, List<IndexRecord>> _indexDatabaseEntries = new Dictionary<string, List<IndexRecord>>();
        

        #endregion
    }
}
