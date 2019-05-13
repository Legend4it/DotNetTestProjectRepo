using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "Excel Interop";
                AssemblyHoursExcelTask();
            });
            task.Wait();

            Console.ReadLine();
        }

        private static void AssemblyHoursExcelTask()
        {
            var filePath = @"C:\Dev\DotNetTestProject\ExcelInterop\Assembly_time_calculator.xlsx";


            Application
                xlAppObject = new Application();

            xlAppObject.Visible = false;
            xlAppObject.ScreenUpdating = true;


            Workbook wb = xlAppObject.Workbooks.Open(filePath, Editable: true, IgnoreReadOnlyRecommended: true);


            Debug.WriteLine($"Nr of Sheet: {wb.Sheets.Count}");

            Sheets sheets = wb.Worksheets;
            Worksheet worksheet = (Worksheet) sheets.Item[1];
            var test = worksheet.get_Range("D5:B14");
            var test02 = (test.Find("Conveyor type") as Range);
            var test03 = test02.get_AddressLocal(false, false, Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1);
            Debug.WriteLine($"Sheet Name: {worksheet.Name}");

            //Get cell value
            Range excelRange = worksheet.UsedRange;




            //var conveyorTypeCell = (string)(worksheet.Cells[5, 3] as Range).Value;
            //var totalLengthOfTheConveyorCell = (double)(worksheet.Cells[6, 3] as Range).Value;
            //var nrOfTheConveyorCell = (double)(worksheet.Cells[7, 3] as Range).Value;
            //var nrOfBendsInTheSystemCell = (double)(worksheet.Cells[8, 3] as Range).Value;
            //var tSlotCoveringCell = (string)(worksheet.Cells[9, 3] as Range).Value;
            //var steelSlideRailCell = (string)(worksheet.Cells[10, 3] as Range).Value;
            //var typeOfSupportsCell = (string)(worksheet.Cells[11, 3] as Range).Value;
            //var nrOdSupportCell = (string)(worksheet.Cells[11, 3] as Range).Value;
            //var distanceBetweenTheSupportsCell = (double)(worksheet.Cells[12, 3] as Range).Value;
            //var assemplyOnTheHeightCell = (string)(worksheet.Cells[13, 3] as Range).Value;
            //var connectionOfTwoConveyorBeamCell = (string)(worksheet.Cells[114, 3] as Range).Value;


            var cellsTuple =(
            conveyorTypeCell: (string)(worksheet.Cells[5, 3] as Range).Value,
            totalLengthOfTheConveyorCell: (double)(worksheet.Cells[6, 3] as Range).Value,
            nrOfTheConveyorCell: (double)(worksheet.Cells[7, 3] as Range).Value,
            nrOfBendsInTheSystemCell: (double)(worksheet.Cells[8, 3] as Range).Value,
            tSlotCoveringCell: (string)(worksheet.Cells[9, 3] as Range).Value,
            steelSlideRailCell: (string)(worksheet.Cells[10, 3] as Range).Value,
            typeOfSupportsCell: (string)(worksheet.Cells[11, 3] as Range).Value,
            distanceBetweenTheSupportsCell: (double)(worksheet.Cells[12, 3] as Range).Value,
            nrOdSupportCell: (double)(worksheet.Cells[12, 4] as Range).Value,
            assemplyOnTheHeightCell: (string)(worksheet.Cells[13, 3] as Range).Value,
            connectionOfTwoConveyorBeamCell: (double)(worksheet.Cells[14, 3] as Range).Value
            );


            var cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;
            Debug.WriteLine($"Value: " +
                            $"{cellsTuple.conveyorTypeCell}\n" +
                            $"{cellsTuple.totalLengthOfTheConveyorCell}\n" +
                            $"{cellsTuple.nrOfTheConveyorCell}\n" +
                            $"{cellsTuple.nrOfBendsInTheSystemCell}\n" +
                            $"{cellsTuple.tSlotCoveringCell}\n" +
                            $"{cellsTuple.steelSlideRailCell}\n" +
                            $"{cellsTuple.typeOfSupportsCell}\n" +
                            $"{cellsTuple.nrOdSupportCell}\n" +
                            $"{cellsTuple.assemplyOnTheHeightCell}\n" +
                            $"{cellsTuple.distanceBetweenTheSupportsCell}\n" +
                            $"{cellsTuple.connectionOfTwoConveyorBeamCell}" +
                            $"Result: {Math.Round(cellResult)}"
            );


            //Parse value to cell
            excelRange.Cells.set_Item(5, 3, "XLX-X85X");

            wb.Save();

            cellsTuple = (
                conveyorTypeCell: (string)(worksheet.Cells[5, 3] as Range).Value,
                totalLengthOfTheConveyorCell: (double)(worksheet.Cells[6, 3] as Range).Value,
                nrOfTheConveyorCell: (double)(worksheet.Cells[7, 3] as Range).Value,
                nrOfBendsInTheSystemCell: (double)(worksheet.Cells[8, 3] as Range).Value,
                tSlotCoveringCell: (string)(worksheet.Cells[9, 3] as Range).Value,
                steelSlideRailCell: (string)(worksheet.Cells[10, 3] as Range).Value,
                typeOfSupportsCell: (string)(worksheet.Cells[11, 3] as Range).Value,
                distanceBetweenTheSupportsCell: (double)(worksheet.Cells[12, 3] as Range).Value,
                nrOdSupportCell: (double)(worksheet.Cells[12, 4] as Range).Value,
                assemplyOnTheHeightCell: (string)(worksheet.Cells[13, 3] as Range).Value,
                connectionOfTwoConveyorBeamCell: (double)(worksheet.Cells[14, 3] as Range).Value
            );
            cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;

            Debug.WriteLine($"Value: {cellsTuple.conveyorTypeCell}\n" +
                                   $"{cellsTuple.totalLengthOfTheConveyorCell}\n"+
                                   $"Result: {Math.Round(cellResult)}");

            //wb.Save();


            Marshal.ReleaseComObject(sheets);
            Marshal.ReleaseComObject(worksheet);

            wb.Close(0);
            xlAppObject.Quit();
        }
        //private static void ExcelTask()
        //{
        //    var filePath = @"C:\Dev\DotNetTestProject\ExcelInterop\Assembly_time_calculator.xlsx";


        //    Application
        //        xlAppObject = new Application();

        //    xlAppObject.Visible = false;
        //    xlAppObject.ScreenUpdating = true;


        //    Workbook wb = xlAppObject.Workbooks.Open(filePath, Editable: true, IgnoreReadOnlyRecommended: true);


        //    Debug.WriteLine($"Nr of Sheet: {wb.Sheets.Count}");

        //    Sheets sheets = wb.Worksheets;
        //    Worksheet worksheet = (Worksheet) sheets.Item[1];

        //    Debug.WriteLine($"Sheet Name: {worksheet.Name}");

        //    //Get cell value
        //    Range excelRange = worksheet.UsedRange;

        //    var cellOneValue = (string) (worksheet.Cells[5, 3] as Range).Value;
        //    var cellTwoValue = (double) (worksheet.Cells[6, 3] as Range).Value;
        //    var cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;


        //    Debug.WriteLine($"Value: {cellOneValue},{cellTwoValue}");

        //    //Parse value to cell
        //    excelRange.Cells.set_Item(5, 3, "X65P-X85P-XKP-003");
        //    excelRange.Cells.set_Item(8, 3, 300);


        //    cellOneValue = (string) (worksheet.Cells[5, 3] as Range).Value;
        //    cellTwoValue = (double) (worksheet.Cells[8, 3] as Range).Value;
        //    cellResult = (double) (worksheet.Cells[39, 10] as Range)?.Value;

        //    Debug.WriteLine($"Value: {cellOneValue},{cellTwoValue}");
        //    Debug.WriteLine($"Result: {cellResult}");

        //    wb.Save();


        //    Marshal.ReleaseComObject(sheets);
        //    Marshal.ReleaseComObject(worksheet);

        //    wb.Close(0);
        //    xlAppObject.Quit();
        //}
    }
}
