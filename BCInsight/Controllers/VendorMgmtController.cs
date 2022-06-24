using BCInsight.BAL.Repository;
using BCInsight.Code;
using BCInsight.DAL;
using BCInsight.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCInsight.Controllers
{
    public class VendorMgmtController : BaseController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAttendance _attendance;
        private readonly IVendor _vendor;
        private readonly IUsers _user;
        private readonly IVendorBrand _vendorbrand;
        private readonly IBrand _brand;
        private readonly IvendorLogin _vendorlogin;
        private readonly IDivision _division;
        private readonly ISite _site;
        private readonly IVendorsaleperson _vendorsaleperson;

        public VendorMgmtController(IAttendance attendance, IVendor vendor, IVendorBrand vendorbrand, IBrand brand, IvendorLogin vendorlogin, IVendorsaleperson vendorsaleperson, IUsers user, ISite site, IDivision division)
        {
            _attendance = attendance;
            _vendor = vendor;
            _vendorbrand = vendorbrand;
            _brand = brand;
            _vendorlogin = vendorlogin;
            _vendorsaleperson = vendorsaleperson;
            _user = user;
            _site = site;
            _division = division;
        }
        // GET: VendorMgmt
        public ActionResult Index()
        {
            List<AttendanceMgmtViewModel> attendanceMgmt = new List<AttendanceMgmtViewModel>();
            try
            {
                if (TempData["result"] == null)
                {
                    List<tblAttendance> Attendance = _attendance.GetAll().OrderBy(c => c.id).ToList();
                    foreach (var item in Attendance)
                    {
                        attendanceMgmt.Add(new AttendanceMgmtViewModel(item));
                    }
                }
                else
                {
                    List<AttendanceMgmtViewModel> Modelresult = TempData["result"] as List<AttendanceMgmtViewModel>;
                    foreach (var item in Modelresult)
                    {
                        attendanceMgmt.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return View(attendanceMgmt);
        }


        public ActionResult UpdateVendorManagement(int id)
        {
            VendorMgmtViewModel addvendor = new VendorMgmtViewModel();
            try
            {
                addvendor = new VendorMgmtViewModel(_vendor.FindBy(c => c.id == id).FirstOrDefault());
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return View(addvendor);
        }
        [HttpPost]
        public ActionResult UpdateVendorManagement(VendorMgmtViewModel model)
        {
            try
            {
                tblVendor isAvaiblevendorname = _vendor.FindBy(c => c.email.ToLower() == model.email.ToLower()).FirstOrDefault();
                if (isAvaiblevendorname != null)
                {
                    if (model.id != isAvaiblevendorname.id)
                    {
                        ModelState.AddModelError("email", "Email Must Be Unique.");
                        return View("UpdateVendorManagement", model);
                    }
                }
                tblVendor vendors = _vendor.FindBy(c => c.id == model.id).FirstOrDefault() ?? new tblVendor();
                vendors.firstName = model.firstName;
                vendors.lastName = model.lastName;
                vendors.email = model.email;

                _vendor.Edit(vendors);
                _vendor.Save();
                this.SetNotification("Vendor Updated Successfully", NotificationEnum.Success);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return RedirectToAction("UpdateVendorManagement", new { id = model.id});
        }

    }
}