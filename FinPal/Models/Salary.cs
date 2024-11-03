using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPal.Interfaces;
using SQLite;

namespace FinPal.Models
{
    public class Salary : IModel
    {
        [PrimaryKey]
        public int Id { set; get; }
        public decimal Amount { set; get; }
        public int Month { set; get; }
        public int Year { set; get; }
        public bool Active { set; get; }
    }
}
