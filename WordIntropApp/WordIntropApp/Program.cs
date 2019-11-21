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
            var sourceFile = "C:\\Dev\\Source\\DotNetTestProjectRepo\\WordIntropApp\\WordIntropApp\\Documents\\HidenText.docx";

            Application oWord = new Application();
            oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            oWord.Visible = false;
            Document oWordDoc = oWord.Documents.Add(sourceFile);

            oWord.Options.AutoFormatAsYouTypeApplyNumberedLists = true;
            oWord.Options.AutoFormatApplyLists = true;
            
            oWordDoc.Range().TextRetrievalMode.IncludeHiddenText = true;




            var controls = oWordDoc.Content.ContentControls;
            int nr;
            foreach (ContentControl item in controls)
            {
                //Console.WriteLine(item.PlaceholderText.Value);
                Console.WriteLine(item.Tag);
                if (item.Tag!=null && item.Tag.Equals("HideMe"))
                {
                    nr = item.Range.ListFormat.ListValue;
                    //item.Range.ListFormat.RemoveNumbers();
                    item.Range.Font.Hidden = 1;

                    item.Range.Text = "Lorem Ipsum Hide Me Is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.";
                    item.Range.ListFormat.ApplyNumberDefault(nr);

                }

                if (item.Tag!=null && item.Tag.Equals("ShowMe"))
                {
                    item.Range.Font.Hidden = 0;
                    //item.Range.ListFormat.ApplyNumberDefault();
                }

                if (item.Tag!=null && item.Tag.Equals("HiddenTag"))
                {
                    item.Range.Font.Hidden = 0;
                }
                
                if (item.Range.Font.Hidden !=0)
                {
                    Console.WriteLine("################  I am Hidden ################");
                }
            }
            
            oWordDoc.Save();

            return oWordDoc.FullName;
        }
    }
}
