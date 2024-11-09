using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using FinPal.Models;
using FinPal.Data;
using CommunityToolkit.Maui.Storage;
using System.Globalization;

namespace FinPal.Services
{
    public class ExcelDataHelper
    {

        public static CategoryDatabase CDatabase { get; private set; } = new CategoryDatabase();
        public static FinanceNameDatabase FDatabase { get; private set; } = new FinanceNameDatabase();
        public static SalaryDatabase SDatabase { get; private set; } = new SalaryDatabase();
        public static BillDatabase BDatabase { get; private set; } = new BillDatabase();

        public async Task<string> ExportDataToExcel()
        {
            
            List<Bill> bills = await BDatabase.GetItemsAsync();
            List<Category> categories = await CDatabase.GetItemsAsync();
            List<FinanceName> financeNames = await FDatabase.GetItemsAsync();
            List<Salary> salaries = await SDatabase.GetItemsAsync();
            
            using (var workbook = new XLWorkbook())
            {
                // Worksheet for Bill
                var billSheet = workbook.Worksheets.Add("Bills");
                var billHeaders = new[] { "ID", "Category ID", "Finance ID", "Item Name", "Amount Due", "Continuous","Period",
                                  "Start Date", "End Date", "Total", "Interest Rate", "Admin Fee", "Note", "Active", 
                                    "", "", "Category Name", "Finance Name"
                                   };

                for (int i = 0; i < billHeaders.Length; i++)
                {
                    billSheet.Cell(1, i + 1).Value = billHeaders[i];
                }

                int billRow = 2;
                foreach (var bill in bills)
                {
                    billSheet.Cell(billRow, 1).Value = bill.Id;
                    billSheet.Cell(billRow, 2).Value = Int32.Parse(bill.CategoryCode);
                    billSheet.Cell(billRow, 3).Value = bill.FinanceCode;
                    billSheet.Cell(billRow, 4).Value = bill.ItemName;
                    billSheet.Cell(billRow, 5).Value = bill.AmountDue;
                    billSheet.Cell(billRow, 6).Value = bill.Continuous;
                    billSheet.Cell(billRow, 7).Value = bill.Period;
                    billSheet.Cell(billRow, 8).Value = bill.StartDate.ToString("dd-MMM-yyyy");
                    billSheet.Cell(billRow, 9).Value = bill.EndDate.ToString("dd-MMM-yyyy");
                    billSheet.Cell(billRow, 10).Value = bill.Total;
                    billSheet.Cell(billRow, 11).Value = bill.InterestRate;
                    billSheet.Cell(billRow, 12).Value = bill.AdminFee;
                    billSheet.Cell(billRow, 13).Value = bill.Note;
                    billSheet.Cell(billRow, 14).Value = bill.Active;
                    billSheet.Cell(billRow, 17).FormulaA1 = $"=INDEX(Categories!B: B, MATCH(B"+billRow+", Categories!A: A, 0))";
                    billSheet.Cell(billRow, 18).FormulaA1 = $"=INDEX(Finances!C: C, MATCH(C"+billRow+", Finances!A: A, 0))";

                    billRow++;
                }

                billSheet.Cell(billRow, 4).Value = "Total";
                billSheet.Cell(billRow, 5).FormulaA1 = $"=SUM(E2:E"+(billRow-1)+")";


                // Worksheet for Category
                var categorySheet = workbook.Worksheets.Add("Categories");
                var categoryHeaders = new[] { "ID", "Name", "Note", "Percentage", "Active" };

                for (int i = 0; i < categoryHeaders.Length; i++)
                {
                    categorySheet.Cell(1, i + 1).Value = categoryHeaders[i];
                }

                int categoryRow = 2;
                foreach (var category in categories)
                {
                    categorySheet.Cell(categoryRow, 1).Value = category.Id;
                    categorySheet.Cell(categoryRow, 2).Value = category.Name;
                    categorySheet.Cell(categoryRow, 3).Value = category.Note;
                    categorySheet.Cell(categoryRow, 4).Value = category.Percentage;
                    categorySheet.Cell(categoryRow, 5).Value = category.Active;

                    categoryRow++;
                }

                // Worksheet for FinanceName
                var financeSheet = workbook.Worksheets.Add("Finances");
                var financeHeaders = new[] { "ID", "Category ID", "Name", "Note", "Pay Date", "Reminder", "Active" };

                for (int i = 0; i < financeHeaders.Length; i++)
                {
                    financeSheet.Cell(1, i + 1).Value = financeHeaders[i];
                }

                int financeRow = 2;
                foreach (var finance in financeNames)
                {
                    financeSheet.Cell(financeRow, 1).Value = finance.Id;
                    financeSheet.Cell(financeRow, 2).Value = finance.CategoryId;
                    financeSheet.Cell(financeRow, 3).Value = finance.Name;
                    financeSheet.Cell(financeRow, 4).Value = finance.Note;
                    financeSheet.Cell(financeRow, 5).Value = finance.PayDate.ToString("dd-MMM-yyyy");
                    financeSheet.Cell(financeRow, 6).Value = finance.Reminder;
                    financeSheet.Cell(financeRow, 7).Value = finance.Active;

                    financeRow++;
                }

                // Worksheet for Salary
                var salarySheet = workbook.Worksheets.Add("Salaries");
                var salaryHeaders = new[] { "ID", "Month", "Year", "Active", "Amount" };

                for (int i = 0; i < salaryHeaders.Length; i++)
                {
                    salarySheet.Cell(1, i + 1).Value = salaryHeaders[i];
                }

                int salaryRow = 2;
                foreach (var salary in salaries)
                {
                    salarySheet.Cell(salaryRow, 1).Value = salary.Id;
                    salarySheet.Cell(salaryRow, 2).Value = salary.Month;
                    salarySheet.Cell(salaryRow, 3).Value = salary.Year;
                    salarySheet.Cell(salaryRow, 4).Value = salary.Active;
                    salarySheet.Cell(salaryRow, 5).Value = salary.Amount;

                    salaryRow++;
                }

                salarySheet.Cell(salaryRow, 4).Value = "Total";
                salarySheet.Cell(salaryRow, 5).FormulaA1 = $"=SUM(E2:E"+(salaryRow-1)+")";


                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0; // Reset the stream position

                    var fileSaverResult = await FileSaver.Default.SaveAsync("DataWorkbook.xlsx", stream);
                    if (fileSaverResult.IsSuccessful)
                    {
                        return "Data has been exported to "+fileSaverResult.FilePath;
                    }
                    else
                    {
                        return "Data could not be exported to the following error: "+fileSaverResult.Exception;
                    }
                }
                    // Save the workbook to the specified file path
                    //workbook.SaveAs(filePath);
            }
        }


        public async Task<string> GenerateExcelReport()
        {

            List<BillwithFC> bills = await BDatabase.GetItemsAsyncWithFC();
            List<Category> categories = await CDatabase.GetItemsAsync();
            List<FinanceName> financeNames = await FDatabase.GetItemsAsync();
            List<Salary> salaries = await SDatabase.GetItemsAsync();

            using (var workbook = new XLWorkbook())
            {
                // Worksheet for Bill
                var billSheet = workbook.Worksheets.Add("Bills");
                var billHeaders = new[] { "Category", "Finance", "Item Name", "Monthly Payment", "Period",
                                  "Start Date", "End Date", "Total", "Interest Rate", "Admin Fee", "Note", "Active"
                                   };

                for (int i = 0; i < billHeaders.Length; i++)
                {
                    billSheet.Cell(1, i + 1).Value = billHeaders[i];
                }

                int billRow = 2;
                foreach (var bill in bills)
                {
                    
                    billSheet.Cell(billRow, 1).Value = bill.CName;
                    billSheet.Cell(billRow, 2).Value = bill.FName;
                    billSheet.Cell(billRow, 3).Value = bill.ItemName;
                    billSheet.Cell(billRow, 4).Value = bill.AmountDue;
                    billSheet.Cell(billRow, 5).Value = bill.Period;
                    billSheet.Cell(billRow, 6).Value = bill.StartDate;
                    billSheet.Cell(billRow, 7).Value = bill.EndDate;
                    billSheet.Cell(billRow, 8).Value = bill.Total;
                    billSheet.Cell(billRow, 9).Value = bill.InterestRate;
                    billSheet.Cell(billRow, 10).Value = bill.AdminFee;
                    billSheet.Cell(billRow, 11).Value = bill.Note;
                    billSheet.Cell(billRow, 12).Value = bill.Active;

                    billRow++;
                }

                billSheet.Cell(billRow, 4).Value = "Total";
                billSheet.Cell(billRow, 5).FormulaA1 = $"=SUM(E2:E" + (billRow - 1) + ")";


                // Worksheet for Category
                var categorySheet = workbook.Worksheets.Add("Categories");
                var categoryHeaders = new[] { "ID", "Name", "Note", "Percentage", "Active" };

                for (int i = 0; i < categoryHeaders.Length; i++)
                {
                    categorySheet.Cell(1, i + 1).Value = categoryHeaders[i];
                }

                int categoryRow = 2;
                foreach (var category in categories)
                {
                    categorySheet.Cell(categoryRow, 1).Value = category.Id;
                    categorySheet.Cell(categoryRow, 2).Value = category.Name;
                    categorySheet.Cell(categoryRow, 3).Value = category.Note;
                    categorySheet.Cell(categoryRow, 4).Value = category.Percentage;
                    categorySheet.Cell(categoryRow, 5).Value = category.Active;

                    categoryRow++;
                }

                // Worksheet for Salary
                var salarySheet = workbook.Worksheets.Add("Salaries");
                var salaryHeaders = new[] { "ID", "Month", "Year", "Active", "Amount" };

                for (int i = 0; i < salaryHeaders.Length; i++)
                {
                    salarySheet.Cell(1, i + 1).Value = salaryHeaders[i];
                }

                int salaryRow = 2;
                foreach (var salary in salaries)
                {
                    salarySheet.Cell(salaryRow, 1).Value = salary.Id;
                    salarySheet.Cell(salaryRow, 2).Value = salary.Month;
                    salarySheet.Cell(salaryRow, 3).Value = salary.Year;
                    salarySheet.Cell(salaryRow, 4).Value = salary.Active;
                    salarySheet.Cell(salaryRow, 5).Value = salary.Amount;

                    salaryRow++;
                }

                salarySheet.Cell(salaryRow, 4).Value = "Total";
                salarySheet.Cell(salaryRow, 5).FormulaA1 = $"=SUM(E2:E" + (salaryRow - 1) + ")";


                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0; // Reset the stream position

                    var fileSaverResult = await FileSaver.Default.SaveAsync("DataWorkbook.xlsx", stream);
                    if (fileSaverResult.IsSuccessful)
                    {
                        return "Reported has been generated to " + fileSaverResult.FilePath;
                    }
                    else
                    {
                        return "Report could not be generated due to the following error: " + fileSaverResult.Exception;
                    }
                }
                // Save the workbook to the specified file path
                //workbook.SaveAs(filePath);
            }
        }

        public async Task ImportDataFromExcel(Stream fileStream)
        {
            using (var workbook = new XLWorkbook(fileStream))
            {
                // Import Bills
                var billSheet = workbook.Worksheet("Bills");
                var bills = new List<Bill>();

                foreach (var row in billSheet.RowsUsed().Skip(1)) // Skip header
                {
                    
                    if (row.Cell(4).GetString() == "Total")
                    {
                        continue; // Skip rows that contain the summary
                    }

                    var bill = new Bill
                    {
                        Id = row.Cell(1).IsEmpty() ? 0 : row.Cell(1).GetValue<int>(),
                        CategoryCode = row.Cell(2).IsEmpty() ? "1" : row.Cell(2).GetString(),
                        FinanceCode = row.Cell(3).IsEmpty() ? 1 : row.Cell(3).GetValue<int>(),
                        ItemName = row.Cell(4).GetString(),
                        AmountDue = row.Cell(5).GetValue<decimal>(),
                        Continuous = row.Cell(6).IsEmpty() ? false : row.Cell(6).GetValue<bool>(),
                        Period = row.Cell(7).IsEmpty() ? 0 : row.Cell(7).GetValue<int>(),
                        StartDate = ParseDate(row.Cell(8).GetString()),
                        EndDate = ParseDate(row.Cell(9).GetString()),
                        Total = row.Cell(10).IsEmpty() ? 0 : row.Cell(10).GetValue<decimal>(),
                        InterestRate = row.Cell(11).IsEmpty() ? 0 : row.Cell(11).GetValue<decimal>(),
                        AdminFee = row.Cell(12).IsEmpty() ? 0 : row.Cell(12).GetValue<decimal>(),
                        Note = row.Cell(13).GetString(),
                        Active = row.Cell(14).IsEmpty() ? true : row.Cell(14).GetValue<bool>(),
                    };
                    bills.Add(bill);
                }

                foreach(var item in bills)
                {
                    await BDatabase.SaveItemAsync(item);
                }
                
                // Import Categories
                var categorySheet = workbook.Worksheet("Categories");
                var categories = new List<Category>();

                foreach (var row in categorySheet.RowsUsed().Skip(1)) // Skip header
                {
                    var category = new Category
                    {
                        Id = row.Cell(1).IsEmpty() ? 0 : row.Cell(1).GetValue<int>(),
                        Name = row.Cell(2).GetString(),
                        Note = row.Cell(3).GetString(),
                        Percentage = row.Cell(4).IsEmpty() ? 0 : row.Cell(4).GetValue<decimal>(),
                        Active = row.Cell(5).IsEmpty() ? true : row.Cell(5).GetValue<bool>(),
                    };
                    categories.Add(category);
                }

                
                foreach (var item in categories)
                {
                    await CDatabase.SaveItemAsync(item);
                }

                // Import Finance Names
                var financeSheet = workbook.Worksheet("Finances");
                var financeNames = new List<FinanceName>();

                foreach (var row in financeSheet.RowsUsed().Skip(1)) // Skip header
                {
                    var finance = new FinanceName
                    {
                        Id = row.Cell(1).IsEmpty() ? 0 : row.Cell(1).GetValue<int>(),
                        CategoryId = row.Cell(2).IsEmpty() ? 1 : row.Cell(2).GetValue<int>(),
                        Name = row.Cell(3).GetString(),
                        Note = row.Cell(4).GetString(),
                        PayDate = ParseDate(row.Cell(5).GetString()),
                        Reminder = row.Cell(6).IsEmpty() ? true : row.Cell(6).GetValue<bool>(),
                        Active = row.Cell(7).IsEmpty() ? true : row.Cell(7).GetValue<bool>(),
                    };
                    financeNames.Add(finance);
                }

                foreach (var item in financeNames)
                {
                    await FDatabase.SaveItemAsync(item);
                }

                // Import Salaries
                var salarySheet = workbook.Worksheet("Salaries");
                var salaries = new List<Salary>();

                foreach (var row in salarySheet.RowsUsed().Skip(1)) // Skip header
                {
                    if (row.Cell(4).GetString() == "Total")
                    {
                        continue; // Skip rows that contain the summary
                    }

                    var salary = new Salary
                    {
                        Id = row.Cell(1).IsEmpty() ? 0 : row.Cell(1).GetValue<int>(),
                        Month = row.Cell(2).GetValue<int>(),
                        Year = row.Cell(3).GetValue<int>(),
                        Active = row.Cell(4).GetValue<bool>(),
                        Amount = row.Cell(5).GetValue<decimal>(),
                    };
                    salaries.Add(salary);
                }

                foreach (var item in salaries)
                {
                    await SDatabase.SaveItemAsync(item);
                }
            }
        }

        // Helper method to parse the date string with a specified format
        private DateTime ParseDate(string dateStr)
        {
            string[] dateFormats = { "dd-MMM-yyyy" };
            // Attempt to parse the date in the specified format
            DateTime parsedDate;
            if (DateTime.TryParseExact(dateStr, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }

            // Handle invalid date or use a default date if parsing fails
            return new DateTime(1900, 1, 1); // Default date, or DateTime.MinValue if preferred
        }
    }
}
