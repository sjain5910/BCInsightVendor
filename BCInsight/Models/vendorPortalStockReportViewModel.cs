using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class vendorPortalStockReportViewModel
    {
        public string siteid { get; set; }
        public string stockdate { get; set; }
        public string dispDate { get; set; }
        public List<string> BrandnamesList { get; set; }
        public List<sitelistmodel> Sitelist { get; set; }
    }
    public class sitelistmodel
    {
        public string SiteName { get; set; }
        public List<Brandsviewmodel> brandslist { get; set; }
    }
    public class Brandsviewmodel
    {
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public List<Departmentsviewmodel> departmentslist { get; set; }
    }
    public class Departmentsviewmodel
    {
        public string Department { get; set; }
        public int DeptQty { get; set; }
    }
}