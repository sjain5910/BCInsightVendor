using BCInsight.BAL.Repository;
using BCInsight.Code;
using BCInsight.DAL;
using BCInsight.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace BCInsight.Controllers
{
    public class LoginController : BaseController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IVendor _vendorlogin;
        private readonly IvendorLogin _vendorloginSave;        
        public LoginController(IVendor vendorlogin, IvendorLogin vendorloginSave)
        {
            _vendorlogin = vendorlogin;
            _vendorloginSave = vendorloginSave;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Login(VendorportalloginViewModel model)
        {
            try
            {
                var cu = new HttpCookie("id");
                cu.Expires = DateTime.Now.AddMinutes(-1);
                cu.Value = "";
                Response.Cookies.Add(cu);

                if (ModelState.IsValid)
                {
                    tblVendor isActiveVendor = _vendorlogin.FindBy(c => c.email == model.email && c.password == model.password).FirstOrDefault();
                    if (isActiveVendor != null)
                    {
                        if (isActiveVendor.isActive)
                        {
                            var c = new HttpCookie("id");
                            c.Expires = DateTime.Now.AddYears(+1);
                            c.Value = isActiveVendor.id.ToString();
                            Response.Cookies.Add(c);
                            FormsAuthentication.SetAuthCookie(isActiveVendor.id.ToString(), true);
                            Session["id"] = isActiveVendor.id;
                            string hostName = Dns.GetHostName();
                            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                            tblVendorLoginHistory savelogin = new tblVendorLoginHistory();
                            savelogin.id = 0;
                            savelogin.vendorId = isActiveVendor.id;
                            savelogin.loginDate = DateTime.Now;
                            savelogin.ip = myIP;
                            _vendorloginSave.Add(savelogin);
                            _vendorloginSave.Save();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["loginfail"] = "User not active";
                        }
                    }
                    else
                    {
                        TempData["loginfail"] = "Wrong username or password";
                    }
                }
                else
                {
                    TempData["loginfail"] = "Enter username or password";
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
                this.SetNotification("Check Internet Connection", NotificationEnum.Warning);
            }
            return RedirectToAction("Index", "Login");
        }


        public ActionResult Logout()
        {
            int userId = Convert.ToInt32(Session["id"]);            
            FormsAuthentication.SignOut();
            Session["id"] = null;
            Session.Abandon();
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Request.Cookies.Add(cookie);

            var c = new HttpCookie("id");
            c.Expires = DateTime.Now.AddMinutes(-1);
            c.Value = "";
            Response.Cookies.Add(c);

            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChangePassword()
        {
            VendorportalloginViewModel model = new VendorportalloginViewModel();
            int id = Convert.ToInt32(Session["id"]);
            tblVendor UpdateVendor = _vendorlogin.FindBy(c => c.id == id).FirstOrDefault();
            model.id = id;
            model.fullname = UpdateVendor.firstName.Trim() + " " + UpdateVendor.lastName.Trim();
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(VendorportalloginViewModel model)
        {            
            try
            {                
                model.id = Convert.ToInt32(Session["id"]);
                tblVendor UpdateVendor = _vendorlogin.FindBy(c => c.id == model.id).FirstOrDefault();
                if(model.Oldpassword!= UpdateVendor.password)
                {
                    ModelState.AddModelError("Oldpassword", "old password not currect");
                    return View(model);
                }
                UpdateVendor.password = model.Newpassword;
                _vendorlogin.Edit(UpdateVendor);
                _vendorlogin.Save();
                this.SetNotification("Password Updated Successfully", NotificationEnum.Success);
                return RedirectToAction("Logout");
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
           
            return View();
        }



    }
}