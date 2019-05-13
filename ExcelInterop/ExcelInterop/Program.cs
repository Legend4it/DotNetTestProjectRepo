using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew((ExcelTask));
            task.Wait();

            Console.ReadLine();
        }

        private static void ExcelTask()
        {
            var filePath = @"C:\Dev\DotNetTestProject\ExcelInterop\Assembly_time_calculator.xlsx";


            Application
                xlAppObject = new Application();

            xlAppObject.Visible = false; //Excel WorkBook Visible mode true , False InVisible  
            xlAppObject.ScreenUpdating = true;


            Workbook wb = xlAppObject.Workbooks.Open(filePath, Editable: true, IgnoreReadOnlyRecommended: true);
            //Workbook wb = xlAppObject.Workbooks.Open(filePath, 0, false, 5, "", "", false, XlPlatform.xlWindows, "",
            //    true, false, 0, true, false, false);


            Debug.WriteLine($"Nr of Sheet: {wb.Sheets.Count}");

            Sheets sheets = wb.Worksheets;
            Worksheet worksheet = (Worksheet) sheets.Item[1];

            Debug.WriteLine($"Sheet Name: {worksheet.Name}");

            //Get cell value
            Range excelRange = worksheet.UsedRange;
            //var cellOneValue = excelRange.Item[1, 1].ToString();
            //var cellTwoValue = excelRange.Item[1, 2].ToString();

            var cellOneValue = (string) (worksheet.Cells[5, 3] as Range).Value;
            var cellTwoValue = (double) (worksheet.Cells[6, 3] as Range).Value;
            var cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;


            Debug.WriteLine($"Value: {cellOneValue},{cellTwoValue}");

            //Parse value to cell
            excelRange.Cells.set_Item(5, 3, "X65P-X85P-XKP");
            excelRange.Cells.set_Item(6, 3, 300);


            //cellOneValue = excelRange.Item[1, 1].ToString();
            //cellTwoValue = excelRange.Item[1, 2].ToString();
            cellOneValue = (string) (worksheet.Cells[5, 3] as Range).Value;
            cellTwoValue = (double) (worksheet.Cells[6, 3] as Range).Value;
            cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;

            Debug.WriteLine($"Value: {cellOneValue},{cellTwoValue}");
            Debug.WriteLine($"Result: {cellResult}");

            ////wb.Save();
            if (File.Exists(@"C:\Dev\DotNetTestProject\ExcelInterop\tmpExcel2019.xlsx"))
            {
                File.Delete(@"C:\Dev\DotNetTestProject\ExcelInterop\tmpExcel2019.xlsx");
            }

            wb.SaveAs(@"C:\Dev\DotNetTestProject\ExcelInterop\tmpExcel2019", XlFileFormat.xlWorkbookDefault);


            Marshal.ReleaseComObject(sheets);
            Marshal.ReleaseComObject(worksheet);

            wb.Close(0);
            xlAppObject.Quit();
        }
    }
}
