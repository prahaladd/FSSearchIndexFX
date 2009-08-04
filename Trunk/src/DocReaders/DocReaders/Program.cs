using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taurus.DocReaders.Implementation;
using Taurus.DocReaders;
using Taurus.DocReaders.Interfaces;

namespace DocReaders
{
    class Program
    {
        static void Main(string[] args)
        {
            //WordReader reader = new WordReader(@"C:\Documents and Settings\prahaladd\My Documents\MSEB_PillPayment_EReceipt.docx");

            //reader.Initialize();

            //string documentText = reader.ReadDocument();

            //reader.Close();

            //Console.WriteLine(documentText);

            //ExcelReader excelReader = new ExcelReader(@"C:\Documents and Settings\prahaladd\My Documents\Sample.xlsx");

            //excelReader.Initialize();

            //excelReader.ReadDocument();

            //excelReader.Close();

            //PDFReader reader = new PDFReader(@"C:\Documents and Settings\prahaladd\My Documents\itr2.pdf");

            //reader.Initialize();

            //string pdfDocumentText = reader.ReadDocument();

            //Console.WriteLine(pdfDocumentText);

            IDocReader docReader = DocReaderFactory.Make(@"C:\Documents and Settings\prahaladd\My Documents\itr2.pdf");

            docReader.Initialize();

            Console.WriteLine(docReader.ReadDocument());

            docReader.Close();



        }
    }
}
