﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Models
{
    public class Settings
    {
        public string APname { get; set; } = "";

        [PrimaryKey]
        public string SetKey { get; set; } = "";
        public string SetStr { get; set; } = "";
    }
}
