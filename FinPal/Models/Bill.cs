using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPal.Interfaces;
using SQLite;

namespace FinPal.Models
{
    public class Bill :IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public decimal AmountDue { get; set; }
        public string CategoryCode { get; set; } = "";
        public int FinanceCode { get; set; }
        public string ItemName { get; set; } = "";
        public string Repeat { get; set; } = "M";
        public int Period { get; set; }
        public bool Continuous { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Total { get; set; }
        public decimal InterestRate { get; set; }
        public decimal AdminFee { get; set; }
        public string Note { get; set; } = "";
        public bool Active { get; set; } = true;
    }

    public class BillwithFC : Bill
    {
        public string FName { get; set; } = "";
        public string FNote { get; set; } = "";
        public string CName { get; set; } = "";
        public string CNote { get; set; } = "";
    }
}
