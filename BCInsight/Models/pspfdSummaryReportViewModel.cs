﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class pspfdSummaryReportViewModel
    {
        public string BrandName { get; set; }
        public int Ranking { get; set; }
        public string ZoneName { get; set; }
        public string Category { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int Avg { get; set; }
    }
    public class PfpsdDetailreportdataViewModel
    {
        public string BrandName { get; set; }
        public string Month { get; set; }
        public string ZoneName { get; set; }
        public string Category { get; set; }
        public int WallSpace { get; set; }
        public int Height { get; set; }
        public int Total { get; set; }
        public int Sqft { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public int persqftrevenue { get; set; }
    }
}