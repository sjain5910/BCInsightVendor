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
    
    public partial class tblVendorLoginHistory
    {
        public int id { get; set; }
        public Nullable<int> vendorId { get; set; }
        public Nullable<System.DateTime> loginDate { get; set; }
        public string ip { get; set; }
    }
}
