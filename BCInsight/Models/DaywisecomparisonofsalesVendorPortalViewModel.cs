using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class DaywisecomparisonofsalesVendorPortalViewModel
    {       
        public string saledate { get; set; }
        public string presaledate { get; set; }
        public string siteid { get; set; }
        public List<string> BrandnamesList { get; set; }
        public List<SiteNameslist> siteNames { get; set; }        
    }
    public class SiteNameslist
    {
        public string siteName { get; set; }
        public List<BrandSaleDetails> BrandSale { get; set; }
    }
    public class BrandSaleDetails
    {
        public string Brand { get; set; }
        public int CurrentDate { get; set; }
        public int LastDate { get; set; }
        public int differance { get; set; }
    }
}