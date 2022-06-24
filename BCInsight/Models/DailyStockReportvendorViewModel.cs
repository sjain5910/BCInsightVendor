using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class detailsStockReportvendorViewModel
    {
        public string siteid { get; set; }
        public string Date { get; set; }
        public string stockdate { get; set; }
        public List<string> BrandnamesList { get; set; }
        public List<siteNamelistmodel> Sitelist { get; set; }
    }
    public class siteNamelistmodel
    {
        public string SiteName { get; set; }
        public List<Stockdetailsviewmodel> brandslist { get; set; }
    }
    public class Stockdetailsviewmodel
    {
        public string Section { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string Desc4 { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string Stylecode { get; set; }
        public string Desc6 { get; set; }
        public string size { get; set; }
        public string Fit { get; set; }
        public string Quantity { get; set; }
        public string MRP { get; set; }
    }
}