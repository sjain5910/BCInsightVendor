using BCInsight.BAL.Repository;
using BCInsight.DAL;
using BCInsight.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCInsight.Controllers
{
    public class ReportController : BaseController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DAL.Vendor_bcInsightEntities entities = new DAL.Vendor_bcInsightEntities();
        private readonly ISite _site;
        public ReportController(ISite site)
        {
            _site = site;
        }

        public ActionResult Daywisesalescomparison(int? siteid, string saledate, string brands)
        {
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();

            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                BrandNamelist.Add(brandname.ToString());
                            }

                        }
                        if (BrandNamelist.Count < 1)
                        {
                            int vendorId = Convert.ToInt32(Session["id"]);
                            var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                            foreach (var brandname in _getVendorBrand)
                            {
                                BrandNamelist.Add(brandname.ToUpper());
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    daywisesale.saledate = string.Format("{0:dd-MM-yyyy}", saledate);

                    DateTime StartDate = DateTime.ParseExact(daywisesale.saledate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime PreyearStartDate = StartDate.AddYears(-1);
                    if (siteid > 0 && siteid != null)
                    {
                        daywisesale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesite(StartDate);
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    daywisesale.presaledate = string.Format("{0:dd-MM-yyyy}", PreyearStartDate);
                    List<SiteNameslist> sitenamelist = new List<SiteNameslist>();
                    try
                    {
                        var vendorsitespredate = context.SP_Vendoravailabesite(PreyearStartDate);
                        foreach (var presitename in vendorsitespredate)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    foreach (var sites in site)
                    {
                        List<BrandSaleDetails> brandsaledetailslist = new List<BrandSaleDetails>();
                        SiteNameslist sitemodel = new SiteNameslist();
                        sitemodel.siteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                int CurrSale = 0;
                                int LastSale = 0;
                                BrandSaleDetails brandsaledetails = new BrandSaleDetails();
                                brandsaledetails.Brand = vendorbrand;
                                var _CYsaledetails = context.SP_VendorTemplateDaywiseComparisionofsaleLastdate(sites.site_cuid, StartDate, vendorbrand);
                                foreach (var CDsale in _CYsaledetails)
                                {
                                    CurrSale = CDsale.saleamount;
                                }
                                brandsaledetails.CurrentDate = CurrSale;
                                var _lastyearsaledetails = context.SP_VendorTemplateDaywiseComparisionofsaleLastdate(sites.site_cuid, PreyearStartDate, vendorbrand);
                                foreach (var lastyearsale in _lastyearsaledetails)
                                {
                                    LastSale = lastyearsale.saleamount;
                                }
                                brandsaledetails.LastDate = LastSale;
                                if (CurrSale > 0 || LastSale > 0)
                                {
                                    brandsaledetails.differance = CurrSale - LastSale;
                                }
                                brandsaledetailslist.Add(brandsaledetails);
                            }
                        }
                        sitemodel.BrandSale = brandsaledetailslist;
                        sitenamelist.Add(sitemodel);
                    }
                    daywisesale.siteNames = sitenamelist;
                    daywisesale.BrandnamesList = BrandNamelist;
                    Session["daywisesaleexport"] = daywisesale;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return PartialView("~/Views/Report/_daywisesalecompairpartial.cshtml", daywisesale);
        }
        public ActionResult Daywisecomparisonofsales()
        {
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                    {
                        Value = x.site_id.ToString(),
                        Text = x.site_name
                    });
                    DateTime getdate = DateTime.Today.AddDays(-1);
                    daywisesale.saledate = string.Format("{0:dd-MM-yyyy}", getdate);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View(daywisesale);
        }
        public ActionResult ExportDaywisesale(int? siteid, string saledate, string brands)
        {
            DataTable dt = new DataTable("DaywisesaleReport");
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                BrandNamelist.Add(brandname.ToString());
                            }
                        }
                        if (BrandNamelist.Count < 1)
                        {
                            int vendorId = Convert.ToInt32(Session["id"]);
                            var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                            foreach (var brandname in _getVendorBrand)
                            {
                                BrandNamelist.Add(brandname.ToUpper());
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    if (saledate != null)
                    {
                        daywisesale.saledate = string.Format("{0:dd-MM-yyyy}", saledate);
                    }
                    else
                    {
                        DateTime getdate = DateTime.Today.AddDays(-1);
                        daywisesale.saledate = string.Format("{0:dd-MM-yyyy}", getdate);
                    }
                    DateTime StartDate = DateTime.ParseExact(daywisesale.saledate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime PreyearStartDate = StartDate.AddYears(-1);
                    if (siteid > 0 && siteid != null)
                    {
                        daywisesale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesite(StartDate);
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    daywisesale.presaledate = string.Format("{0:dd-MM-yyyy}", PreyearStartDate);
                    dt.Columns.AddRange(new DataColumn[5] {
                                            new DataColumn("Site"),
                                            new DataColumn("brand"),
                                            new DataColumn("Current Date" + daywisesale.saledate,typeof(Int32)),
                                            new DataColumn("Last Date" + daywisesale.presaledate,typeof(Int32)),
                                            new DataColumn("Differance",typeof(Int32))});
                    try
                    {
                        var vendorsitespredate = context.SP_Vendoravailabesite(PreyearStartDate);
                        foreach (var presitename in vendorsitespredate)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    foreach (var sites in site)
                    {
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                int CurrSale = 0;
                                int LastSale = 0;
                                var _CYsaledetails = context.SP_VendorTemplateDaywiseComparisionofsaleLastdate(sites.site_cuid, StartDate, vendorbrand);
                                foreach (var CDsale in _CYsaledetails)
                                {
                                    CurrSale = CDsale.saleamount;
                                }
                                var _lastyearsaledetails = context.SP_VendorTemplateDaywiseComparisionofsaleLastdate(sites.site_cuid, PreyearStartDate, vendorbrand);
                                foreach (var lastyearsale in _lastyearsaledetails)
                                {
                                    LastSale = lastyearsale.saleamount;
                                }
                                int diff = 0;
                                if (CurrSale > 0 || LastSale > 0)
                                    diff = (CurrSale - LastSale);

                                dt.Rows.Add(sites.site_name, vendorbrand, CurrSale, LastSale, diff);
                            }
                        }

                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DaywisesaleReport.xlsx");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return RedirectToAction("Daywisecomparisonofsales");
        }

        public ActionResult MonthSummarysalescomparison(int? siteid, string brands)
        {
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                BrandNamelist.Add(brandname.ToString());
                            }

                        }
                        if (BrandNamelist.Count < 1)
                        {
                            int vendorId = Convert.ToInt32(Session["id"]);
                            var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                            foreach (var brandname in _getVendorBrand)
                            {
                                BrandNamelist.Add(brandname.ToUpper());
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    DateTime todaysdate = DateTime.Now;
                    string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                    var date = today.Split('-');
                    string month = date[1];
                    int year = Convert.ToInt32(date[2]);
                    string MonthStartdate = "01-" + month + "-" + year;
                    DateTime CurrentMonthEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime CurrentMonthStartdate = DateTime.ParseExact(MonthStartdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime PreyearStartDate = CurrentMonthStartdate.AddYears(-1);
                    DateTime PreyearEndDate = PreyearStartDate.AddMonths(1);
                    PreyearEndDate = PreyearEndDate.AddDays(-1);
                    daywisesale.saledate = string.Format("{0:MMM-yyyy}", CurrentMonthStartdate);
                    if (siteid > 0 && siteid != null)
                    {
                        daywisesale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesiteformonth(CurrentMonthStartdate, CurrentMonthEnddate);
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    daywisesale.presaledate = string.Format("{0:MMM-yyyy}", PreyearStartDate);
                    List<SiteNameslist> sitenamelist = new List<SiteNameslist>();
                    try
                    {
                        var vendorsitespredate = context.SP_Vendoravailabesiteformonth(PreyearStartDate, PreyearEndDate);
                        foreach (var presitename in vendorsitespredate)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    foreach (var sites in site)
                    {
                        List<tblsale> currentsaleresult = context.tblsales.Where(x => x.siteCuid.ToLower() == sites.site_cuid.ToLower() && x.saleDate >= CurrentMonthStartdate && x.saleDate <= CurrentMonthEnddate).ToList();
                        List<tblsale> previoussaleresult = context.tblsales.Where(x => x.siteCuid.ToLower() == sites.site_cuid.ToLower() && x.saleDate >= PreyearStartDate && x.saleDate <= PreyearEndDate).ToList();
                        List<BrandSaleDetails> brandsaledetailslist = new List<BrandSaleDetails>();
                        SiteNameslist sitemodel = new SiteNameslist();
                        sitemodel.siteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                try
                                {
                                    int CurrSale = 0;
                                    int LastSale = 0;
                                    BrandSaleDetails brandsaledetails = new BrandSaleDetails();
                                    brandsaledetails.Brand = vendorbrand;
                                    var _CYsaledetails = (from c in currentsaleresult where c.brandName.ToLower() == vendorbrand.ToLower() select c.netAmt).Sum();
                                    CurrSale = Convert.ToInt32(_CYsaledetails);
                                    brandsaledetails.CurrentDate = CurrSale;
                                    var _lastyearsaledetails = (from c in previoussaleresult where c.brandName.ToLower() == vendorbrand.ToLower() select c.netAmt).Sum();
                                    LastSale = Convert.ToInt32(_lastyearsaledetails);
                                    brandsaledetails.LastDate = LastSale;
                                    if (CurrSale > 0 || LastSale > 0)
                                    {
                                        brandsaledetails.differance = CurrSale - LastSale;
                                    }
                                    brandsaledetailslist.Add(brandsaledetails);
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }
                        sitemodel.BrandSale = brandsaledetailslist;
                        sitenamelist.Add(sitemodel);
                    }
                    daywisesale.siteNames = sitenamelist;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return PartialView("~/Views/Report/_monthsummarysalescompairpartial.cshtml", daywisesale);

        }
        public ActionResult MonthSummarycomparisonofsales()
        {
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            try
            {
                ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                {
                    Value = x.site_id.ToString(),
                    Text = x.site_name
                });
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View(daywisesale);
        }
        public ActionResult ExportMonthSummarysale(int? siteid, string brands)
        {
            DataTable dt = new DataTable("MonthSummarysaleReport");
            DaywisecomparisonofsalesVendorPortalViewModel daywisesale = new DaywisecomparisonofsalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                BrandNamelist.Add(brandname.ToString());
                            }

                        }
                        if (BrandNamelist.Count < 1)
                        {
                            int vendorId = Convert.ToInt32(Session["id"]);
                            var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                            foreach (var brandname in _getVendorBrand)
                            {
                                BrandNamelist.Add(brandname.ToUpper());
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    DateTime todaysdate = DateTime.Now;
                    string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                    var date = today.Split('-');
                    string month = date[1];
                    int year = Convert.ToInt32(date[2]);
                    string MonthStartdate = "01-" + month + "-" + year;
                    DateTime CurrentMonthEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime CurrentMonthStartdate = DateTime.ParseExact(MonthStartdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime PreyearStartDate = CurrentMonthStartdate.AddYears(-1);
                    DateTime PreyearEndDate = PreyearStartDate.AddMonths(1);
                    PreyearEndDate = PreyearEndDate.AddDays(-1);
                    daywisesale.saledate = string.Format("{0:MMM-yyyy}", CurrentMonthStartdate);
                    if (siteid > 0 && siteid != null)
                    {
                        daywisesale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesiteformonth(CurrentMonthStartdate, CurrentMonthEnddate);
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    daywisesale.presaledate = string.Format("{0:MMM-yyyy}", PreyearStartDate);
                    List<SiteNameslist> sitenamelist = new List<SiteNameslist>();
                    dt.Columns.AddRange(new DataColumn[5] {
                                            new DataColumn("Site"),
                                            new DataColumn("brand"),
                                            new DataColumn("Current Date" + daywisesale.saledate,typeof(Int32)),
                                            new DataColumn("Last Date" + daywisesale.presaledate,typeof(Int32)),
                                            new DataColumn("Differance",typeof(Int32))});
                    try
                    {
                        var vendorsitespredate = context.SP_Vendoravailabesiteformonth(PreyearStartDate, PreyearEndDate);
                        foreach (var presitename in vendorsitespredate)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    foreach (var sites in site)
                    {
                        List<tblsale> currentsaleresult = context.tblsales.Where(x => x.siteCuid.ToLower() == sites.site_cuid.ToLower() && x.saleDate >= CurrentMonthStartdate && x.saleDate <= CurrentMonthEnddate).ToList();
                        List<tblsale> previoussaleresult = context.tblsales.Where(x => x.siteCuid.ToLower() == sites.site_cuid.ToLower() && x.saleDate >= PreyearStartDate && x.saleDate <= PreyearEndDate).ToList();
                        List<BrandSaleDetails> brandsaledetailslist = new List<BrandSaleDetails>();
                        SiteNameslist sitemodel = new SiteNameslist();
                        sitemodel.siteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                int CurrSale = 0;
                                int LastSale = 0;
                                var _CYsaledetails = (from c in currentsaleresult where c.brandName.ToLower() == vendorbrand.ToLower() select c.netAmt).Sum();
                                if (_CYsaledetails > 0)
                                {
                                    CurrSale = Convert.ToInt32(_CYsaledetails);
                                }
                                var _lastyearsaledetails = (from c in previoussaleresult where c.brandName.ToLower() == vendorbrand.ToLower() select c.netAmt).Sum();
                                if (_lastyearsaledetails > 0)
                                {
                                    LastSale = Convert.ToInt32(_lastyearsaledetails);
                                }
                                int diff = 0;
                                if (CurrSale > 0 || LastSale > 0)
                                {
                                    diff = (CurrSale - LastSale);
                                }
                                dt.Rows.Add(sitemodel.siteName, vendorbrand, CurrSale, LastSale, diff);
                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MonthSummarysaleReport.xlsx");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return RedirectToAction("MonthSummarycomparisonofsales");
        }

        public ActionResult ReportWeekWiseSale(int? siteid)
        {
            WeeklysalesVendorPortalViewModel Weeklysale = new WeeklysalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<DateTime> datelist = new List<DateTime>();
            List<string> labellist = new List<string>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();
            int vendorId;
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                        foreach (var brandname in _getVendorBrand)
                        {
                            BrandNamelist.Add(brandname.ToUpper());
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                    {
                        Value = x.site_id.ToString(),
                        Text = x.site_name
                    });
                    DateTime todaysdate = DateTime.Now;
                    string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                    DateTime CurrentWeekEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime CurrentWeekStartdate = CurrentWeekEnddate.AddDays(-6);
                    DateTime labeldate = CurrentWeekStartdate;
                    for (int labels = 1; labels <= 7; labels++)
                    {
                        labellist.Add(string.Format("{0:dd-MMM}", labeldate));
                        labeldate = labeldate.AddDays(1);
                    }
                    Weeklysale.label = labellist;
                    if (siteid > 0 && siteid != null)
                    {
                        Weeklysale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesiteformonth(CurrentWeekStartdate, CurrentWeekEnddate);
                        //List<tblsale> getallsite = context.tblsales.Where(x => x.saleDate >= CurrentWeekStartdate && x.saleDate <= CurrentWeekEnddate).ToList();
                        //var vendorsites = (from c in getallsite select c.siteCuid).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    List<SiteNamesweeklylist> sitenamelist = new List<SiteNamesweeklylist>();

                    foreach (var sites in site)
                    {

                        List<BrandSaleWeeklyDetails> brandsaledetailslist = new List<BrandSaleWeeklyDetails>();
                        SiteNamesweeklylist sitemodel = new SiteNamesweeklylist();
                        sitemodel.siteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<int> amountlist = new List<int>();
                                int CurrSale = 0;
                                int LastSale = 0;
                                List<salehistory> salehistorylist = new List<salehistory>();
                                BrandSaleWeeklyDetails brandsaledetails = new BrandSaleWeeklyDetails();
                                brandsaledetails.Brand = vendorbrand;
                                DateTime processdate = CurrentWeekStartdate;
                                var _CYsaledetails = context.SP_VendorTemplateweeklysalereport(sites.site_cuid, vendorbrand, CurrentWeekStartdate, CurrentWeekEnddate);
                                foreach (var CDsale in _CYsaledetails)
                                {
                                    CurrSale = CurrSale + CDsale.saleamount;
                                    salehistory sales = new salehistory();
                                    sales.saledate = CDsale.saleDate;
                                    sales.amount = CDsale.saleamount;
                                    salehistorylist.Add(sales);
                                }
                                if (salehistorylist.Count > 0)
                                {
                                    foreach (var history in salehistorylist)
                                    {
                                        Restart:
                                        if (history.saledate == processdate)
                                        {
                                            amountlist.Add(history.amount);
                                        }
                                        else
                                        {
                                            amountlist.Add(0);
                                            processdate = processdate.AddDays(1);
                                            goto Restart;
                                        }
                                    }
                                }
                                Recheck:
                                if (amountlist.Count < 7)
                                {
                                    amountlist.Add(0);
                                    goto Recheck;
                                }
                                brandsaledetails.daywisesale = amountlist;
                                brandsaledetails.Total = CurrSale;
                                brandsaledetailslist.Add(brandsaledetails);
                            }
                        }
                        sitemodel.BrandSale = brandsaledetailslist;
                        sitenamelist.Add(sitemodel);
                    }
                    Weeklysale.siteNames = sitenamelist;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View(Weeklysale);
        }
        public ActionResult ExportWeekWiseSale(int? siteid)
        {
            DataTable dt = new DataTable("WeeklysaleReportVendor");
            WeeklysalesVendorPortalViewModel Weeklysale = new WeeklysalesVendorPortalViewModel();
            List<tblSite> site = new List<tblSite>();
            List<DateTime> datelist = new List<DateTime>();
            List<string> labellist = new List<string>();
            List<string> BrandNamelist = new List<string>();
            List<string> vendorsitelist = new List<string>();
            int vendorId;
            try
            {
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        var _getVendorBrand = context.SP_VendorTemplateGetVendorBrand(vendorId);
                        foreach (var brandname in _getVendorBrand)
                        {
                            BrandNamelist.Add(brandname.ToUpper());
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    DateTime todaysdate = DateTime.Now;
                    string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                    DateTime CurrentWeekEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime CurrentWeekStartdate = CurrentWeekEnddate.AddDays(-6);
                    DateTime labeldate = CurrentWeekStartdate;
                    for (int labels = 1; labels <= 7; labels++)
                    {
                        labellist.Add(string.Format("{0:dd-MMM-yyyy}", labeldate));
                        labeldate = labeldate.AddDays(1);
                    }
                    Weeklysale.label = labellist;
                    if (siteid > 0 && siteid != null)
                    {
                        Weeklysale.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.SP_Vendoravailabesiteformonth(CurrentWeekStartdate, CurrentWeekEnddate);
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    List<SiteNamesweeklylist> sitenamelist = new List<SiteNamesweeklylist>();
                    foreach (var sites in site)
                    {
                        List<BrandSaleWeeklyDetails> brandsaledetailslist = new List<BrandSaleWeeklyDetails>();
                        SiteNamesweeklylist sitemodel = new SiteNamesweeklylist();
                        sitemodel.siteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<int> amountlist = new List<int>();
                                int CurrSale = 0;
                                List<salehistory> salehistorylist = new List<salehistory>();
                                BrandSaleWeeklyDetails brandsaledetails = new BrandSaleWeeklyDetails();
                                brandsaledetails.Brand = vendorbrand;
                                DateTime processdate = CurrentWeekStartdate;
                                var _CYsaledetails = context.SP_VendorTemplateweeklysalereport(sites.site_cuid, vendorbrand, CurrentWeekStartdate, CurrentWeekEnddate);
                                foreach (var CDsale in _CYsaledetails)
                                {
                                    CurrSale = CurrSale + CDsale.saleamount;
                                    salehistory sales = new salehistory();
                                    sales.saledate = CDsale.saleDate;
                                    sales.amount = CDsale.saleamount;
                                    salehistorylist.Add(sales);
                                }
                                if (salehistorylist.Count > 0)
                                {
                                    foreach (var history in salehistorylist)
                                    {
                                        Restart:
                                        if (history.saledate == processdate)
                                        {
                                            amountlist.Add(history.amount);
                                        }
                                        else
                                        {
                                            amountlist.Add(0);
                                            processdate = processdate.AddDays(1);
                                            goto Restart;
                                        }
                                    }
                                }
                                Recheck:
                                if (amountlist.Count < 7)
                                {
                                    amountlist.Add(0);
                                    goto Recheck;
                                }
                                brandsaledetails.daywisesale = amountlist;
                                brandsaledetails.Total = CurrSale;
                                brandsaledetailslist.Add(brandsaledetails);
                            }
                        }
                        sitemodel.BrandSale = brandsaledetailslist;
                        sitenamelist.Add(sitemodel);
                    }
                    dt.Columns.AddRange(new DataColumn[10] {
                                            new DataColumn("Site"),
                                            new DataColumn("brand"),
                                            new DataColumn(labellist[0]),
                                            new DataColumn(labellist[1]),
                                            new DataColumn(labellist[2]),
                                            new DataColumn(labellist[3]),
                                            new DataColumn(labellist[4]),
                                            new DataColumn(labellist[5]),
                                            new DataColumn(labellist[6]),
                                            new DataColumn("Total")});
                    foreach (var result in sitenamelist)
                    {
                        if (result.BrandSale.Count > 0)
                        {
                            foreach (var brandresult in result.BrandSale)
                            {
                                string D1;
                                if (brandresult.daywisesale[0] > 0)
                                {
                                    D1 = brandresult.daywisesale[0].ToString();
                                }
                                else
                                {
                                    D1 = "NA";
                                }
                                string D2;
                                if (brandresult.daywisesale[1] > 0)
                                {
                                    D2 = brandresult.daywisesale[1].ToString();
                                }
                                else
                                {
                                    D2 = "NA";
                                }
                                string D3;
                                if (brandresult.daywisesale[2] > 0)
                                {
                                    D3 = brandresult.daywisesale[2].ToString();
                                }
                                else
                                {
                                    D3 = "NA";
                                }
                                string D4;
                                if (brandresult.daywisesale[3] > 0)
                                {
                                    D4 = brandresult.daywisesale[3].ToString();
                                }
                                else
                                {
                                    D4 = "NA";
                                }
                                string D5;
                                if (brandresult.daywisesale[4] > 0)
                                {
                                    D5 = brandresult.daywisesale[4].ToString();
                                }
                                else
                                {
                                    D5 = "NA";
                                }
                                string D6;
                                if (brandresult.daywisesale[5] > 0)
                                {
                                    D6 = brandresult.daywisesale[5].ToString();
                                }
                                else
                                {
                                    D6 = "NA";
                                }
                                string D7;
                                if (brandresult.daywisesale[6] > 0)
                                {
                                    D7 = brandresult.daywisesale[6].ToString();
                                }
                                else
                                {
                                    D7 = "NA";
                                }
                                string total;
                                if (brandresult.Total > 0)
                                {
                                    total = brandresult.Total.ToString();
                                }
                                else
                                {
                                    total = "-";
                                }
                                dt.Rows.Add(result.siteName, brandresult.Brand, D1, D2, D3, D4, D5, D6, D7, total);
                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WeeklysaleReportVendor.xlsx");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return RedirectToAction("ReportWeekWiseSale");
        }

        public ActionResult getStockReport(int? siteid, string stockdate, string brands)
        {
            vendorPortalStockReportViewModel stockReportModel = new vendorPortalStockReportViewModel();
            List<sitelistmodel> sitedetailsmodellist = new List<sitelistmodel>();
            List<tblSite> site = new List<tblSite>();
            List<tblBrand> BrandNamelist = new List<tblBrand>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                stockReportModel.stockdate = string.Format("{0:dd-MM-yyyy}", stockdate);
                DateTime startDate = DateTime.ParseExact(stockReportModel.stockdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                stockReportModel.dispDate = string.Format("{0:dd-MMM-yyyy}", startDate);
                DateTime endDate = startDate.AddDays(1);
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblDepartment> AllDept = context.tblDepartments.OrderBy(x => x.departmentName).ToList();
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                        if (BrandNamelist.Count < 1)
                        {
                            vendorId = Convert.ToInt32(Session["id"]);
                            List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                            foreach (var brandname in _getVendorBrand)
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                                if (assignbrand != null)
                                {
                                    BrandNamelist.Add(assignbrand);
                                }
                            }
                        }

                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    if (siteid > 0 && siteid != null)
                    {
                        stockReportModel.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate).Select(x => x.site_id).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_id == sitename select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    foreach (var sites in site)
                    {

                        sitelistmodel sitedetailsmodel = new sitelistmodel();
                        List<Brandsviewmodel> brandlistdetails = new List<Brandsviewmodel>();
                        sitedetailsmodel.SiteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            List<tblstock> Stockresult = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate && x.site_id == sites.site_id).ToList();
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<int> departlist = new List<int>();
                                Brandsviewmodel stockbrand = new Brandsviewmodel();
                                List<Departmentsviewmodel> stockdeptlist = new List<Departmentsviewmodel>();
                                stockbrand.BrandName = vendorbrand.brandName;
                                List<tblstock> stockqty = (from c in Stockresult where c.brand_id == vendorbrand.brand_id && c.site_id == sites.site_id select c).ToList();
                                int avlstock = (from c in Stockresult where c.brand_id == vendorbrand.brand_id && c.site_id == sites.site_id select c.closingTotal).Sum();
                                stockbrand.Quantity = avlstock;
                                if (stockqty.Count > 0)
                                {
                                    foreach (var dept in stockqty)
                                    {
                                        if (!departlist.Exists(p => p.Equals(dept.dep_id)))
                                        {
                                            departlist.Add(dept.dep_id);
                                            tblDepartment deptname = (from c in AllDept where c.dep_id == dept.dep_id select c).FirstOrDefault();
                                            if (deptname != null)
                                            {
                                                Departmentsviewmodel stockdept = new Departmentsviewmodel();
                                                stockdept.Department = deptname.departmentName;
                                                //List<tblstock> deptqty = (from c in stockqty where c.dep_id == dept.dep_id select c).ToList();
                                                int avldept = (from c in stockqty where c.dep_id == dept.dep_id select c.closingTotal).Sum();
                                                stockdept.DeptQty = avldept;
                                                stockdeptlist.Add(stockdept);
                                            }
                                        }
                                    }
                                }
                                stockbrand.departmentslist = stockdeptlist;
                                brandlistdetails.Add(stockbrand);
                            }
                        }
                        sitedetailsmodel.brandslist = brandlistdetails;
                        sitedetailsmodellist.Add(sitedetailsmodel);

                    }
                    stockReportModel.Sitelist = sitedetailsmodellist;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return PartialView("~/Views/Report/_partialstockreport.cshtml", stockReportModel);
        }
        public ActionResult StockReport()
        {
            vendorPortalStockReportViewModel stockReportModel = new vendorPortalStockReportViewModel();
            try
            {
                DateTime getdate = DateTime.Today.AddDays(-1);
                stockReportModel.stockdate = string.Format("{0:dd-MM-yyyy}", getdate);

                ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                {
                    Value = x.site_id.ToString(),
                    Text = x.site_name
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return View(stockReportModel);
        }
        public ActionResult ExportStockReport(int? siteid, string stockdate, string brands)
        {
            DataTable dt = new DataTable("StockReportVendor");
            dt.Columns.AddRange(new DataColumn[4] {
                                            new DataColumn("Sitename"),
                                            new DataColumn("Brand"),
                                            new DataColumn("Department"),
                                            new DataColumn("Pcs",typeof(Int32))});
            vendorPortalStockReportViewModel stockReportModel = new vendorPortalStockReportViewModel();
            List<sitelistmodel> sitedetailsmodellist = new List<sitelistmodel>();
            List<tblSite> site = new List<tblSite>();
            List<tblBrand> BrandNamelist = new List<tblBrand>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                DateTime startDate = DateTime.ParseExact(stockdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //string stdate = string.Format("{0:dd-MM-yyyy 23:59:59}", endDate);
                DateTime endDate = startDate.AddDays(1);
                // DateTime startDate = DateTime.ParseExact(stockReportModel.stockdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblDepartment> AllDept = context.tblDepartments.OrderBy(x => x.departmentName).ToList();
                    brands = brands.Replace("A_N_D", "&");
                    string[] element = brands.ToString().Split(',');
                    foreach (var brandname in element)
                    {
                        if (!string.IsNullOrEmpty(brandname))
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                            if (assignbrand != null)
                                BrandNamelist.Add(assignbrand);
                        }
                    }
                    if (BrandNamelist.Count < 1)
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                        foreach (var brandname in _getVendorBrand)
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                            if (assignbrand != null)
                                BrandNamelist.Add(assignbrand);

                        }
                    }
                    if (siteid > 0 && siteid != null)
                    {
                        stockReportModel.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate).Select(x => x.site_id).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_id == sitename select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    foreach (var sites in site)
                    {
                        sitelistmodel sitedetailsmodel = new sitelistmodel();
                        List<Brandsviewmodel> brandlistdetails = new List<Brandsviewmodel>();
                        sitedetailsmodel.SiteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            List<tblstock> Stockresult = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate && x.site_id == sites.site_id).ToList();
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<int> departlist = new List<int>();
                                Brandsviewmodel stockbrand = new Brandsviewmodel();
                                List<Departmentsviewmodel> stockdeptlist = new List<Departmentsviewmodel>();
                                stockbrand.BrandName = vendorbrand.brandName;
                                List<tblstock> stockqty = (from c in Stockresult where c.brand_id == vendorbrand.brand_id && c.site_id == sites.site_id select c).ToList();
                                int avlstock = (from c in Stockresult where c.brand_id == vendorbrand.brand_id && c.site_id == sites.site_id select c.closingTotal).Sum();
                                stockbrand.Quantity = avlstock;
                                if (stockqty.Count > 0)
                                {
                                    foreach (var dept in stockqty)
                                    {
                                        if (!departlist.Exists(p => p.Equals(dept.dep_id)))
                                        {
                                            departlist.Add(dept.dep_id);
                                            tblDepartment deptname = (from c in AllDept where c.dep_id == dept.dep_id select c).FirstOrDefault();
                                            if (deptname != null)
                                            {
                                                Departmentsviewmodel stockdept = new Departmentsviewmodel();
                                                stockdept.Department = deptname.departmentName;
                                                int avldept = (from c in stockqty where c.dep_id == dept.dep_id select c.closingTotal).Sum();
                                                stockdept.DeptQty = avldept;
                                                dt.Rows.Add(sitedetailsmodel.SiteName, stockbrand.BrandName, stockdept.Department, stockdept.DeptQty);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockReportVendor.xlsx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return RedirectToAction("StockReport");
        }

        public ActionResult getDailysalereport(string Startdate, string Enddate, string brands)
        {
            vendorDailySaleReportviewModel dailySaleReportModel = new vendorDailySaleReportviewModel();
            List<vendorDailySalebrand> vendorbrandlist = new List<vendorDailySalebrand>();
            List<tblBrand> BrandNamelist = new List<tblBrand>();
            try
            {
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblSite> sitenamelist = context.tblSites.OrderBy(x => x.site_id).ToList();
                    DateTime Fdate = DateTime.Now;
                    DateTime Ldate = DateTime.Now;
                    if (Startdate != null && Enddate != null)
                    {
                        Fdate = DateTime.ParseExact(Startdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.StartDate = string.Format("{0:dd-MM-yyyy}", Fdate);
                        Ldate = DateTime.ParseExact(Enddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.EndDate = string.Format("{0:dd-MM-yyyy}", Ldate);
                        dailySaleReportModel.processdate = string.Format("{0:dd-MM-yyyy}", Fdate) + " To " + string.Format("{0:dd-MM-yyyy}", Ldate);
                    }
                    else
                    {
                        DateTime date = DateTime.Now;
                        dailySaleReportModel.StartDate = string.Format("{0:dd-MM-yyyy}", date);
                        Fdate = DateTime.ParseExact(dailySaleReportModel.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.EndDate = string.Format("{0:dd-MM-yyyy}", date);
                        Ldate = DateTime.ParseExact(dailySaleReportModel.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.processdate = string.Format("{0:dd-MM-yyyy}", Fdate);
                    }
                    try
                    {
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                        if (BrandNamelist.Count < 1)
                        {
                            vendorId = Convert.ToInt32(Session["id"]);
                            List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                            foreach (var brandname in _getVendorBrand)
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                                if (assignbrand != null)
                                {
                                    BrandNamelist.Add(assignbrand);
                                }
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Logout", "Login");
                    }

                    foreach (var brandname in BrandNamelist)
                    {
                        List<salesviewmodel> saledetailslist = new List<salesviewmodel>();
                        vendorDailySalebrand vendorbrand = new vendorDailySalebrand();
                        vendorbrand.Brandname = brandname.brandName;
                        List<tblsale> salelist = context.tblsales.Where(x => x.saleDate >= Fdate && x.saleDate <= Ldate && x.brandName.ToLower() == brandname.brandName.ToLower()).OrderBy(x => x.saleDate).ToList();
                        foreach (var item in salelist)
                        {
                            var sitename = (from c in sitenamelist where c.site_cuid.ToLower() == item.siteCuid.ToLower() select c).FirstOrDefault();

                            salesviewmodel saledetails = new salesviewmodel();
                            saledetails.saleDate = item.saleDate;
                            if (sitename != null)
                            {
                                saledetails.siteName = sitename.site_name;
                            }
                            saledetails.department = item.department;
                            saledetails.cat2 = item.cat2;
                            saledetails.billNo = item.billNo;
                            saledetails.section = item.section;
                            saledetails.product = item.product;
                            saledetails.cat6code = item.cat6;
                            saledetails.ItemDesc6 = item.ItemDesc6;
                            saledetails.cat3color = item.cat3;
                            saledetails.cat4size = item.cat4;
                            saledetails.saleQty = item.saleQty;
                            saledetails.mrpAmt = item.mrpAmt;
                            saledetails.PrmoAmt = item.PrmoAmt;
                            saledetails.ItemDiscountAmt = item.ItemDiscountAmt;
                            saledetails.BillDiscountAmt = item.BillDiscountAmt;
                            saledetails.LPDiscountAmt = item.LPDiscountAmt;
                            saledetails.ExTaxAmtFactor = item.ExTaxAmtFactor;
                            saledetails.netAmt = item.netAmt;
                            saledetailslist.Add(saledetails);
                        }
                        vendorbrand.salelist = saledetailslist;
                        vendorbrandlist.Add(vendorbrand);
                    }
                    dailySaleReportModel.brandlist = vendorbrandlist;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return PartialView("~/Views/Report/_dailysalereportPartial.cshtml", dailySaleReportModel);

        }
        public ActionResult Dailysalereport(string Startdate, string Enddate)
        {
            vendorDailySaleReportviewModel dailySaleReportModel = new vendorDailySaleReportviewModel();

            try
            {
                DateTime date = DateTime.Now;
                dailySaleReportModel.StartDate = string.Format("{0:dd-MM-yyyy}", date);
                dailySaleReportModel.EndDate = string.Format("{0:dd-MM-yyyy}", date);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return View(dailySaleReportModel);
        }
        public ActionResult ExportDailysalereport(string Startdate, string Enddate, string brands)
        {
            vendorDailySaleReportviewModel dailySaleReportModel = new vendorDailySaleReportviewModel();
            List<vendorDailySalebrand> vendorbrandlist = new List<vendorDailySalebrand>();
            List<tblBrand> BrandNamelist = new List<tblBrand>();
            DataTable dt = new DataTable("DailysaleReportVendor");
            dt.Columns.AddRange(new DataColumn[20] {
                                            new DataColumn("BRAND"),
                                            new DataColumn("DATE"),
                                            new DataColumn("SITE NAME"),
                                            new DataColumn("DIVISION"),
                                            new DataColumn("CAT2"),
                                            new DataColumn("BILL NO"),
                                            new DataColumn("SECTION"),
                                            new DataColumn("PRODUCT"),
                                            new DataColumn("CODE"),
                                            new DataColumn("ITEMDESC6"),
                                            new DataColumn("COLOR"),
                                            new DataColumn("SIZE"),
                                            new DataColumn("SALE QTY",typeof(Int32)),
                                            new DataColumn("MRPAMT(A)",typeof(decimal)),
                                            new DataColumn("PRMOAMT(B)",typeof(decimal)),
                                            new DataColumn("ITEMDISCOUNTAMT(C)",typeof(decimal)),
                                            new DataColumn("BILLDISCOUNTAMT(D)",typeof(decimal)),
                                            new DataColumn("LPDISCOUNTAMT(E)",typeof(decimal)),
                                            new DataColumn("EXTAXAMT FACTOR(F)",typeof(decimal)),
                                            new DataColumn("Net Amt",typeof(decimal))});
            try
            {
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblSite> sitenamelist = context.tblSites.OrderBy(x => x.site_id).ToList();
                    DateTime Fdate = DateTime.Now;
                    DateTime Ldate = DateTime.Now;
                    if (Startdate != null && Enddate != null)
                    {
                        Fdate = DateTime.ParseExact(Startdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.StartDate = string.Format("{0:dd-MM-yyyy}", Fdate);
                        Ldate = DateTime.ParseExact(Enddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.EndDate = string.Format("{0:dd-MM-yyyy}", Ldate);
                        dailySaleReportModel.processdate = string.Format("{0:dd-MM-yyyy}", Fdate) + "To" + string.Format("{0:dd-MM-yyyy}", Ldate);
                    }
                    else
                    {
                        DateTime date = DateTime.Now;
                        dailySaleReportModel.StartDate = string.Format("{0:dd-MM-yyyy}", date);
                        Fdate = DateTime.ParseExact(dailySaleReportModel.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.EndDate = string.Format("{0:dd-MM-yyyy}", date);
                        Ldate = DateTime.ParseExact(dailySaleReportModel.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dailySaleReportModel.processdate = string.Format("{0:dd-MM-yyyy}", Fdate);
                    }


                    brands = brands.Replace("A_N_D", "&");
                    string[] element = brands.ToString().Split(',');
                    foreach (var brandname in element)
                    {
                        if (!string.IsNullOrEmpty(brandname))
                        {                            
                               tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                            BrandNamelist.Add(assignbrand);
                        }
                    }
                    if (BrandNamelist.Count < 1)
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                        foreach (var brandname in _getVendorBrand)
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                            if (assignbrand != null)
                            {
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                    }
                    foreach (var brandname in BrandNamelist)
                    {
                        List<tblsale> salelist = context.tblsales.Where(x => x.saleDate >= Fdate && x.saleDate <= Ldate && x.brandName.ToLower() == brandname.brandName.ToLower()).OrderBy(x => x.saleDate).ToList();
                        foreach (var item in salelist)
                        {
                            if (string.IsNullOrEmpty(item.ItemDiscountAmt))
                            {
                                item.ItemDiscountAmt = "0";
                            }
                            if (string.IsNullOrEmpty(item.BillDiscountAmt))
                            {
                                item.BillDiscountAmt = "0";
                            }
                            if (string.IsNullOrEmpty(item.LPDiscountAmt))
                            {
                                item.LPDiscountAmt = "0";
                            }
                            if (string.IsNullOrEmpty(item.ExTaxAmtFactor))
                            {
                                item.ExTaxAmtFactor = "0";
                            }
                            if (string.IsNullOrEmpty(item.PrmoAmt))
                            {
                                item.PrmoAmt = "0";
                            }
                            var sitename = (from c in sitenamelist where c.site_cuid.ToLower() == item.siteCuid.ToLower() select c).FirstOrDefault();
                            string saledate = string.Format("{0:dd-MM-yyyy}", item.saleDate);
                            salesviewmodelexport saledetails = new salesviewmodelexport();
                            if (sitename != null)
                            {
                                dt.Rows.Add(brandname.brandName, saledate, sitename.site_name, item.department, item.cat2, item.billNo, item.section, item.product, item.cat6, item.ItemDesc6, item.cat3, item.cat4, item.saleQty, item.mrpAmt, item.PrmoAmt, item.ItemDiscountAmt, item.BillDiscountAmt, item.LPDiscountAmt, item.ExTaxAmtFactor, item.netAmt);
                            }
                        }

                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DailysaleReportVendor.xlsx");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return RedirectToAction("Dailysalereport");
        }
        public ActionResult getReportWeekSaleCompare(int? siteid, string brands)
        {
            WeeklysalescompareViewModel Weeklysale = new WeeklysalescompareViewModel();
            try
            {
                List<weeklysalesitelist> sitemodellist = new List<weeklysalesitelist>();
                DateTime todaysdate = DateTime.Now;
                List<labesmodel> labellist = new List<labesmodel>();
                List<string> vendorsitelist = new List<string>();
                List<tblSite> site = new List<tblSite>();
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                DateTime CurrentWeekEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime CurrentWeekStartdate = CurrentWeekEnddate.AddDays(-6);
                DateTime preyearWeekEnddate = CurrentWeekEnddate.AddYears(-1);
                DateTime preyearWeekStartdate = preyearWeekEnddate.AddDays(-6);
                Weeklysale.preyear = string.Format("{0:yyyy}", preyearWeekEnddate);
                Weeklysale.curryear = string.Format("{0:yyyy}", CurrentWeekEnddate);
                DateTime labeldate = CurrentWeekStartdate;
                for (int labels = 1; labels <= 7; labels++)
                {
                    labesmodel datelabel = new labesmodel();
                    datelabel.datelist = string.Format("{0:dd-MMM}", labeldate);
                    labellist.Add(datelabel);
                    labeldate = labeldate.AddDays(1);
                }
                Weeklysale.labels = labellist;
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    try
                    {
                        List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                        brands = brands.Replace("A_N_D", "&");
                        string[] element = brands.ToString().Split(',');
                        foreach (var brandname in element)
                        {
                            if (!string.IsNullOrEmpty(brandname))
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                        if (BrandNamelist.Count < 1)
                        {
                            vendorId = Convert.ToInt32(Session["id"]);
                            List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                            foreach (var brandname in _getVendorBrand)
                            {
                                tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                                if (assignbrand != null)
                                {
                                    BrandNamelist.Add(assignbrand);
                                }
                            }
                        }
                        if (siteid > 0 && siteid != null)
                        {
                            Weeklysale.siteid = siteid.ToString();
                            site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                            foreach (var sitename in site)
                            {
                                vendorsitelist.Add(sitename.site_name);
                            }
                        }
                        else
                        {
                            site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                            var vendorsites = context.tblsales.Where(x => x.saleDate >= CurrentWeekStartdate && x.saleDate <= CurrentWeekEnddate).Select(x => x.siteCuid).Distinct().ToList();
                            foreach (var sitename in vendorsites)
                            {
                                var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                                if (getsitename != null)
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }
                            }
                            var vendorpresites = context.tblsales.Where(x => x.saleDate >= preyearWeekStartdate && x.saleDate <= preyearWeekEnddate).Select(x => x.siteCuid).Distinct().ToList();
                            foreach (var presitename in vendorpresites)
                            {
                                var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                                if (getsitename != null)
                                {
                                    if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                    {
                                        vendorsitelist.Add(getsitename.site_name);
                                    }

                                }
                            }
                        }
                        foreach (var sites in site)
                        {
                            try
                            {
                                weeklysalesitelist sitemodel = new weeklysalesitelist();
                                List<saledetailssitewise> Brandwisesalelist = new List<saledetailssitewise>();
                                sitemodel.siteName = sites.site_name;
                                if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                                {
                                    List<tblsale> curryearsaledetails = context.tblsales.Where(x => x.saleDate >= CurrentWeekStartdate && x.saleDate <= CurrentWeekEnddate && x.siteCuid.ToLower() == sites.site_cuid.ToLower()).ToList();
                                    List<tblsale> preyearsaledetails = context.tblsales.Where(x => x.saleDate >= preyearWeekStartdate && x.saleDate <= preyearWeekEnddate && x.siteCuid.ToLower() == sites.site_cuid.ToLower()).ToList();
                                    foreach (var vendorbrand in BrandNamelist)
                                    {
                                        List<tblsale> preyearsalebrandwise = (from c in preyearsaledetails where c.brandName.ToLower() == vendorbrand.brandName.ToLower() select c).ToList();
                                        saledetailssitewise brandsalemodel = new saledetailssitewise();
                                        List<saledetailsyearwise> daywisesalelist = new List<saledetailsyearwise>();
                                        brandsalemodel.BrandName = vendorbrand.brandName;
                                        DateTime curyrdate = CurrentWeekStartdate;
                                        DateTime preyrdate = preyearWeekStartdate;
                                        for (int labels = 1; labels <= 7; labels++)
                                        {
                                            var CYdatewisesale = (from c in curryearsaledetails where c.brandName.ToLower() == vendorbrand.brandName.ToLower() && c.saleDate == curyrdate select c.netAmt).Sum();
                                            var PYdatewisesale = (from c in preyearsalebrandwise where c.brandName.ToLower() == vendorbrand.brandName.ToLower() && c.saleDate == preyrdate select c.netAmt).Sum();
                                            saledetailsyearwise daywisesale = new saledetailsyearwise();
                                            daywisesale.curryearsale = Convert.ToInt32(CYdatewisesale);
                                            daywisesale.preyearsale = Convert.ToInt32(PYdatewisesale);
                                            daywisesale.Diff = (daywisesale.curryearsale - daywisesale.preyearsale);
                                            curyrdate = curyrdate.AddDays(1);
                                            preyrdate = preyrdate.AddDays(1);
                                            daywisesalelist.Add(daywisesale);
                                        }
                                        brandsalemodel.DaywiseSale = daywisesalelist;
                                        Brandwisesalelist.Add(brandsalemodel);
                                    }
                                }
                                sitemodel.sitewisesale = Brandwisesalelist;
                                sitemodellist.Add(sitemodel);
                            }
                            catch (Exception e)
                            {

                            }

                        }
                        Weeklysale.sites = sitemodellist;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return PartialView("~/Views/Report/_weeksalecompairpartial.cshtml", Weeklysale);
        }
        public ActionResult ReportWeekSaleCompare()
        {
            WeeklysalescompareViewModel Weeklysale = new WeeklysalescompareViewModel();
            try
            {
                ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                {
                    Value = x.site_id.ToString(),
                    Text = x.site_name
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return View(Weeklysale);
        }
        public ActionResult ExportWeekSaleCompare(int? siteid, string brands)
        {
            try
            {
                DateTime todaysdate = DateTime.Now;
                List<string> labellist = new List<string>();
                List<string> vendorsitelist = new List<string>();
                List<tblSite> site = new List<tblSite>();
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                string today = string.Format("{0:dd-MM-yyyy}", todaysdate);
                DateTime CurrentWeekEnddate = DateTime.ParseExact(today, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime CurrentWeekStartdate = CurrentWeekEnddate.AddDays(-6);
                DateTime preyearWeekEnddate = CurrentWeekEnddate.AddYears(-1);
                DateTime preyearWeekStartdate = preyearWeekEnddate.AddDays(-6);
                DateTime labeldate = CurrentWeekStartdate;
                DateTime prlabeldate = preyearWeekStartdate;
                for (int labels = 1; labels <= 7; labels++)
                {
                    string datelabel;
                    datelabel = string.Format("{0:dd-MMM-yyyy}", labeldate);
                    labellist.Add(datelabel);
                    datelabel = string.Format("{0:dd-MMM-yyyy}", prlabeldate);
                    labellist.Add(datelabel);
                    labellist.Add("Diff" + labels);
                    labeldate = labeldate.AddDays(1);
                    prlabeldate = prlabeldate.AddDays(1);
                }
                DataTable dt = new DataTable("WeeklysalecompareReport");
                dt.Columns.AddRange(new DataColumn[23] {
                                            new DataColumn("Sitename"),
                                            new DataColumn("Brand"),
                                            new DataColumn(labellist[0],typeof(Int32)),
                                            new DataColumn(labellist[1],typeof(Int32)),
                                            new DataColumn(labellist[2],typeof(Int32)),
                                            new DataColumn(labellist[3],typeof(Int32)),
                                            new DataColumn(labellist[4],typeof(Int32)),
                                            new DataColumn(labellist[5],typeof(Int32)),
                                            new DataColumn(labellist[6],typeof(Int32)),
                                            new DataColumn(labellist[7],typeof(Int32)),
                                            new DataColumn(labellist[8],typeof(Int32)),
                                            new DataColumn(labellist[9],typeof(Int32)),
                                            new DataColumn(labellist[10],typeof(Int32)),
                                            new DataColumn(labellist[11],typeof(Int32)),
                                            new DataColumn(labellist[12],typeof(Int32)),
                                            new DataColumn(labellist[13],typeof(Int32)),
                                            new DataColumn(labellist[14],typeof(Int32)),
                                            new DataColumn(labellist[15],typeof(Int32)),
                                            new DataColumn(labellist[16],typeof(Int32)),
                                            new DataColumn(labellist[17],typeof(Int32)),
                                            new DataColumn(labellist[18],typeof(Int32)),
                                            new DataColumn(labellist[19],typeof(Int32)),
                                            new DataColumn(labellist[20],typeof(Int32))});
                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    brands = brands.Replace("A_N_D", "&");
                    string[] element = brands.ToString().Split(',');
                    foreach (var brandname in element)
                    {
                        if (!string.IsNullOrEmpty(brandname))
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                            BrandNamelist.Add(assignbrand);
                        }
                    }
                    if (BrandNamelist.Count < 1)
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                        foreach (var brandname in _getVendorBrand)
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                            if (assignbrand != null)
                            {
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                    }
                    if (siteid > 0 && siteid != null)
                    {
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.tblsales.Where(x => x.saleDate >= CurrentWeekStartdate && x.saleDate <= CurrentWeekEnddate).Select(x => x.siteCuid).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == sitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                        var vendorpresites = context.tblsales.Where(x => x.saleDate >= preyearWeekStartdate && x.saleDate <= preyearWeekEnddate).Select(x => x.siteCuid).Distinct().ToList();
                        foreach (var presitename in vendorpresites)
                        {
                            var getsitename = (from c in site where c.site_cuid.ToLower() == presitename.ToLower() select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                if (!vendorsitelist.Exists(p => p.Equals(getsitename.site_name)))
                                {
                                    vendorsitelist.Add(getsitename.site_name);
                                }

                            }
                        }
                    }
                    foreach (var sites in site)
                    {
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            List<tblsale> curryearsaledetails = context.tblsales.Where(x => x.saleDate >= CurrentWeekStartdate && x.saleDate <= CurrentWeekEnddate && x.siteCuid.ToLower() == sites.site_cuid.ToLower()).ToList();
                            List<tblsale> preyearsaledetails = context.tblsales.Where(x => x.saleDate >= preyearWeekStartdate && x.saleDate <= preyearWeekEnddate && x.siteCuid.ToLower() == sites.site_cuid.ToLower()).ToList();
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<int> salelist = new List<int>();
                                DateTime curyrdate = CurrentWeekStartdate;
                                DateTime preyrdate = preyearWeekStartdate;
                                for (int labels = 1; labels <= 7; labels++)
                                {
                                    int CYsale = 0;
                                    int PYsale = 0;
                                    var CYdatewisesale = (from c in curryearsaledetails where c.brandName.ToLower() == vendorbrand.brandName.ToLower() && c.saleDate == curyrdate select c.netAmt).Sum();
                                    var PYdatewisesale = (from c in preyearsaledetails where c.brandName.ToLower() == vendorbrand.brandName.ToLower() && c.saleDate == preyrdate select c.netAmt).Sum();
                                    int curryearsale = Convert.ToInt32(CYdatewisesale);
                                    int preyearsale = Convert.ToInt32(PYdatewisesale);
                                    int Diff = (curryearsale - preyearsale);
                                    curyrdate = curyrdate.AddDays(1);
                                    preyrdate = preyrdate.AddDays(1);
                                    if (curryearsale > 0 || curryearsale < 0)
                                    {
                                        CYsale = curryearsale;
                                    }
                                    salelist.Add(CYsale);
                                    if (preyearsale > 0 || preyearsale < 0)
                                    {
                                        PYsale = preyearsale;
                                    }
                                    salelist.Add(PYsale);
                                    salelist.Add(Diff);
                                }
                                dt.Rows.Add(sites.site_name, vendorbrand.brandName, salelist[0], salelist[1], salelist[2],
                                    salelist[3], salelist[4], salelist[5], salelist[6], salelist[7], salelist[8], salelist[9],
                                    salelist[10], salelist[11], salelist[12], salelist[13], salelist[14], salelist[15], salelist[16],
                                    salelist[17], salelist[18], salelist[19], salelist[20]);

                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WeeklysalecompareReport.xlsx");
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("ReportWeekSaleCompare");
        }

        public ActionResult getdetailStockReport(int? siteid, string stockdate, string brands)
        {
            detailsStockReportvendorViewModel DailystockReportModel = new detailsStockReportvendorViewModel();
            List<siteNamelistmodel> sitedetailsmodellist = new List<siteNamelistmodel>();
            List<tblSite> site = new List<tblSite>();
            List<tblBrand> BrandNamelist = new List<tblBrand>();
            List<string> vendorsitelist = new List<string>();
            try
            {
                DateTime startDate = DateTime.ParseExact(stockdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = startDate.AddDays(1);

                int vendorId;
                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblSection> Sectionlist = context.tblSections.OrderBy(x => x.sectionName).ToList();
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblDepartment> AllDept = context.tblDepartments.OrderBy(x => x.departmentName).ToList();
                    List<tblDivision> Divisionlist = context.tblDivisions.OrderBy(x => x.divisionName).ToList();
                    List<tblColor> Colorlist = context.tblColors.OrderBy(x => x.colorName).ToList();
                    List<tblSize> Sizelist = context.tblSizes.OrderBy(x => x.sizeName).ToList();
                    brands = brands.Replace("A_N_D", "&");
                    string[] element = brands.ToString().Split(',');
                    foreach (var brandname in element)
                    {
                        if (!string.IsNullOrEmpty(brandname))
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                            BrandNamelist.Add(assignbrand);
                        }
                    }
                    if (BrandNamelist.Count < 1)
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                        foreach (var brandname in _getVendorBrand)
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                            if (assignbrand != null)
                            {
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                    }
                    if (siteid > 0 && siteid != null)
                    {
                        DailystockReportModel.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate).Select(x => x.site_id).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_id == sitename select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    foreach (var sites in site)
                    {
                        siteNamelistmodel sitedetailsmodel = new siteNamelistmodel();
                        List<Stockdetailsviewmodel> stocklistdetails = new List<Stockdetailsviewmodel>();
                        sitedetailsmodel.SiteName = sites.site_name;
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            List<tblstock> Stockresult = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate && x.site_id == sites.site_id).ToList();
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<tblstock> stockqty = (from c in Stockresult where c.brand_id == vendorbrand.brand_id select c).ToList();
                                foreach (var stockitem in stockqty)
                                {
                                    Stockdetailsviewmodel stockresult = new Stockdetailsviewmodel();
                                    var sectionname = (from c in Sectionlist where c.sec_id == stockitem.sec_id select c).FirstOrDefault();
                                    if (sectionname != null)
                                    {
                                        stockresult.Section = sectionname.sectionName;
                                    }
                                    stockresult.Category = stockitem.category2;
                                    stockresult.Brand = vendorbrand.brandName;
                                    var departmentname = (from c in AllDept where c.dep_id == stockitem.dep_id select c).FirstOrDefault();
                                    if (departmentname != null)
                                    {
                                        stockresult.Department = departmentname.departmentName;
                                    }
                                    stockresult.Desc4 = stockitem.desc4;
                                    var divisionname = (from c in Divisionlist where c.div_id == stockitem.div_id select c).FirstOrDefault();
                                    if (divisionname != null)
                                    {
                                        stockresult.Division = divisionname.divisionName;
                                    }
                                    var colorname = (from c in Colorlist where c.color_id == stockitem.color_id select c).FirstOrDefault();
                                    if (colorname != null)
                                    {
                                        stockresult.Color = colorname.colorName;
                                    }
                                    stockresult.Stylecode = stockitem.styleCode;
                                    stockresult.Desc6 = stockitem.desc6;
                                    var sizename = (from c in Sizelist where c.size_id == stockitem.size_id select c).FirstOrDefault();
                                    if (sizename != null)
                                    {
                                        stockresult.size = sizename.sizeName;
                                    }
                                    stockresult.Fit = stockitem.fit;
                                    stockresult.Quantity = stockitem.closingTotal.ToString();
                                    stockresult.MRP = stockitem.mrp;
                                    stocklistdetails.Add(stockresult);
                                }
                            }
                        }
                        sitedetailsmodel.brandslist = stocklistdetails;
                        sitedetailsmodellist.Add(sitedetailsmodel);
                    }
                    DailystockReportModel.Sitelist = sitedetailsmodellist;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return PartialView("~/Views/Report/_detailStockReportPartial.cshtml", DailystockReportModel);

        }
        public ActionResult DatailsStockReportSitewise()
        {
            detailsStockReportvendorViewModel DailystockReportModel = new detailsStockReportvendorViewModel();
            List<string> vendorsitelist = new List<string>();
            try
            {
                DateTime getdate = DateTime.Today.AddDays(-1);
                DailystockReportModel.stockdate = string.Format("{0:dd-MM-yyyy}", getdate);


                ViewBag.SiteNames = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
                {
                    Value = x.site_id.ToString(),
                    Text = x.site_name
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return View(DailystockReportModel);
        }
        public ActionResult ExportDatailsStockReportSitewise(int? siteid, string stockdate, string brands)
        {
            DateTime startDate = DateTime.ParseExact(stockdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = startDate.AddDays(1);
            int vendorId;

            try
            {
                DataTable dt = new DataTable("StockDetailsReportSitewise");
                dt.Columns.AddRange(new DataColumn[13] {
                                            new DataColumn("Sitename"),
                                            new DataColumn("Section"),
                                            new DataColumn("Category2"),
                                            new DataColumn("Brand"),
                                            new DataColumn("Department"),
                                            new DataColumn("Division"),
                                            new DataColumn("Color"),
                                            new DataColumn("Stylecode"),
                                            new DataColumn("Desc6"),
                                            new DataColumn("Size"),
                                            new DataColumn("Fit"),
                                            new DataColumn("Quantity",typeof(Int32)),
                                            new DataColumn("MRP",typeof(decimal))});
                detailsStockReportvendorViewModel DailystockReportModel = new detailsStockReportvendorViewModel();
                List<tblSite> site = new List<tblSite>();
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                List<string> vendorsitelist = new List<string>();

                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    List<tblSection> Sectionlist = context.tblSections.OrderBy(x => x.sectionName).ToList();
                    List<tblBrand> AllBrand = context.tblBrands.OrderBy(x => x.brandName).ToList();
                    List<tblDepartment> AllDept = context.tblDepartments.OrderBy(x => x.departmentName).ToList();
                    List<tblDivision> Divisionlist = context.tblDivisions.OrderBy(x => x.divisionName).ToList();
                    List<tblColor> Colorlist = context.tblColors.OrderBy(x => x.colorName).ToList();
                    List<tblSize> Sizelist = context.tblSizes.OrderBy(x => x.sizeName).ToList();
                    brands = brands.Replace("A_N_D", "&");
                    string[] element = brands.ToString().Split(',');
                    foreach (var brandname in element)
                    {
                        if (!string.IsNullOrEmpty(brandname))
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brandName.Trim() == brandname.Trim() select c).FirstOrDefault();
                            BrandNamelist.Add(assignbrand);
                        }
                    }
                    if (BrandNamelist.Count < 1)
                    {
                        vendorId = Convert.ToInt32(Session["id"]);
                        List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                        foreach (var brandname in _getVendorBrand)
                        {
                            tblBrand assignbrand = (from c in AllBrand where c.brand_id == brandname.brandId select c).FirstOrDefault();
                            if (assignbrand != null)
                            {
                                BrandNamelist.Add(assignbrand);
                            }
                        }
                    }
                    if (siteid > 0 && siteid != null)
                    {
                        DailystockReportModel.siteid = siteid.ToString();
                        site = _site.FindBy(x => x.site_id == siteid).OrderBy(c => c.site_name).ToList();
                        foreach (var sitename in site)
                        {
                            vendorsitelist.Add(sitename.site_name);
                        }
                    }
                    else
                    {
                        site = _site.GetAll().OrderBy(c => c.site_name).ToList();
                        var vendorsites = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate).Select(x => x.site_id).Distinct().ToList();
                        foreach (var sitename in vendorsites)
                        {
                            var getsitename = (from c in site where c.site_id == sitename select c).FirstOrDefault();
                            if (getsitename != null)
                            {
                                vendorsitelist.Add(getsitename.site_name);
                            }
                        }
                    }
                    foreach (var sites in site)
                    {
                        if (vendorsitelist.Exists(p => p.Equals(sites.site_name)))
                        {
                            List<tblstock> Stockresult = context.tblstocks.Where(x => x.lastStockUpdate >= startDate && x.lastStockUpdate < endDate && x.site_id == sites.site_id).ToList();
                            foreach (var vendorbrand in BrandNamelist)
                            {
                                List<tblstock> stockqty = (from c in Stockresult where c.brand_id == vendorbrand.brand_id select c).ToList();
                                foreach (var stockitem in stockqty)
                                {
                                    Stockdetailsviewmodel stockresult = new Stockdetailsviewmodel();
                                    var sectionname = (from c in Sectionlist where c.sec_id == stockitem.sec_id select c).FirstOrDefault();
                                    if (sectionname != null)
                                    {
                                        stockresult.Section = sectionname.sectionName;
                                    }
                                    var departmentname = (from c in AllDept where c.dep_id == stockitem.dep_id select c).FirstOrDefault();
                                    if (departmentname != null)
                                    {
                                        stockresult.Department = departmentname.departmentName;
                                    }
                                    var divisionname = (from c in Divisionlist where c.div_id == stockitem.div_id select c).FirstOrDefault();
                                    if (divisionname != null)
                                    {
                                        stockresult.Division = divisionname.divisionName;
                                    }
                                    var colorname = (from c in Colorlist where c.color_id == stockitem.color_id select c).FirstOrDefault();
                                    if (colorname != null)
                                    {
                                        stockresult.Color = colorname.colorName;
                                    }

                                    var sizename = (from c in Sizelist where c.size_id == stockitem.size_id select c).FirstOrDefault();
                                    if (sizename != null)
                                    {
                                        stockresult.size = sizename.sizeName;
                                    }
                                    decimal mrpamt = Convert.ToDecimal(stockitem.mrp);
                                    dt.Rows.Add(sites.site_name, stockresult.Section, stockitem.category2, vendorbrand.brandName, stockresult.Department,
                                   stockresult.Division, stockresult.Color, stockitem.styleCode, stockitem.desc6, stockresult.size, stockitem.fit, stockitem.closingTotal, mrpamt);
                                }
                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockDetailsReportSitewise.xlsx");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return RedirectToAction("DatailsStockReportSitewise");
        }

        public ActionResult GetVendorBrandList(string q, int? id)
        {
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                List<Select2Model> details = new List<Select2Model>();
                int vendorId;
                vendorId = Convert.ToInt32(Session["id"]);

                var _getVendorBrand = (from c in context.tblBrands join v in context.tblVendorBrands on c.brand_id equals v.brandId where v.vendorId == vendorId select c.brandName).ToList();
                //context.SP_VendorTemplateGetVendorBrand(vendorId);
                foreach (var brandname in _getVendorBrand)
                {
                    Select2Model getdata = new Select2Model();
                    getdata.id = brandname.ToString();
                    getdata.text = brandname;
                    getdata.selected = true;
                    details.Add(getdata);
                    // BrandNamelist.Add(brandname.ToUpper());
                }
                List<Select2Model> list = new List<Select2Model>();

                if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
                {
                    list = details.Where(x => x.text.ToLower().StartsWith(q.ToLower())).Select(c => new Select2Model() { id = c.text.ToString(), text = c.text }).ToList();
                }
                else
                {
                    list = details.ToList().Select(c => new Select2Model() { id = c.text.ToString(), text = c.text }).ToList();
                }


                return Json(new { items = list }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PSPFDReport()
        {
            ViewBag.Sites = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
            {
                Value = x.site_name.ToString(),
                Text = x.site_name.Trim()
            });
            return View();
        }
        public ActionResult PsfpddetailReport(string Category, string Reporttype, string selectedmonth, string Sites)
        {
            List<PfpsdDetailreportViewModel> detailrankingreportlist = new List<PfpsdDetailreportViewModel>();
            List<PfpsdDetailreportViewModel> Newdetailrankingreportlist = new List<PfpsdDetailreportViewModel>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {

                List<tblBrand> allbrands = context.tblBrands.ToList();
                DateTime startdate = DateTime.Now;
                DateTime enddate = startdate;
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                int currentmonth = Convert.ToInt32(DateTime.Now.Month);
                int passmonth = Convert.ToInt32(selectedmonth);
                int vendorId = Convert.ToInt32(Session["id"]);
                List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                foreach (var brandname in _getVendorBrand)
                {
                    tblBrand assignbrand = (from c in allbrands where c.brand_id == brandname.brandId select c).FirstOrDefault();
                    if (assignbrand != null)
                    {
                        BrandNamelist.Add(assignbrand);
                    }
                }
                if (passmonth == currentmonth)
                {
                    string eddate = string.Format("{0:dd-MM-yyyy}", enddate);
                    string stdate = "01-" + selectedmonth + "-" + DateTime.Now.Year.ToString();
                    startdate = DateTime.ParseExact(stdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    enddate = DateTime.ParseExact(eddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    int year = DateTime.Now.Year;
                    if (passmonth > 3 && currentmonth < 4)
                        year = DateTime.Now.Year - 1;
                    else if (currentmonth > 3 && passmonth < 4)
                        year = DateTime.Now.Year + 1;

                    startdate = DateTime.ParseExact("01-" + selectedmonth + "-" + year, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    enddate = startdate.AddMonths(1);
                    enddate = enddate.AddDays(-1);
                }
                int daydiff = Convert.ToInt32((enddate - startdate).TotalDays + 1);


                try
                {
                    if (string.IsNullOrEmpty(Sites))
                    {
                        var _rankingreport = context.SP_Getpspfddetailreportmonth(startdate, enddate);
                        foreach (var item in _rankingreport)
                        {
                            var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                            if (branddetails.height != null && branddetails.wallSpace != null)
                            {
                                PfpsdDetailreportViewModel detailrankingreportmodel = new PfpsdDetailreportViewModel();
                                detailrankingreportmodel.BrandName = item.brandname;
                                detailrankingreportmodel.LocationGrade = branddetails.locationGrade;
                                detailrankingreportmodel.Category = branddetails.brandCategory;
                                detailrankingreportmodel.ZoneName = branddetails.zoneName;
                                detailrankingreportmodel.WallSpace = Convert.ToInt32(branddetails.wallSpace);
                                detailrankingreportmodel.Height = Convert.ToInt32(branddetails.height);
                                int total = (Convert.ToInt32(branddetails.wallSpace) * Convert.ToInt32(branddetails.height));
                                detailrankingreportmodel.Total = total;
                                double sqft2 = (double)total / 144;
                                double sqft = Math.Round((double)total / 144);
                                detailrankingreportmodel.Sqft = Convert.ToInt32(sqft);
                                detailrankingreportmodel.Quantity = item.saleQty;
                                detailrankingreportmodel.Amount = item.netAmt;
                                double revenue = Math.Round(item.netAmt / sqft2 / daydiff);
                                detailrankingreportmodel.persqftrevenue = Convert.ToInt32(revenue);
                                detailrankingreportlist.Add(detailrankingreportmodel);
                            }

                        }
                    }
                    else
                    {
                        tblSite site = context.tblSites.Where(x => x.site_name.ToLower() == Sites.ToLower()).FirstOrDefault();
                        var _rankingreport = context.SP_GetpspfddetailreportmonthwithSite(site.site_cuid, startdate, enddate);
                        foreach (var item in _rankingreport)
                        {
                            var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                            if (branddetails.height != null && branddetails.wallSpace != null)
                            {
                                PfpsdDetailreportViewModel detailrankingreportmodel = new PfpsdDetailreportViewModel();
                                detailrankingreportmodel.BrandName = item.brandname;
                                detailrankingreportmodel.LocationGrade = branddetails.locationGrade;
                                detailrankingreportmodel.Category = branddetails.brandCategory;
                                detailrankingreportmodel.ZoneName = branddetails.zoneName;
                                detailrankingreportmodel.WallSpace = Convert.ToInt32(branddetails.wallSpace);
                                detailrankingreportmodel.Height = Convert.ToInt32(branddetails.height);
                                int total = (Convert.ToInt32(branddetails.wallSpace) * Convert.ToInt32(branddetails.height));
                                detailrankingreportmodel.Total = total;
                                double sqft2 = (double)total / 144;
                                double sqft = Math.Round((double)total / 144);
                                detailrankingreportmodel.Sqft = Convert.ToInt32(sqft);
                                detailrankingreportmodel.Quantity = item.saleQty;
                                detailrankingreportmodel.Amount = item.netAmt;
                                double revenue = Math.Round(item.netAmt / sqft2 / daydiff);
                                detailrankingreportmodel.persqftrevenue = Convert.ToInt32(revenue);
                                detailrankingreportlist.Add(detailrankingreportmodel);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {

                }


                int counter = 0;
                int Totalbusinessofzone = 0;
                int Totalsqftofzone = 0;
                double Persqftrevenuedailyofzone = 0;
                string zone = string.Empty;
                string cat = string.Empty;
                if (Category == "Zone Level")
                {
                    foreach (var copylist in detailrankingreportlist.OrderBy(x => x.ZoneName).ThenByDescending(x => x.persqftrevenue))
                    {
                        if (string.IsNullOrEmpty(cat) || cat == copylist.ZoneName)
                            counter = counter + 1;
                        else
                            counter = 1;

                        if (!string.IsNullOrEmpty(cat) && cat != copylist.ZoneName)
                        {
                            Totalsqftofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Sqft).Sum();
                            Totalbusinessofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Amount).Sum();
                            Persqftrevenuedailyofzone = Math.Round((double)Totalbusinessofzone / Totalsqftofzone / 12, 2);
                        }
                        if (string.IsNullOrEmpty(cat))
                        {
                            Totalsqftofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Sqft).Sum();
                            Totalbusinessofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Amount).Sum();
                            Persqftrevenuedailyofzone = Math.Round((double)Totalbusinessofzone / Totalsqftofzone / 12, 2);
                        }
                        if (BrandNamelist.Exists(p => p.zoneName.Equals(copylist.ZoneName)))
                        {
                            PfpsdDetailreportViewModel detailrankingreportmodel = new PfpsdDetailreportViewModel();
                            detailrankingreportmodel.BrandName = copylist.BrandName;
                            detailrankingreportmodel.Ranking = counter.ToString();
                            detailrankingreportmodel.LocationGrade = copylist.LocationGrade;
                            detailrankingreportmodel.Category = copylist.Category;
                            detailrankingreportmodel.ZoneName = copylist.ZoneName;
                            detailrankingreportmodel.WallSpace = Convert.ToInt32(copylist.WallSpace);
                            detailrankingreportmodel.Height = Convert.ToInt32(copylist.Height);
                            detailrankingreportmodel.Total = copylist.Total;
                            detailrankingreportmodel.Sqft = copylist.Sqft; ;
                            detailrankingreportmodel.Quantity = copylist.Quantity;
                            detailrankingreportmodel.Amount = copylist.Amount;
                            detailrankingreportmodel.persqftrevenue = copylist.persqftrevenue;
                            if (counter < 5)
                            {
                                Newdetailrankingreportlist.Add(detailrankingreportmodel);
                            }
                            else
                            {
                                if (BrandNamelist.Exists(p => p.brandName.Equals(copylist.BrandName)))
                                {
                                    Newdetailrankingreportlist.Add(detailrankingreportmodel);
                                }
                            }

                        }
                        cat = copylist.ZoneName;


                    }
                }
                else
                {
                    foreach (var copylist in detailrankingreportlist.OrderBy(x => x.Category).ThenByDescending(x => x.persqftrevenue))
                    {
                        if (string.IsNullOrEmpty(cat) || cat == copylist.Category)
                            counter = counter + 1;
                        else
                            counter = 1;

                        if (!string.IsNullOrEmpty(zone) && zone != copylist.ZoneName)
                        {
                            Totalsqftofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Sqft).Sum();
                            Totalbusinessofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Amount).Sum();
                            Persqftrevenuedailyofzone = Math.Round((double)Totalbusinessofzone / Totalsqftofzone / 12, 2);
                        }
                        if (string.IsNullOrEmpty(zone))
                        {
                            Totalsqftofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Sqft).Sum();
                            Totalbusinessofzone = (from c in detailrankingreportlist where c.ZoneName == copylist.ZoneName select c.Amount).Sum();
                            Persqftrevenuedailyofzone = Math.Round((double)Totalbusinessofzone / Totalsqftofzone / 12, 2);
                        }
                        if (BrandNamelist.Exists(p => p.brandCategory.Equals(copylist.Category)))
                        {
                            PfpsdDetailreportViewModel detailrankingreportmodel = new PfpsdDetailreportViewModel();
                            detailrankingreportmodel.BrandName = copylist.BrandName;
                            detailrankingreportmodel.Ranking = counter.ToString();
                            detailrankingreportmodel.LocationGrade = copylist.LocationGrade;
                            detailrankingreportmodel.Category = copylist.Category;
                            detailrankingreportmodel.ZoneName = copylist.ZoneName;
                            detailrankingreportmodel.WallSpace = Convert.ToInt32(copylist.WallSpace);
                            detailrankingreportmodel.Height = Convert.ToInt32(copylist.Height);
                            detailrankingreportmodel.Total = copylist.Total;
                            detailrankingreportmodel.Sqft = copylist.Sqft; ;
                            detailrankingreportmodel.Quantity = copylist.Quantity;
                            detailrankingreportmodel.Amount = copylist.Amount;
                            detailrankingreportmodel.persqftrevenue = copylist.persqftrevenue;
                            if (counter < 5)
                            {
                                Newdetailrankingreportlist.Add(detailrankingreportmodel);
                            }
                            else
                            {
                                if (BrandNamelist.Exists(p => p.brandName.Equals(copylist.BrandName)))
                                {
                                    Newdetailrankingreportlist.Add(detailrankingreportmodel);
                                }
                            }
                        }
                        cat = copylist.Category;
                        zone = copylist.ZoneName;


                    }
                }


                Session["DownloadpspfddetailReport"] = Newdetailrankingreportlist;
            }
            return PartialView("~/Views/Report/_pspfdmonthreportpartial.cshtml", Newdetailrankingreportlist);
        }

        public ActionResult PsfpdsummaryReport(string Category, string Reporttype, string Sites)
        {
            List<pspfdSummaryReportViewModel> Newsummaryrankingreportlist = new List<pspfdSummaryReportViewModel>();
            List<pspfdSummaryReportViewModel> summaryrankingreportlist = new List<pspfdSummaryReportViewModel>();
            List<PfpsdDetailreportdataViewModel> detailrankingreportlist = new List<PfpsdDetailreportdataViewModel>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                int divideby = 0;
                List<tblBrand> allbrands = context.tblBrands.ToList();
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                DateTime startdate = DateTime.Now;
                DateTime enddate = startdate;
                DateTime Todaysdate = startdate;
                int currentmonth = Convert.ToInt32(DateTime.Now.Month);
                int vendorId = Convert.ToInt32(Session["id"]);
                List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                foreach (var brandname in _getVendorBrand)
                {
                    tblBrand assignbrand = (from c in allbrands where c.brand_id == brandname.brandId select c).FirstOrDefault();
                    if (assignbrand != null)
                    {
                        BrandNamelist.Add(assignbrand);
                    }
                }
                if (currentmonth > 3)
                {
                    divideby = currentmonth - 3;
                    string stdate = "01-" + "04-" + DateTime.Now.Year;
                    startdate = DateTime.ParseExact(stdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    divideby = currentmonth + 9;
                    int extyr = DateTime.Now.Year - 1;
                    string stdate = "01-" + "04-" + extyr;
                    startdate = DateTime.ParseExact(stdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                enddate = startdate.AddMonths(1);
                enddate = enddate.AddDays(-1);
                for (int i = 1; i <= 12; i++)
                {
                    int daydiff = Convert.ToInt32((enddate - startdate).TotalDays + 1);
                    try
                    {
                        if (string.IsNullOrEmpty(Sites))
                        {
                            var _rankingreport = context.SP_Getpspfddetailreportmonth(startdate, enddate);
                            foreach (var item in _rankingreport)
                            {
                                var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                                if (branddetails.height != null && branddetails.wallSpace != null)
                                {
                                    PfpsdDetailreportdataViewModel detailrankingreportmodel = new PfpsdDetailreportdataViewModel();
                                    detailrankingreportmodel.BrandName = item.brandname;
                                    detailrankingreportmodel.Month = startdate.Month.ToString();
                                    detailrankingreportmodel.ZoneName = branddetails.zoneName;
                                    detailrankingreportmodel.Category = branddetails.brandCategory;
                                    detailrankingreportmodel.WallSpace = Convert.ToInt32(branddetails.wallSpace);
                                    detailrankingreportmodel.Height = Convert.ToInt32(branddetails.height);
                                    int total = (Convert.ToInt32(branddetails.wallSpace) * Convert.ToInt32(branddetails.height));
                                    detailrankingreportmodel.Total = total;
                                    double sqft2 = (double)total / 144;
                                    double sqft = Math.Round((double)total / 144);
                                    detailrankingreportmodel.Sqft = Convert.ToInt32(sqft);
                                    detailrankingreportmodel.Quantity = item.saleQty;
                                    detailrankingreportmodel.Amount = item.netAmt;
                                    double revenue = Math.Round(item.netAmt / sqft2 / daydiff);
                                    detailrankingreportmodel.persqftrevenue = Convert.ToInt32(revenue);
                                    detailrankingreportlist.Add(detailrankingreportmodel);
                                }

                            }
                        }
                        else
                        {
                            tblSite site = context.tblSites.Where(x => x.site_name.ToLower() == Sites.ToLower()).FirstOrDefault();
                            var _rankingreport = context.SP_GetpspfddetailreportmonthwithSite(site.site_cuid, startdate, enddate);
                            foreach (var item in _rankingreport)
                            {
                                var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                                if (branddetails.height != null && branddetails.wallSpace != null)
                                {
                                    PfpsdDetailreportdataViewModel detailrankingreportmodel = new PfpsdDetailreportdataViewModel();
                                    detailrankingreportmodel.BrandName = item.brandname;
                                    detailrankingreportmodel.Month = startdate.Month.ToString();
                                    detailrankingreportmodel.ZoneName = branddetails.zoneName;
                                    detailrankingreportmodel.Category = branddetails.brandCategory;
                                    detailrankingreportmodel.WallSpace = Convert.ToInt32(branddetails.wallSpace);
                                    detailrankingreportmodel.Height = Convert.ToInt32(branddetails.height);
                                    int total = (Convert.ToInt32(branddetails.wallSpace) * Convert.ToInt32(branddetails.height));
                                    detailrankingreportmodel.Total = total;
                                    double sqft2 = (double)total / 144;
                                    double sqft = Math.Round((double)total / 144);
                                    detailrankingreportmodel.Sqft = Convert.ToInt32(sqft);
                                    detailrankingreportmodel.Quantity = item.saleQty;
                                    detailrankingreportmodel.Amount = item.netAmt;
                                    double revenue = Math.Round(item.netAmt / sqft2 / daydiff);
                                    detailrankingreportmodel.persqftrevenue = Convert.ToInt32(revenue);
                                    detailrankingreportlist.Add(detailrankingreportmodel);
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    startdate = startdate.AddMonths(1);
                    enddate = startdate.AddMonths(1);
                    enddate = enddate.AddDays(-1);
                    if (Todaysdate >= startdate && Todaysdate <= enddate)
                    {
                        enddate = Todaysdate;
                    }

                }

                var distinctbrandlist = (from c in detailrankingreportlist select c.BrandName).ToList().Distinct();
                foreach (var brand in distinctbrandlist)
                {

                    pspfdSummaryReportViewModel summaryrankingreport = new pspfdSummaryReportViewModel();
                    summaryrankingreport.BrandName = brand;
                    var catandzone = (from c in detailrankingreportlist where c.BrandName == brand select c).FirstOrDefault();
                    summaryrankingreport.ZoneName = catandzone.ZoneName;
                    summaryrankingreport.Category = catandzone.Category;
                    var apr = (from c in detailrankingreportlist where c.Month == "4" && c.BrandName == brand select c).FirstOrDefault();
                    if (apr != null)
                        summaryrankingreport.April = apr.persqftrevenue;
                    else
                        summaryrankingreport.April = 0;
                    var may = (from c in detailrankingreportlist where c.Month == "5" && c.BrandName == brand select c).FirstOrDefault();
                    if (may != null)
                        summaryrankingreport.May = may.persqftrevenue;
                    else
                        summaryrankingreport.May = 0;
                    var june = (from c in detailrankingreportlist where c.Month == "6" && c.BrandName == brand select c).FirstOrDefault();
                    if (june != null)
                        summaryrankingreport.June = june.persqftrevenue;
                    else
                        summaryrankingreport.June = 0;
                    var jul = (from c in detailrankingreportlist where c.Month == "7" && c.BrandName == brand select c).FirstOrDefault();
                    if (jul != null)
                        summaryrankingreport.July = jul.persqftrevenue;
                    else
                        summaryrankingreport.July = 0;
                    var aug = (from c in detailrankingreportlist where c.Month == "8" && c.BrandName == brand select c).FirstOrDefault();
                    if (aug != null)
                        summaryrankingreport.August = aug.persqftrevenue;
                    else
                        summaryrankingreport.August = 0;
                    var sept = (from c in detailrankingreportlist where c.Month == "9" && c.BrandName == brand select c).FirstOrDefault();
                    if (sept != null)
                        summaryrankingreport.September = sept.persqftrevenue;
                    else
                        summaryrankingreport.September = 0;
                    var oct = (from c in detailrankingreportlist where c.Month == "10" && c.BrandName == brand select c).FirstOrDefault();
                    if (oct != null)
                        summaryrankingreport.October = oct.persqftrevenue;
                    else
                        summaryrankingreport.October = 0;
                    var nov = (from c in detailrankingreportlist where c.Month == "11" && c.BrandName == brand select c).FirstOrDefault();
                    if (nov != null)
                        summaryrankingreport.November = nov.persqftrevenue;
                    else
                        summaryrankingreport.November = 0;
                    var dec = (from c in detailrankingreportlist where c.Month == "12" && c.BrandName == brand select c).FirstOrDefault();
                    if (dec != null)
                        summaryrankingreport.December = dec.persqftrevenue;
                    else
                        summaryrankingreport.December = 0;
                    var jan = (from c in detailrankingreportlist where c.Month == "1" && c.BrandName == brand select c).FirstOrDefault();
                    if (jan != null)
                        summaryrankingreport.January = jan.persqftrevenue;
                    else
                        summaryrankingreport.January = 0;
                    var feb = (from c in detailrankingreportlist where c.Month == "2" && c.BrandName == brand select c).FirstOrDefault();
                    if (feb != null)
                        summaryrankingreport.February = feb.persqftrevenue;
                    else
                        summaryrankingreport.February = 0;
                    var mar = (from c in detailrankingreportlist where c.Month == "3" && c.BrandName == brand select c).FirstOrDefault();
                    if (mar != null)
                        summaryrankingreport.March = mar.persqftrevenue;
                    else
                        summaryrankingreport.March = 0;
                    summaryrankingreport.Avg = (summaryrankingreport.April + summaryrankingreport.May + summaryrankingreport.June + summaryrankingreport.July + summaryrankingreport.August + summaryrankingreport.September + summaryrankingreport.October + summaryrankingreport.November + summaryrankingreport.December + summaryrankingreport.January + summaryrankingreport.February + summaryrankingreport.March) / divideby;
                    summaryrankingreportlist.Add(summaryrankingreport);
                }

                int counter = 0;
                string cat = string.Empty;
                if (Category == "Zone Level")
                {
                    foreach (var sort in summaryrankingreportlist.OrderBy(x => x.ZoneName).ThenByDescending(x => x.Avg))
                    {
                        if (string.IsNullOrEmpty(cat) || cat == sort.ZoneName)
                            counter = counter + 1;
                        else
                            counter = 1;
                        if (BrandNamelist.Exists(p => p.zoneName.Equals(sort.ZoneName)))
                        {
                            pspfdSummaryReportViewModel summaryrankingreport = new pspfdSummaryReportViewModel();
                            summaryrankingreport.BrandName = sort.BrandName;
                            summaryrankingreport.Ranking = counter;
                            summaryrankingreport.April = sort.April;
                            summaryrankingreport.May = sort.May;
                            summaryrankingreport.June = sort.June;
                            summaryrankingreport.July = sort.July;
                            summaryrankingreport.August = sort.August;
                            summaryrankingreport.September = sort.September;
                            summaryrankingreport.October = sort.October;
                            summaryrankingreport.November = sort.November;
                            summaryrankingreport.December = sort.December;
                            summaryrankingreport.January = sort.January;
                            summaryrankingreport.February = sort.February;
                            summaryrankingreport.March = sort.March;
                            summaryrankingreport.Avg = sort.Avg;
                            if (counter < 5)
                            {
                                Newsummaryrankingreportlist.Add(summaryrankingreport);
                            }
                            else
                            {
                                if (BrandNamelist.Exists(p => p.brandName.Equals(sort.BrandName)))
                                {
                                    Newsummaryrankingreportlist.Add(summaryrankingreport);
                                }
                            }
                        }
                        cat = sort.ZoneName;

                    }
                }
                else
                {
                    foreach (var sort in summaryrankingreportlist.OrderBy(x => x.Category).ThenByDescending(x => x.Avg))
                    {
                        if (string.IsNullOrEmpty(cat) || cat == sort.Category)
                            counter = counter + 1;
                        else
                            counter = 1;
                        if (BrandNamelist.Exists(p => p.brandCategory.Equals(sort.Category)))
                        {
                            pspfdSummaryReportViewModel summaryrankingreport = new pspfdSummaryReportViewModel();
                            summaryrankingreport.BrandName = sort.BrandName;
                            summaryrankingreport.Ranking = counter;
                            summaryrankingreport.April = sort.April;
                            summaryrankingreport.May = sort.May;
                            summaryrankingreport.June = sort.June;
                            summaryrankingreport.July = sort.July;
                            summaryrankingreport.August = sort.August;
                            summaryrankingreport.September = sort.September;
                            summaryrankingreport.October = sort.October;
                            summaryrankingreport.November = sort.November;
                            summaryrankingreport.December = sort.December;
                            summaryrankingreport.January = sort.January;
                            summaryrankingreport.February = sort.February;
                            summaryrankingreport.March = sort.March;
                            summaryrankingreport.Avg = sort.Avg;
                            if (counter < 5)
                            {
                                Newsummaryrankingreportlist.Add(summaryrankingreport);
                            }
                            else
                            {
                                if (BrandNamelist.Exists(p => p.brandName.Equals(sort.BrandName)))
                                {
                                    Newsummaryrankingreportlist.Add(summaryrankingreport);
                                }
                            }
                        }
                        cat = sort.Category;
                    }
                }
                Session["DownloadpspfdSummaryReport"] = Newsummaryrankingreportlist;
            }
            return PartialView("~/Views/Report/_pspfdyearsummaryreportpartial.cshtml", Newsummaryrankingreportlist);
        }

        public ActionResult DownloadPspfdmonthReport()
        {
            List<PfpsdDetailreportViewModel> ModelResultdata = Session["DownloadpspfddetailReport"] as List<PfpsdDetailreportViewModel>;
            DataTable dt = new DataTable("PSPFrankingDetailReport");
            dt.Columns.AddRange(new DataColumn[8] {
                                            new DataColumn("Brand Name"),
                                            new DataColumn("Ranking"),
                                            new DataColumn("Category"),
                                            new DataColumn("Zone Name"),
                                            new DataColumn("Sq Ft",typeof(Int32)),
                                            new DataColumn("Quantity",typeof(Int32)),
                                            new DataColumn("Amount",typeof(Int32)),
                                            new DataColumn("Per Sq Ft Revenue Daily",typeof(Int32))});

            foreach (var rslt in ModelResultdata)
            {
                dt.Rows.Add(rslt.BrandName, rslt.Ranking, rslt.Category, rslt.ZoneName, rslt.Sqft, rslt.Quantity, rslt.Amount, rslt.persqftrevenue);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PSPFrankingDetailReport.xlsx");
                }
            }
        }
        public ActionResult DownloadPspfdYearsummaryReport()
        {
            List<pspfdSummaryReportViewModel> ModelResultdata = Session["DownloadpspfdSummaryReport"] as List<pspfdSummaryReportViewModel>;
            DataTable dt = new DataTable("PSPFrankingSummaryReport");
            dt.Columns.AddRange(new DataColumn[15] {
                                            new DataColumn("Brand Name"),
                                            new DataColumn("Ranking",typeof(Int32)),
                                            new DataColumn("April",typeof(Int32)),
                                            new DataColumn("May",typeof(Int32)),
                                            new DataColumn("June",typeof(Int32)),
                                            new DataColumn("July",typeof(Int32)),
                                            new DataColumn("August",typeof(Int32)),
                                            new DataColumn("September",typeof(Int32)),
                                            new DataColumn("October",typeof(Int32)),
                                            new DataColumn("November",typeof(Int32)),
                                            new DataColumn("December",typeof(Int32)),
                                            new DataColumn("January",typeof(Int32)),
                                            new DataColumn("February",typeof(Int32)),
                                            new DataColumn("March",typeof(Int32)),
                                            new DataColumn("Avg",typeof(Int32))});
            foreach (var rslt in ModelResultdata)
            {
                dt.Rows.Add(rslt.BrandName, rslt.Ranking, rslt.April, rslt.May, rslt.June, rslt.July, rslt.August, rslt.September, rslt.October, rslt.November, rslt.December, rslt.January, rslt.February, rslt.March, rslt.Avg);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PSPFrankingSummaryReport.xlsx");
                }
            }
        }

        public ActionResult SellThroughReport()
        {
            ViewBag.Sites = _site.GetAll().OrderBy(x => x.site_id).Select(x => new SelectListItem()
            {
                Value = x.site_name.ToString(),
                Text = x.site_name.Trim()
            });
            return View();
        }

        public ActionResult sellThroughRankingReport(string SiteName, string selectedmonth, string Division)
        {
            List<saleThroughrankungViewModel> salethroughreportlist = new List<saleThroughrankungViewModel>();
            List<saleThroughrankungViewModel> Newsalethroughreportlist = new List<saleThroughrankungViewModel>();
            List<stocksaleThroughreankungViewModel> detailstockmodellist = new List<stocksaleThroughreankungViewModel>();
            List<saleThroughreankungViewModel> detailSalemodellist = new List<saleThroughreankungViewModel>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                List<tblBrand> allbrands = context.tblBrands.ToList();
                List<string> brandresult = new List<string>();
                List<tblBrand> BrandNamelist = new List<tblBrand>();
                List<tblDivision> alldivision = context.tblDivisions.ToList();
                DateTime startdate = DateTime.Now;
                DateTime enddate = startdate;
                int vendorId = Convert.ToInt32(Session["id"]);
                List<tblVendorBrand> _getVendorBrand = context.tblVendorBrands.Where(x => x.vendorId == vendorId).ToList();
                foreach (var brandname in _getVendorBrand)
                {
                    tblBrand assignbrand = (from c in allbrands where c.brand_id == brandname.brandId select c).FirstOrDefault();
                    if (assignbrand != null)
                    {
                        BrandNamelist.Add(assignbrand);
                    }
                }
                tblDivision division = alldivision.Where(x => x.divisionName.Trim().ToLower() == Division.Trim().ToLower()).FirstOrDefault();
                if (!string.IsNullOrEmpty(selectedmonth))
                {
                    int currentmonth = Convert.ToInt32(DateTime.Now.Month);
                    int passmonth = Convert.ToInt32(selectedmonth);
                    if (passmonth == currentmonth)
                    {
                        string eddate = string.Format("{0:dd-MM-yyyy}", enddate);
                        string stdate = "01-" + selectedmonth + "-" + DateTime.Now.Year.ToString();
                        startdate = DateTime.ParseExact(stdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        enddate = DateTime.ParseExact(eddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        int year = DateTime.Now.Year;
                        if (passmonth > 3 && currentmonth < 4)
                            year = DateTime.Now.Year - 1;
                        else if (currentmonth > 3 && passmonth < 4)
                            year = DateTime.Now.Year + 1;

                        startdate = DateTime.ParseExact("01-" + selectedmonth + "-" + year, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        enddate = startdate.AddMonths(1);
                        enddate = enddate.AddDays(-1);
                    }
                }
                else
                {
                    int currentmonth = Convert.ToInt32(DateTime.Now.Month);
                    int year = DateTime.Now.Year;
                    if (currentmonth < 4)
                        year = year - 1;
                    string eddate = string.Format("{0:dd-MM-yyyy}", enddate);
                    string stdate = "01-" + "04-" + year;
                    startdate = DateTime.ParseExact(stdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    enddate = DateTime.ParseExact(eddate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                if (!string.IsNullOrEmpty(SiteName))
                {
                    tblSite site = context.tblSites.Where(x => x.site_name.ToLower() == SiteName.ToLower()).FirstOrDefault();
                    var _SellDetails = context.SP_GetpspfddetailreportmonthwithSite(site.site_cuid, startdate, enddate);
                    try
                    {
                        foreach (var item in _SellDetails)
                        {
                            var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                            if (branddetails.height != null && branddetails.wallSpace != null)
                            {
                                saleThroughreankungViewModel detailSalemodel = new saleThroughreankungViewModel();
                                if (!brandresult.Exists(p => p.Equals(item.brandname)))
                                {
                                    brandresult.Add(item.brandname);
                                }
                                detailSalemodel.BrandName = item.brandname;
                                detailSalemodel.Quantity = item.saleQty;
                                detailSalemodel.Amount = item.netAmt;
                                detailSalemodellist.Add(detailSalemodel);
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (division != null)
                    {
                        var _StockDetailds = context.SP_GetsellthroughstockDetailswithsite(site.site_id, division.div_id, startdate, enddate);
                        try
                        {
                            foreach (var item in _StockDetailds)
                            {
                                var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                                if (branddetails.height != null && branddetails.wallSpace != null)
                                {
                                    stocksaleThroughreankungViewModel detailstockmodel = new stocksaleThroughreankungViewModel();
                                    if (!brandresult.Exists(p => p.Equals(item.brandname)))
                                    {
                                        brandresult.Add(item.brandname);
                                    }
                                    detailstockmodel.BrandName = item.brandname;
                                    detailstockmodel.closingQuantity = item.clqty;
                                    decimal amount = Math.Round(Convert.ToDecimal(item.clamt));
                                    detailstockmodel.closingAmount = Convert.ToInt32(amount);
                                    if (division.div_id == item.div_id)
                                    {
                                        detailstockmodel.Division = division.divisionName;
                                    }
                                    else
                                    {
                                        tblDivision getdivision = alldivision.Where(x => x.div_id == item.div_id).FirstOrDefault();
                                        detailstockmodel.Division = getdivision.divisionName;
                                    }
                                    detailstockmodellist.Add(detailstockmodel);
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                else
                {
                    var _SellDetails = context.SP_Getpspfddetailreportmonth(startdate, enddate);
                    try
                    {
                        foreach (var item in _SellDetails)
                        {
                            var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                            if (branddetails.height != null && branddetails.wallSpace != null)
                            {
                                saleThroughreankungViewModel detailSellmodel = new saleThroughreankungViewModel();
                                if (!brandresult.Exists(p => p.Equals(item.brandname)))
                                {
                                    brandresult.Add(item.brandname);
                                }
                                detailSellmodel.BrandName = item.brandname;
                                detailSellmodel.Quantity = item.saleQty;
                                detailSellmodel.Amount = item.netAmt;
                                detailSalemodellist.Add(detailSellmodel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (division != null)
                    {
                        var _StockDetailds = context.SP_GetsellthroughstockDetailsdivision(division.div_id, startdate, enddate);
                        try
                        {
                            foreach (var item in _StockDetailds)
                            {
                                var branddetails = (from c in allbrands where c.brandName == item.brandname select c).FirstOrDefault();
                                if (branddetails.height != null && branddetails.wallSpace != null)
                                {
                                    stocksaleThroughreankungViewModel detailstockmodel = new stocksaleThroughreankungViewModel();
                                    if (!brandresult.Exists(p => p.Equals(item.brandname)))
                                    {
                                        brandresult.Add(item.brandname);
                                    }
                                    detailstockmodel.BrandName = item.brandname;
                                    detailstockmodel.closingQuantity = item.clqty;
                                    decimal amount = Math.Round(Convert.ToDecimal(item.clamt));
                                    detailstockmodel.closingAmount = Convert.ToInt32(amount);
                                    if (division.div_id == item.div_id)
                                    {
                                        detailstockmodel.Division = division.divisionName;
                                    }
                                    else
                                    {
                                        tblDivision getdivision = alldivision.Where(x => x.div_id == item.div_id).FirstOrDefault();
                                        detailstockmodel.Division = getdivision.divisionName;
                                    }
                                    detailstockmodellist.Add(detailstockmodel);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                foreach (var result in brandresult)
                {
                    var branddetails = (from c in allbrands where c.brandName == result select c).FirstOrDefault();
                    var saleinfo = (from c in detailSalemodellist where c.BrandName == result select c).FirstOrDefault();
                    var stockinfo = (from c in detailstockmodellist where c.BrandName == result select c).FirstOrDefault();
                    saleThroughrankungViewModel sellthroughreportmodel = new saleThroughrankungViewModel();
                    sellthroughreportmodel.BrandName = result;
                    sellthroughreportmodel.BrandCategory = branddetails.brandCategory;
                    sellthroughreportmodel.Division = Division;
                    if (stockinfo != null)
                    {
                        sellthroughreportmodel.StockclosingQuantity = stockinfo.closingQuantity;
                        sellthroughreportmodel.StockclosingAmount = stockinfo.closingAmount;
                    }
                    else
                    {
                        sellthroughreportmodel.StockclosingQuantity = 0;
                        sellthroughreportmodel.StockclosingAmount = 0;
                    }
                    if (saleinfo != null)
                    {
                        sellthroughreportmodel.SaleQuantity = saleinfo.Quantity;
                        sellthroughreportmodel.SaleAmount = saleinfo.Amount;
                    }
                    else
                    {
                        sellthroughreportmodel.SaleQuantity = 0;
                        sellthroughreportmodel.SaleAmount = 0;
                    }
                    sellthroughreportmodel.saleThroughReport = Math.Round(Convert.ToDouble((double)sellthroughreportmodel.SaleQuantity / (sellthroughreportmodel.SaleQuantity + sellthroughreportmodel.StockclosingQuantity) * 100), 2);
                    salethroughreportlist.Add(sellthroughreportmodel);
                }
                int counter = 0;
                string cat = string.Empty;
                double entiresalethrough = 0;
                foreach (var salethrough in salethroughreportlist.OrderBy(x => x.BrandCategory).ThenByDescending(x => x.saleThroughReport))
                {
                    if (string.IsNullOrEmpty(cat) || cat == salethrough.BrandCategory)
                        counter = counter + 1;
                    else
                        counter = 1;
                    if (BrandNamelist.Exists(p => p.brandCategory.Equals(salethrough.BrandCategory)))
                    {
                        if (entiresalethrough == 0)
                        {
                            int saleqty = (from c in salethroughreportlist select c.SaleQuantity).Sum();
                            int stkqty = (from c in salethroughreportlist select c.StockclosingQuantity).Sum();
                            entiresalethrough = Math.Round(Convert.ToDouble((double)saleqty / (saleqty + stkqty) * 100), 2);

                        }
                        saleThroughrankungViewModel sellthroughreportmodel = new saleThroughrankungViewModel();
                        sellthroughreportmodel.BrandName = salethrough.BrandName;
                        sellthroughreportmodel.BrandCategory = salethrough.BrandCategory;
                        sellthroughreportmodel.Ranking = counter;
                        sellthroughreportmodel.Division = salethrough.Division;
                        sellthroughreportmodel.SaleQuantity = salethrough.SaleQuantity;
                        sellthroughreportmodel.SaleAmount = salethrough.SaleAmount;
                        sellthroughreportmodel.StockclosingQuantity = salethrough.StockclosingQuantity;
                        sellthroughreportmodel.StockclosingAmount = salethrough.StockclosingAmount;
                        sellthroughreportmodel.saleThroughReport = salethrough.saleThroughReport;
                        sellthroughreportmodel.EntireBrandsaleThroughReport = entiresalethrough;
                        if (counter < 5)
                        {
                            Newsalethroughreportlist.Add(sellthroughreportmodel);
                        }
                        else
                        {
                            if (BrandNamelist.Exists(p => p.brandName.Equals(salethrough.BrandName)))
                            {
                                Newsalethroughreportlist.Add(sellthroughreportmodel);
                            }
                        }

                    }

                    cat = salethrough.BrandCategory;

                }
                Session["DownloadsalethroughReport"] = Newsalethroughreportlist;

            }
            return PartialView("~/Views/Report/_salethroughreportpartial.cshtml", Newsalethroughreportlist);

        }
        public ActionResult DownloadsalethroughReport()
        {
            List<saleThroughrankungViewModel> ModelResultdata = Session["DownloadsalethroughReport"] as List<saleThroughrankungViewModel>;
            DataTable dt = new DataTable("SaleThroughRankingReport");
            dt.Columns.AddRange(new DataColumn[10] {
                                            new DataColumn("Brand Name"),
                                            new DataColumn("Ranking",typeof(Int32)),
                                            new DataColumn("Category"),
                                            new DataColumn("Division"),
                                            new DataColumn("Sale Qty",typeof(Int32)),
                                            new DataColumn("Sale Amt",typeof(Int32)),
                                            new DataColumn("Cl Qty",typeof(Int32)),
                                            new DataColumn("Cl Amt",typeof(Int32)),
                                            new DataColumn("Sale Through",typeof(double)),
                                            new DataColumn("Entire Sale Through",typeof(double))});
            foreach (var rslt in ModelResultdata)
            {
                dt.Rows.Add(rslt.BrandName, rslt.Ranking, rslt.BrandCategory, rslt.Division, rslt.SaleQuantity, rslt.SaleAmount, rslt.StockclosingQuantity, rslt.StockclosingAmount, rslt.saleThroughReport, rslt.EntireBrandsaleThroughReport);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SaleThroughRankingReport.xlsx");
                }
            }
        }
    }
}