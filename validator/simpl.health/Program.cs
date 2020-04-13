using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/*using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;*/
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using simpl.health.Universes;

namespace simpl.health
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rowNum = 2;
            var columnNum = 1;
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                var filePath = new FileInfo(string.Concat(Environment.CurrentDirectory,
                    "/Data/Table1.xlsx"));
                var test = Environment.CurrentDirectory;
                using (var package = new ExcelPackage(filePath))
                {
                    
                    var worksheet = package.Workbook.Worksheets.First();

                    //Figure out the column names

                    //Get the list of columns
                    var row = worksheet.Row(1);
                    int columnLocation = 0;
                    List<string> columnNames = new List<string>();
                    while (worksheet.Cells[1, ++columnLocation].Value != null)
                    {
                        columnNames.Add(worksheet.Cells[1, columnLocation].Value.ToString());
                    }

                    var table = new Table1StandardOrgDeterminations();
                    var validation = table.ValidateFieldNames(columnNames);

                    if (validation.IsValid)
                    {
                        var keepProcessing = true;

                        List<string> values = new List<string>();
                        while (keepProcessing)
                        {
                            for (columnNum = 1; columnNum <= columnNames.Count(); columnNum ++)
                            {
                                if (rowNum == 2 && columnNum == 9)
                                {
                                    int test1 = 1;
                                }
                                var columnValue = worksheet.Cells[rowNum, columnNum].Value;
                                /*if (columnNum == 9)
                                {
                                    var test2 = worksheet.Cells[rowNum, columnNum].GetValue<DateTime>();
                                }*/
                                values.Add(columnValue == null ? string.Empty : columnValue.ToString());
                            }
                            keepProcessing = !values.All(v => String.IsNullOrWhiteSpace(v));
                            //Validate
                            table.ValidateFields(values);
                            if (keepProcessing)
                            {
                                rowNum++;
                            }
                        }
                       
                    }
                    

                }
               /* using (SpreadsheetDocument doc =
                    SpreadsheetDocument.Open(string.Concat(test,"/Data/Table1.xlsx"), false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();

                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    //Console.WriteLine(sheetData.InnerXml.ToString());
                    string text;
                    int count = sheetData.Elements<Row>().Count();
                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        var CellCont = r.ChildElements.Count();

                        var childElement = r.ChildElements[0];

                        childElement = childElement.ChildElements[0];

                   
                    }
           
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //CreateHostBuilder(args).Build().Run();
        }

        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
                */
    }
}
