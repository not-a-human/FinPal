using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Models
{
    // Model for PRAGMA table_info query
    public class TableColumnInfo
    {
        public string Name { get; set; } = "";
        public string? Type { get; set; }
        public int NotNull { get; set; }
        public string? DefaultValue { get; set; }
        public int PrimaryKey { get; set; }
    }
}
