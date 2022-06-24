using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class Vendorsalepersonviewmodel
    {
        public Vendorsalepersonviewmodel()
        {

        }
        public Vendorsalepersonviewmodel(tblVendorsalesperson saleperson)
        {
            if(saleperson!=null)
            {
                Id = saleperson.Id;
                id_vendor = saleperson.Id;
                site_cuid = saleperson.site_cuid;
                department = saleperson.department;
                brand = saleperson.brand;
                sales_person_no = saleperson.sales_person_no;
                insert_date= saleperson.insert_date;
                last_update_date = saleperson.last_update_date;
            }
           
        }
        public int Id { get; set; }
        public int? id_vendor { get; set; }
        [Required(ErrorMessage = "Required siteName")]
        public string site_cuid { get; set; }
        [Required(ErrorMessage = "Required department")]
        public string department { get; set; }
        [Required(ErrorMessage = "Required brand")]
        public string brand { get; set; }
        public int? sales_person_no { get; set; }
        [Required(ErrorMessage = "Required sale person no - name")]
        public string sales_person_no_name { get; set; }
        public DateTime? insert_date { get; set; }
        public DateTime? last_update_date { get; set; }
    }
}