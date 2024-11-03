using FinPal.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Models
{
    public class FinanceName : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime PayDate { get; set; }
        public bool Reminder { get; set; } = false;
        public bool Active { get; set; } = true;
    }
}
