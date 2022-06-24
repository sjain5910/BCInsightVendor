using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class WeeklysalescompareViewModel
    {
        public string siteid { get; set; }
        public List<string> BrandnamesList { get; set; }
        public List<labesmodel> labels { get; set; }
        public string curryear { get; set; }
        public string preyear { get; set; }
        public List<weeklysalesitelist> sites { get; set; }
    }
    public class weeklysalesitelist
    {
        public string siteName { get; set; }
        public List<saledetailssitewise> sitewisesale { get; set; }
    }
    public class saledetailssitewise
    {
        public string BrandName { get; set; }
        public List<saledetailsyearwise> DaywiseSale { get; set; }
    }
    public class saledetailsyearwise
    {
        public int curryearsale { get; set; }
        public int preyearsale { get; set; }
        public int Diff { get; set; }
    }
    public class labesmodel
    {
        public string datelist { get; set; }
    }
}