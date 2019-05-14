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
                Thread.CurrentThread.Name = "ExcelInterop2019";
                AssemblyHoursExcelTask();
            });
            task.Wait();
            task.Dispose();
            Console.ReadLine();
        }

        private static void AssemblyHoursExcelTask()
        {
            var filePath = @"C:\Dev\Source\DotNetTestProjectRepo\ExcelInterop\Assembly_time_calculator.xlsx";


            Application
                xlAppObject = new Application();

            xlAppObject.Visible = false;
            xlAppObject.ScreenUpdating = true;


            Workbook wb = xlAppObject.Workbooks.Open(filePath, Editable: true, IgnoreReadOnlyRecommended: true);


            Debug.WriteLine($"Nr of Sheet: {wb.Sheets.Count}");

            Sheets sheets = wb.Worksheets;
            Worksheet worksheet = (Worksheet) sheets.Item[1];
            //var rangeCells = worksheet.Range["D5:B1"];
            //var findCell = (rangeCells.Find("Conveyor type") as Range);
            //var addressForCell = findCell.AddressLocal[false, false, Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1];

            Debug.WriteLine($"Sheet Name: {worksheet.Name}");

            ////Get cell value
            //Range excelRange = worksheet.UsedRange;


            var cellsTuple = (
                conveyorTypeCell: worksheet.Range["$C$5"].Value,
                totalLengthOfTheConveyorCell: worksheet.Range["$C$6"].Value,
                nrOfTheConveyorCell: worksheet.Range["$C$7"].Value,
                nrOfBendsInTheSystemCell: worksheet.Range["$C$8"].Value,
                tSlotCoveringCell: worksheet.Range["$C$9"].Value,
                steelSlideRailCell: worksheet.Range["$C$10"].Value,
                typeOfSupportsCell: worksheet.Range["$C$11"].Value,
                distanceBetweenTheSupportsCell: worksheet.Range["$C$12"].Value,
                nrOdSupportCell: worksheet.Range["$D$12"].Value,
                assemplyOnTheHeightCell: worksheet.Range["$C$13"].Value,
                connectionOfTwoConveyorBeamCell: worksheet.Range["$C$14"].Value,
                guideRail: worksheet.Range["$B$292"].Value
            );


            var cellResult = worksheet.Range["$J$39"]?.Value;
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
                            $"{cellsTuple.guideRail}" +
                            $"Result: {Math.Round(cellResult)}"
            );


            //Parse value to cell
            Range rng = worksheet.Range["$C$5"];
            rng.Value2 = "XLX-X85X";
            rng = worksheet.Range["$B$292"];
            rng.Value2 = true;
            rng = worksheet.Range["$C$6"];
            rng.Value2 = 100;
            wb.Save();

            cellsTuple = (
                conveyorTypeCell: worksheet.Range["$C$5"].Value,
                totalLengthOfTheConveyorCell: worksheet.Range["$C$6"].Value,
                nrOfTheConveyorCell: worksheet.Range["$C$7"].Value,
                nrOfBendsInTheSystemCell: worksheet.Range["$C$8"].Value,
                tSlotCoveringCell: worksheet.Range["$C$9"].Value,
                steelSlideRailCell: worksheet.Range["$C$10"].Value,
                typeOfSupportsCell: worksheet.Range["$C$11"].Value,
                distanceBetweenTheSupportsCell: worksheet.Range["$C$12"].Value,
                nrOdSupportCell: worksheet.Range["$D$12"].Value,
                assemplyOnTheHeightCell: worksheet.Range["$C$13"].Value,
                connectionOfTwoConveyorBeamCell: worksheet.Range["$C$14"].Value,
                guideRail: worksheet.Range["$B$292"].Value
            );
            cellResult = worksheet.Range["$J$39"]?.Value;

            Debug.WriteLine($"Value: {cellsTuple.conveyorTypeCell}\n" +
                                   $"{cellsTuple.totalLengthOfTheConveyorCell}\n"+
                                   $"{cellsTuple.guideRail}\n" +
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
