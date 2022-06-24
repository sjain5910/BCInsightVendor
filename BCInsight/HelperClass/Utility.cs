﻿using BCInsight.Code;
using BCInsight.DAL;
using BCInsight.Models;
using System;
using System.Linq;
using System.Web;

namespace BCInsight.Web.HelperClass
{
    public class Utility
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LoginUser CurrentLoginUser => GetLoggedInUser();

        private static LoginUser GetLoggedInUser()
        {
            try
            {
                Int32 userid = 0;
                HttpCookie myCookie1 = HttpContext.Current.Request.Cookies["id"];
                if (myCookie1 != null && !string.IsNullOrEmpty(myCookie1.Value))
                    userid = Convert.ToInt32(myCookie1.Value);
                    HttpContext.Current.Session["id"] = userid;

                if (userid <= 0)
                    userid = HttpContext.Current.Session["id"] != null ? Convert.ToInt32(HttpContext.Current.Session["id"]) : 0;

                using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                {
                    dynamic user = null;

                    if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        int uid = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                        user = context.tblVendors.FirstOrDefault(m => m.id == uid);
                       
                    }
                    if (user == null || user.id == 0)
                    {
                        user = context.tblVendors.FirstOrDefault(m => m.id == userid);
                        if (user == null || user.id == 0)
                            return null;
                    }

                    var loginUser = new LoginUser
                    {
                        id= user.id,
                        LoginId = user.email,
                        Name = user.firstName +" " + user.lastName,
                        IsLogin = true
                    };

                    return loginUser;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error from BCInsight Utility User : " + e); Log.Error("Error from BCInsight Utility User : " + e.Message);

            }
            return null;
        }
       
    }
}