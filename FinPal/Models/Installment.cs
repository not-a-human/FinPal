using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Models
{
    public class Installment
    {
        [ PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FinanceCode { get; set; }
        public string? ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Period { get; set; }
        public decimal Interest { get; set; }
        public decimal Fees { get; set; }
        public decimal Monthly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class InstallmentwithFinance
    {
        public string fName { get; set; }
        public string fNote { get; set; }
        public int Id { get; set; }
        public int FinanceCode { get; set; }
        public string? ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Period { get; set; }
        public decimal Interest { get; set; }
        public decimal Fees { get; set; }
        public decimal Monthly { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
