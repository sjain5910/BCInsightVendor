using BCInsight.Code;
using BCInsight.DAL;
using BCInsight.Web.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BCInsight.Controllers
{
    public class BaseController : Controller
    {

        private static readonly log4net.ILog Log =
          log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DAL.Vendor_bcInsightEntities entities = new DAL.Vendor_bcInsightEntities();
        public string result;

        public LoginUser CurrentUser;
        public BaseController()
        {
            CurrentUser = Utility.CurrentLoginUser;
            
        }
        public int LoginUserId()
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string UserName = authTicket.Name;
                List<string> s = new List<string>(UserName.Split(new string[] { "|" }, StringSplitOptions.None));
                return Convert.ToInt32(s.FirstOrDefault());
            }
            return 0;
        }


        /// <summary>
        /// Sets the information for the system notification.
        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="autoHideNotification">Determines whether the notification will stay visible or auto-hide.</param>
        /// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
        public void SetNotification(string message, NotificationEnum notifyType, bool autoHideNotification = true)
        {
            this.TempData["Notification"] = message;
            this.TempData["NotificationAutoHide"] = (autoHideNotification) ? "true" : "false";

            switch (notifyType)
            {
                case NotificationEnum.Success:
                    this.TempData["NotificationCSS"] = "notificationbox notification-success";
                    break;
                case NotificationEnum.Error:
                    this.TempData["NotificationCSS"] = "notificationbox notification-danger";
                    break;
                case NotificationEnum.Warning:
                    this.TempData["NotificationCSS"] = "notificationbox notification-warning";
                    break;
                case NotificationEnum.Info:
                    this.TempData["NotificationCSS"] = "notificationbox notification-info";
                    break;
            }

        }

        [Obsolete]
        public void saveadminlog(TblAdminLog adminlogmodel)
        {
            try
            {
                entities.TblAdminLogs.Add(adminlogmodel);
                entities.SaveChanges();
            }
            catch(Exception e)
            {

            }
            
        }

        public string getdaysDifferance(string Operationdate)
        {
            DateTime d1 = DateTime.Now;
            string d2 = Operationdate;
            DateTime d3 = Convert.ToDateTime(d2);
            double total_days = (d1 - d3).TotalDays;
            int daydiff = Convert.ToInt32(total_days);
            if (daydiff > 0)
            {
                if (daydiff == 1)
                {
                    result = " (Yesterday)";
                }
                else if (daydiff > 1 && daydiff < 30)
                {
                    double day = daydiff;
                    int daysdiff = Convert.ToInt32(day);
                    result = " (" + daysdiff + " Days Ago)";
                }
                else if (daydiff >= 30 && daydiff <= 365)
                {
                    double month = daydiff / 30;
                    int monthdiff = Convert.ToInt32(month);
                    result = " (" + monthdiff + " Month Ago)";
                }
                else if (daydiff > 365)
                {
                    double year = daydiff / 365;
                    int yeardiff = Convert.ToInt32(year);
                    result = " (" + yeardiff + " Year Ago)";
                }
            }
            else
            {
                result = " (Today)";
            }
            return result;
        }

        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;
            string result = null;

            if (DateTime.Today.Month > 3)
            {
                result = NexYear.Substring(NexYear.Length - 2);
                NexYear = result.ToString();
                FinYear = CurYear + "-" + NexYear;
            }
            else
            {
                result = CurYear.Substring(CurYear.Length - 2);
                CurYear = result.ToString();
                FinYear = PreYear + "-" + CurYear;
            }
            return FinYear.Trim();
        }

        public static string GetPreviousFinancialYear()
        {
            int CurrentYear;
            int PreviousYear;
            int Currentmonth = DateTime.Today.Month;
            if(Currentmonth<=3)
            {
                CurrentYear = DateTime.Today.Year - 1;
                PreviousYear = DateTime.Today.Year - 2;
            }
            else
            {
                CurrentYear = DateTime.Today.Year;
                PreviousYear = DateTime.Today.Year - 1;
            }            
            string PreYear = PreviousYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;
            string result = null;
            result = CurYear.Substring(CurYear.Length - 2);
            CurYear = result.ToString();
            FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }

    }
}