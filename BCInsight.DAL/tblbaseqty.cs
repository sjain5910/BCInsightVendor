//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BCInsight.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblbaseqty
    {
        public int baseid { get; set; }
        public string barcode { get; set; }
        public string siteCuid { get; set; }
        public string division { get; set; }
        public string section { get; set; }
        public string department { get; set; }
        public string brand { get; set; }
        public string styleCode { get; set; }
        public string color { get; set; }
        public string fit { get; set; }
        public string size { get; set; }
        public int baseQty { get; set; }
        public System.DateTime lastUpdateDate { get; set; }
        public int initQty { get; set; }
        public int rptcurrQty { get; set; }
        public int calreqtoAddQty { get; set; }
        public int calrecvdQty { get; set; }
        public string combination { get; set; }
        public int whinitqty { get; set; }
    }
}