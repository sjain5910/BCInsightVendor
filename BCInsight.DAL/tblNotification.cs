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
    
    public partial class tblNotification
    {
        public int notif_id { get; set; }
        public string notificationType { get; set; }
        public Nullable<int> fromUserId { get; set; }
        public Nullable<int> fromSiteId { get; set; }
        public Nullable<int> toSiteId { get; set; }
        public string toEmail { get; set; }
        public string toMobile { get; set; }
        public string notificationText { get; set; }
        public string stockList { get; set; }
        public string statusFlag { get; set; }
        public Nullable<System.DateTime> notificationDate { get; set; }
        public Nullable<System.DateTime> actionDate { get; set; }
        public string requestNote { get; set; }
        public string clientReference { get; set; }
        public string styleCode { get; set; }
    }
}