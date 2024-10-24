﻿using System;
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
        [PrimaryKey, AutoIncrement]
        public int Id { set; get; }
        public int Amount { set; get; }
        public int Month { set; get; }
        public int Year { set; get; }
        public bool Active { set; get; }
    }
}
