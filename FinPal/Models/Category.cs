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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public double Percentage { get; set; }
        public bool Active { get; set; } = true;
    }
}
