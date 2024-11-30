using FinPal.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Models
{
    public class Category : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public decimal Percentage { get; set; }
        public decimal FixedAmount { get; set; }
        public bool FixedOrPerc { get; set; } = true; // False = FixedAmount , True = Percentage
        public bool Active { get; set; } = true;
    }

    public class AllocatedFunds : Category
    {
        public decimal funds { get; set; }
    }
}
