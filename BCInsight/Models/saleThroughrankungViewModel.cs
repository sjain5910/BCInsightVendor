using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class saleThroughrankungViewModel
    {
        public string BrandName { get; set; }
        public string BrandCategory { get; set; }
        public int Ranking { get; set; }
        public string Division { get; set; }
        public int SaleQuantity { get; set; }
        public int SaleAmount { get; set; }
        public int StockclosingQuantity { get; set; }
        public int StockclosingAmount { get; set; }
        public double saleThroughReport { get; set; }
        public double EntireBrandsaleThroughReport { get; set; }

    }
    public class saleThroughreankungViewModel
    {

        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }


    }
    public class stocksaleThroughreankungViewModel
    {

        public string BrandName { get; set; }
        public string Division { get; set; }
        public int closingQuantity { get; set; }
        public int closingAmount { get; set; }


    }
}