using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class VendorLoginMgmtViewModel
    {
        public VendorLoginMgmtViewModel()
        {

        }
        public VendorLoginMgmtViewModel(tblVendorLoginHistory loginhistory)
        {
            if(loginhistory!=null)
            {
                id = loginhistory.id;
                vendorId = loginhistory.vendorId;
                loginDate = loginhistory.loginDate;
                ip = loginhistory.ip;
            }
        }

        public int id { get; set; }
        public int? vendorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalLogin { get; set; }
        public DateTime? loginDate { get; set; }
        public string ip { get; set; }
    }
}