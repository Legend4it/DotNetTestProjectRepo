using System.Windows.Forms;
using System.IO;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace WordDocumentGenerator
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void cmdCreateNew_Click(object sender, EventArgs e)
        {
            var sourceFileTemplate = txtFile.Text;
            if (txtFile.Text.Trim() == "")
            {
                MessageBox.Show(@"Please select DOCX file", "DOCX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var desFile = @"C:\Dev\DestinationFile.docx";
            File.Copy(sourceFileTemplate, desFile, true);
            using (WordprocessingDocument destinationDocument = WordprocessingDocument.Open(desFile, true))
            {
                // Change the document type to Document
                destinationDocument.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

                // Get the MainPart of the document
                MainDocumentPart mainPart = destinationDocument.MainDocumentPart;
                using (WordprocessingDocument sourceDocument = WordprocessingDocument.Open(sourceFileTemplate, true))
                {
                    CopyTableStyles(sourceDocument, destinationDocument);
                    //The Style not found in the template document
                    var table = GenerateTable();
                    mainPart.Document.Body.Append(table);
                }

            }
            MessageBox.Show(@"Table created successfully., Please check word document", @"C# and OpenXML", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void CopyTableStyles(WordprocessingDocument sourceDocument, WordprocessingDocument destinationDocument)
        {
            var sourceStyles = sourceDocument.MainDocumentPart.StyleDefinitionsPart.RootElement;
            var tableStyle = sourceStyles
                .Descendants<Style>()
                .FirstOrDefault(s => s.StyleName.Val == "Grid Table 5 Dark Accent 1");
            if (tableStyle != null)
            {
                var destinationStyles = destinationDocument.MainDocumentPart.StyleDefinitionsPart.RootElement;
                destinationStyles.AppendChild(tableStyle.CloneNode(true));
                destinationDocument.MainDocumentPart.StyleDefinitionsPart.Styles.Save();
            }
        }

        private void CreateNewStyle(WordprocessingDocument destination)
        {
            throw new NotImplementedException();
        }

        public Table GenerateTable()
        {
            var table = CreateTableStyle();


            AddItemsToTable(table);


            return table;
        }

        private static Table CreateTableStyle()
        {
            //create table object 
            Table table1 = new Table();

            //define table properties
            TableProperties tableProperties1 = new TableProperties();
            TableStyle tableStyle1 = new TableStyle() { Val = "GridTable5Dark-Accent1" };
            TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableLook tableLook1 = new TableLook() { Val = "04A0" };

            //add table styles to properties
            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);
            table1.Append(tableProperties1);
            return table1;
        }

        private static void AddItemsToTable(Table table1)
        {

            //Add test items to table
            #region MyRegion

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "2394" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2394" };
            GridColumn gridColumn3 = new GridColumn() { Width = "2394" };

            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);

            //create table row object
            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "004D1DA5", RsidTableRowProperties = "004D1DA5" };

            //create row properties
            TableRowProperties tableRowProperties1 = new TableRowProperties();
            ConditionalFormatStyle conditionalFormatStyle1 = new ConditionalFormatStyle() { Val = "100000000000" };

            tableRowProperties1.Append(conditionalFormatStyle1);

            //create cell object and its properties
            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            ConditionalFormatStyle conditionalFormatStyle2 = new ConditionalFormatStyle() { Val = "001000000000" };
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark1 = new HideMark();

            tableCellProperties1.Append(conditionalFormatStyle2);
            tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(hideMark1);

            //create paragrpah object and its properties
            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            Justification justification1 = new Justification() { Val = JustificationValues.Center };

            paragraphProperties1.Append(justification1);

            //create Run and Text 
            Run run1 = new Run();
            Text text1 = new Text();
            //add content in Text
            text1.Text = "No";

            //add Text to Run
            run1.Append(text1);

            //add Run to paragraph
            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            //add Paragraph to cell
            tableCell1.Append(tableCellProperties1);
            tableCell1.Append(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark2 = new HideMark();

            tableCellProperties2.Append(tableCellWidth2);
            tableCellProperties2.Append(hideMark2);

            Paragraph paragraph2 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();
            Justification justification2 = new Justification() { Val = JustificationValues.Center };
            ConditionalFormatStyle conditionalFormatStyle3 = new ConditionalFormatStyle() { Val = "100000000000" };

            paragraphProperties2.Append(justification2);
            paragraphProperties2.Append(conditionalFormatStyle3);

            Run run2 = new Run();
            Text text2 = new Text();
            text2.Text = "Name";

            run2.Append(text2);

            paragraph2.Append(paragraphProperties2);
            paragraph2.Append(run2);

            tableCell2.Append(tableCellProperties2);
            tableCell2.Append(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark3 = new HideMark();

            tableCellProperties3.Append(tableCellWidth3);
            tableCellProperties3.Append(hideMark3);

            Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties3 = new ParagraphProperties();
            Justification justification3 = new Justification() { Val = JustificationValues.Center };
            ConditionalFormatStyle conditionalFormatStyle4 = new ConditionalFormatStyle() { Val = "100000000000" };

            paragraphProperties3.Append(justification3);
            paragraphProperties3.Append(conditionalFormatStyle4);

            Run run3 = new Run();
            Text text3 = new Text();
            text3.Text = "Sex";

            run3.Append(text3);

            paragraph3.Append(paragraphProperties3);
            paragraph3.Append(run3);

            tableCell3.Append(tableCellProperties3);
            tableCell3.Append(paragraph3);

            tableRow1.Append(tableRowProperties1);
            tableRow1.Append(tableCell1);
            tableRow1.Append(tableCell2);
            tableRow1.Append(tableCell3);

            TableRow tableRow2 = new TableRow() { RsidTableRowAddition = "004D1DA5", RsidTableRowProperties = "004D1DA5" };

            TableRowProperties tableRowProperties2 = new TableRowProperties();
            ConditionalFormatStyle conditionalFormatStyle5 = new ConditionalFormatStyle() { Val = "000000100000" };

            tableRowProperties2.Append(conditionalFormatStyle5);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            ConditionalFormatStyle conditionalFormatStyle6 = new ConditionalFormatStyle() { Val = "001000000000" };
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark4 = new HideMark();

            tableCellProperties4.Append(conditionalFormatStyle6);
            tableCellProperties4.Append(tableCellWidth4);
            tableCellProperties4.Append(hideMark4);

            Paragraph paragraph4 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            Run run4 = new Run();
            Text text4 = new Text();
            text4.Text = "1";

            run4.Append(text4);

            paragraph4.Append(run4);

            tableCell4.Append(tableCellProperties4);
            tableCell4.Append(paragraph4);

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark5 = new HideMark();

            tableCellProperties5.Append(tableCellWidth5);
            tableCellProperties5.Append(hideMark5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties4 = new ParagraphProperties();
            ConditionalFormatStyle conditionalFormatStyle7 = new ConditionalFormatStyle() { Val = "000000100000" };

            paragraphProperties4.Append(conditionalFormatStyle7);

            Run run5 = new Run();
            Text text5 = new Text();
            text5.Text = "ABC";

            run5.Append(text5);

            paragraph5.Append(paragraphProperties4);
            paragraph5.Append(run5);

            tableCell5.Append(tableCellProperties5);
            tableCell5.Append(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark6 = new HideMark();

            tableCellProperties6.Append(tableCellWidth6);
            tableCellProperties6.Append(hideMark6);

            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties5 = new ParagraphProperties();
            ConditionalFormatStyle conditionalFormatStyle8 = new ConditionalFormatStyle() { Val = "000000100000" };

            paragraphProperties5.Append(conditionalFormatStyle8);

            Run run6 = new Run();
            Text text6 = new Text();
            text6.Text = "Male";

            run6.Append(text6);

            paragraph6.Append(paragraphProperties5);
            paragraph6.Append(run6);

            tableCell6.Append(tableCellProperties6);
            tableCell6.Append(paragraph6);

            tableRow2.Append(tableRowProperties2);
            tableRow2.Append(tableCell4);
            tableRow2.Append(tableCell5);
            tableRow2.Append(tableCell6);

            TableRow tableRow3 = new TableRow() { RsidTableRowAddition = "004D1DA5", RsidTableRowProperties = "004D1DA5" };

            TableRowProperties tableRowProperties3 = new TableRowProperties();
            ConditionalFormatStyle conditionalFormatStyle9 = new ConditionalFormatStyle() { Val = "000000010000" };

            tableRowProperties3.Append(conditionalFormatStyle9);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            ConditionalFormatStyle conditionalFormatStyle10 = new ConditionalFormatStyle() { Val = "001000000000" };
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark7 = new HideMark();

            tableCellProperties7.Append(conditionalFormatStyle10);
            tableCellProperties7.Append(tableCellWidth7);
            tableCellProperties7.Append(hideMark7);

            Paragraph paragraph7 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            Run run7 = new Run();
            Text text7 = new Text();
            text7.Text = "2";

            run7.Append(text7);

            paragraph7.Append(run7);

            tableCell7.Append(tableCellProperties7);
            tableCell7.Append(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark8 = new HideMark();

            tableCellProperties8.Append(tableCellWidth8);
            tableCellProperties8.Append(hideMark8);

            Paragraph paragraph8 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties6 = new ParagraphProperties();
            ConditionalFormatStyle conditionalFormatStyle11 = new ConditionalFormatStyle() { Val = "000000010000" };

            paragraphProperties6.Append(conditionalFormatStyle11);

            Run run8 = new Run();
            Text text8 = new Text();
            text8.Text = "PQR";

            run8.Append(text8);

            paragraph8.Append(paragraphProperties6);
            paragraph8.Append(run8);

            tableCell8.Append(tableCellProperties8);
            tableCell8.Append(paragraph8);

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            HideMark hideMark9 = new HideMark();

            tableCellProperties9.Append(tableCellWidth9);
            tableCellProperties9.Append(hideMark9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphAddition = "004D1DA5", RsidRunAdditionDefault = "004D1DA5" };

            ParagraphProperties paragraphProperties7 = new ParagraphProperties();
            ConditionalFormatStyle conditionalFormatStyle12 = new ConditionalFormatStyle() { Val = "000000010000" };

            paragraphProperties7.Append(conditionalFormatStyle12);

            Run run9 = new Run();
            Text text9 = new Text();
            text9.Text = "Female";

            run9.Append(text9);

            paragraph9.Append(paragraphProperties7);
            paragraph9.Append(run9);

            tableCell9.Append(tableCellProperties9);
            tableCell9.Append(paragraph9);

            tableRow3.Append(tableRowProperties3);
            tableRow3.Append(tableCell7);
            tableRow3.Append(tableCell8);
            tableRow3.Append(tableCell9);

            table1.Append(tableGrid1);
            table1.Append(tableRow1);
            table1.Append(tableRow2);
            table1.Append(tableRow3);

            #endregion
        }


        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFile.Text = openDlg.FileName;
            }
        }


    }
}
