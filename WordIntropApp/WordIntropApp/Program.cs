using Microsoft.Office.Interop.Word;
using System;

namespace WordIntropApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHeaderAndFooter();
            Console.WriteLine($"Test Word Introp Object:{SetWordObject()}");
            Console.ReadLine();
        }
        private static void GetHeaderAndFooter() {
            string str = "";
            var sourceFile = "C:\\Dev\\HidenText.docx";

            Application oWord = new Application();
            oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            oWord.Visible = false;
            Document oWordDoc = oWord.Documents.Add(sourceFile);
            //Get storyRanges from document for header and footer properties
            StoryRanges storyRanges = oWordDoc.StoryRanges;

            foreach (Range storyRange in storyRanges)
            {
                ContentControls storyRangeControls = storyRange.ContentControls;

                if (storyRangeControls != null)
                {
                    if (storyRangeControls.Count > 0)
                    {
                        foreach (ContentControl control in storyRangeControls)
                        {
                            var x = control.Tag;
                        }
                    }
                }
            }
            //foreach (Section Section in oWordDoc.Sections)
            //{


            //    foreach (Microsoft.Office.Interop.Word.HeaderFooter header in Section.Headers)

            //        if (header.Exists)
            //        {
            //            header.Range.Words.First.Select();

            //            str += header.Range.Text + System.Environment.NewLine;



            //        }

            //    foreach (HeaderFooter Footer in Section.Footers)

            //        if (Footer.Exists)
            //        {
            //            Footer.Range.Words.First.Select();

            //            str += Footer.Range.Text + System.Environment.NewLine;

            //        }
            //}

            Console.WriteLine(str);
        }
        private static string SetWordObject()
        {
            //var sourceFile = "C:\\Dev\\Repo\\DotNetTestProjectRepo\\WordIntropApp\\WordIntropApp\\Documents\\HidenText.docx";
            var sourceFile = "C:\\Dev\\Source\\DotNetTestProjectRepo\\WordIntropApp\\WordIntropApp\\Documents\\HidenText.docx";

            Application oWord = new Application();
            oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            oWord.Visible = false;
            Document oWordDoc = oWord.Documents.Add(sourceFile);

            oWord.Options.AutoFormatAsYouTypeApplyNumberedLists = true;
            oWord.Options.AutoFormatApplyLists = true;
            
            oWordDoc.Range().TextRetrievalMode.IncludeHiddenText = true;


            //Find range by text
            bool wasFound = false;
            object missing = Type.Missing;
            Range rngFind = oWordDoc.Content;
            object oEnd = WdCollapseDirection.wdCollapseEnd;
            wasFound = rngFind.Find.Execute("Just Test Text!", ref missing, ref missing,
                       ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                       ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);


            foreach (Paragraph paragraph in oWordDoc.Paragraphs)
            {
                var prg = paragraph;
                if (prg.Range.Find.Execute("Lorem Ipsum Hide Me"))
                {
                    var prevRange = oWordDoc.Range(rngFind.Start, rngFind.End).Previous();
                    var nextRange = oWordDoc.Range(rngFind.Start, rngFind.End).Next();
                    paragraph.Range.InsertBefore("Hello New Line ############ 02 ############");
                    paragraph.Range.ListFormat.ApplyNumberDefault();
                    //nextRange.InsertAfter("Hello New Line");
                    //oWordDoc.Save();

                }
            }
            

            var controls = oWordDoc.Content.ContentControls;
            int nr;
            foreach (ContentControl item in controls)
            {
                
                //Console.WriteLine(item.PlaceholderText.Value);
                Console.WriteLine(item.Tag);
                if (item.Tag!=null && item.Tag.Equals("HideMe"))
                {
                    //Get range Start-End then insert before Or after
                    var rng=item.Range;
                    nr = item.Range.ListFormat.ListValue;
                    item.Range.ListFormat.RemoveNumbers();
                    item.Range.Font.Hidden = 1;

                    //item.Range.ListFormat.ApplyNumberDefault(nr);

                }

                item.Range.InsertAfter("##################### Add text to the End Of Paragraph");

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
            
            oWordDoc.SaveAs2("C:\\Users\\seaan0\\Desktop\\OutPut.docx");

            return oWordDoc.FullName;
        }
    }
}
