using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class WeeklysalesVendorPortalViewModel
    {
        public string siteid { get; set; }
        public List<string> label { get; set; }
        public List<SiteNamesweeklylist> siteNames { get; set; }
    }
    public class SiteNamesweeklylist
    {
        public string siteName { get; set; }
        public List<BrandSaleWeeklyDetails> BrandSale { get; set; }
    }
    public class BrandSaleWeeklyDetails
    {
        public string Brand { get; set; }
        public List<int> daywisesale { get; set; }
        public int Total { get; set; }
    }

    public class salehistory
    {
        public DateTime saledate { get; set; }
        public int amount { get; set; }
    }
}