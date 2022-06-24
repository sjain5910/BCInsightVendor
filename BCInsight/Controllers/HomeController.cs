using BCInsight.BAL.Repository;
using BCInsight.Code;
using BCInsight.DAL;
using BCInsight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BCInsight.Controllers
{
    public class HomeController : BaseController
    {

        //[AuthorizeUser(-1)]
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HomeController()
        {

        }
        public ActionResult Index()
        {
            int vendorId;
            List<string> BrandNamelist = new List<string>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                try
                {
                    var laststockUpdate = context.TblAdminLogs.Where(x => x.SectionName == "Stock" && x.Action == "Import").OrderByDescending(x => x.LogDate).FirstOrDefault();
                    var lastSaleUpdate = context.TblAdminLogs.Where(x => x.SectionName == "Sales" && x.Action == "Import").OrderByDescending(x => x.LogDate).FirstOrDefault();
                    if (laststockUpdate != null && lastSaleUpdate != null)
                    {
                        ViewBag.laststock = "Last Stock Updated : " + string.Format("{0:dd-MM-yyyy hh:mm tt}", laststockUpdate.LogDate);
                        ViewBag.lastsale = "Last Sales Updated : " + string.Format("{0:dd-MM-yyyy hh:mm tt}", lastSaleUpdate.LogDate);
                    }
                }
                catch
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ErrorLog()
        {
            List<TblLog> log = new List<TblLog>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                log = context.TblLogs.OrderByDescending(c => c.Id).ToList();
            }
            return View(log);
        }

        public ActionResult ApiLog()
        {
            List<TblApiLog> log = new List<TblApiLog>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                log = context.TblApiLogs.OrderByDescending(c => c.Id).ToList();
            }
            return View(log);
        }

        [HttpGet]
        public ActionResult Clearlog()
        {
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                context.TblLogs.RemoveRange(context.TblLogs);
                context.SaveChanges();
            }
            return RedirectToAction("ErrorLog");
        }

        public ActionResult AdminLog()
        {
            List<TblAdminLog> log = new List<TblAdminLog>();
            using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
            {
                log = context.TblAdminLogs.OrderByDescending(c => c.LogId).ToList();
            }
            return View(log);
        }
    }
}