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
    
    public partial class tblUser
    {
        public int user_id { get; set; }
        public string userId { get; set; }
        public string loginId { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int designationId { get; set; }
        public int siteId { get; set; }
        public string department { get; set; }
        public string brand_names { get; set; }
        public string contactNo { get; set; }
        public string imei { get; set; }
        public bool isActive { get; set; }
        public System.DateTime lastLogin { get; set; }
        public Nullable<int> salesPersonNo { get; set; }
        public Nullable<byte> visible_flag { get; set; }
        public bool notification_enabled { get; set; }
        public string notification_token { get; set; }
        public bool isAdmin { get; set; }
        public bool isCalculationActive { get; set; }
        public string appVersion { get; set; }
    }
}
