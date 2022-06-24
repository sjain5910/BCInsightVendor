using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class VendorportalloginViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Required Email")]
        [EmailAddress(ErrorMessage = "Required Valid Email Address")]
        public string email { get; set; }        
        public string firstName { get; set; }        
        public string lastName { get; set; }
        public string fullname { get; set; }
        public string brandname { get; set; }
        public List<string> BrandnamesList { get; set; }
        [Required(ErrorMessage = "Required Password")]
        public string password { get; set; }
        public string Oldpassword { get; set; }
        public string CNewpassword { get; set; }
        public string Newpassword { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastLoginDate { get; set; }
        public int? lastLoginIp { get; set; }
        public List<Vendorsalepersonviewmodel> vendosaleperson { get; set; }
        public bool isActive { get; set; }
        public int vid { get; set; }
        public string vendorName { get; set; }
    }
    
}