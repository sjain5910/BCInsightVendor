using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class vendorDailySaleReportviewModel
    {
        public string processdate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> BrandnamesList { get; set; }
        public List<vendorDailySalebrand> brandlist { get; set; }
    }
    public class vendorDailySalebrand
    {
        public string Brandname { get; set; }       
        public List<salesviewmodel> salelist { get; set; }
    }

    public class salesviewmodel
    {       
        public DateTime saleDate { get; set; }
        public string siteName { get; set; }        
        public string department { get; set; }
        public string cat2 { get; set; }
        public string billNo { get; set; }
        public string section { get; set; }
        public string product { get; set; }
        public string cat6code { get; set; }
        public string ItemDesc6 { get; set; }
        public string cat3color { get; set; }
        public string cat4size { get; set; }
        public int saleQty { get; set; }
        public int mrpAmt { get; set; }
        public string PrmoAmt { get; set; }
        public string ItemDiscountAmt { get; set; }
        public string BillDiscountAmt { get; set; }
        public string LPDiscountAmt { get; set; }
        public string ExTaxAmtFactor { get; set; }
        public decimal netAmt { get; set; }        
    }
    public class salesviewmodelexport
    {
        public DateTime saleDate { get; set; }
        public string siteName { get; set; }
        public string department { get; set; }
        public string cat2 { get; set; }
        public string billNo { get; set; }
        public string section { get; set; }
        public string product { get; set; }
        public string cat6code { get; set; }
        public string ItemDesc6 { get; set; }
        public string cat3color { get; set; }
        public string cat4size { get; set; }
        public int saleQty { get; set; }
        public int mrpAmt { get; set; }
        public decimal? PrmoAmt { get; set; }
        public decimal? ItemDiscountAmt { get; set; }
        public decimal? BillDiscountAmt { get; set; }
        public decimal? LPDiscountAmt { get; set; }
        public decimal? ExTaxAmtFactor { get; set; }
        public decimal netAmt { get; set; }
    }
}