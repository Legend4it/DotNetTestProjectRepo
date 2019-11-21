using Microsoft.Office.Interop.Word;
using System;

namespace WordIntropApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine($"Test Word Introp Object:{SetWordObject()}");
            Console.ReadLine();
        }

        private static string SetWordObject()
        {
            var sourceFile = "C:\\Dev\\HidenText.docx";

            Application oWord = new Application();
            oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            oWord.Visible = false;
            Document oWordDoc = oWord.Documents.Add(sourceFile);

            //oWordDoc.Range().TextRetrievalMode.IncludeHiddenText = false;

            var rangeAll = oWordDoc.Range();
            rangeAll.TextRetrievalMode.IncludeHiddenText = true;


            var controls = oWordDoc.Content.ContentControls;
            foreach (ContentControl item in controls)
            {
                //Console.WriteLine(item.PlaceholderText.Value);
                Console.WriteLine(item.Tag);
                if (item.Range.Font.Hidden !=0)
                {
                    Console.WriteLine("################  I am Hidden ################");
                }
            }
            

            foreach (Range p in rangeAll.Words)
            {

                if (p.Font.Hidden != 0) //Hidden text found
                {
                    // Do something
                    Console.WriteLine("I am Visible ...");
                }
            }



            foreach (Paragraph para in oWordDoc.Paragraphs) // see the change of wordFooter in this line
            {
                Range range = para.Range;
                //Tag position as range start, and next tag position as range end
                range.SetRange(1, 5);
                Console.WriteLine(range.Text);
//                range.Delete();
            }

            return oWordDoc.FullName;
        }
    }
}
