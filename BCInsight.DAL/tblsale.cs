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
    
    public partial class tblsale
    {
        public int sale_id { get; set; }
        public System.DateTime import_date { get; set; }
        public System.DateTime saleDate { get; set; }
        public string uniqueBillId { get; set; }
        public string billNo { get; set; }
        public string siteCuid { get; set; }
        public string department { get; set; }
        public string section { get; set; }
        public string product { get; set; }
        public string brandName { get; set; }
        public string cat6 { get; set; }
        public string cat3 { get; set; }
        public string cat4 { get; set; }
        public string salesPersonNo { get; set; }
        public string itemDesc4 { get; set; }
        public string cat2 { get; set; }
        public string customerName { get; set; }
        public string customerMobile { get; set; }
        public int saleQty { get; set; }
        public int mrpAmt { get; set; }
        public decimal netAmt { get; set; }
        public string PrmoAmt { get; set; }
        public string ItemDesc6 { get; set; }
        public string ItemDiscountAmt { get; set; }
        public string BillDiscountAmt { get; set; }
        public string LPDiscountAmt { get; set; }
        public string ExTaxAmtFactor { get; set; }
        public int flag_delete { get; set; }
        public Nullable<int> pvt_label_group_id { get; set; }
        public string vendorName { get; set; }
    }
}
