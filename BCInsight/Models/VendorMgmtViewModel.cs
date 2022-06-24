using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class VendorMgmtViewModel
    {
        public VendorMgmtViewModel()
        {

        }
        public VendorMgmtViewModel(tblVendor vendor)
        {
            if (vendor != null)
            {
                id = vendor.id;
                email = vendor.email;
                firstName = vendor.firstName;
                lastName = vendor.lastName;
                password = vendor.password;
                dateCreated = vendor.dateCreated;
                lastLoginDate = vendor.lastLoginDate;
                lastLoginIp = vendor.lastLoginIp;
                isActive = vendor.isActive;
                vendorName = vendor.vendorName;
            }
        }
        public int id { get; set; }
        [Required(ErrorMessage = "Required Email")]
        [EmailAddress(ErrorMessage = "Required Valid Email Address")]
        public string email { get; set; }
        [Required(ErrorMessage = "Required FirstName")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Required LastName")]
        public string lastName { get; set; }
        public string fullname { get; set; }
        public string brandname { get; set; }
        public List<string> BrandnamesList { get; set; }        
        public string password { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastLoginDate { get; set; }
        public int? lastLoginIp { get; set; }
        public List<Vendorsalepersonviewmodel> vendosaleperson { get; set; }
        public bool isActive { get; set; }
        public int vid { get; set; }
        public string vendorName { get; set; }
    }
   
}