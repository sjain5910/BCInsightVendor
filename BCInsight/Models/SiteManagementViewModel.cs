using BCInsight.DAL;
using System.ComponentModel.DataAnnotations;

namespace BCInsight.Models
{
    public class SiteManagementViewModel
    {

        public SiteManagementViewModel()
        {

        }

        public SiteManagementViewModel(tblSite site)
        {
            SiteId = site.site_id;
            SiteName = site.site_name;
            SiteCuid = site.site_cuid;
            SiteCity = site.site_city;
            SiteLocation = site.site_location;
            SiteEmail = site.site_email;
            SiteContactNo = site.contact_no;

        }
        public int SiteId { get; set; }

        [Required(ErrorMessage = "Required Site Cuid")]
        public string SiteCuid { get; set; }

        [Required(ErrorMessage = "Required Site Name")]
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public string SiteCity { get; set; }

        [EmailAddress(ErrorMessage = "Required Valid Email Address")]
        public string SiteEmail { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Required Valid Mobile Number")]
        public string SiteContactNo { get; set; }
    }
}