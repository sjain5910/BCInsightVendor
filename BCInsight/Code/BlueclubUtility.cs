using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace BCInsight.Code
{
    public class BlueclubUtility
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LoginUser GetLoggedInUser()
        {
            try
            {
                var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                Log.Info("authCookie " + authCookie);
                if (authCookie != null)
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    string UserName = authTicket.Name;
                    using (Vendor_bcInsightEntities context = new Vendor_bcInsightEntities())
                    {

                        List<string> s = new List<string>(UserName.Split(new string[] { "|" }, StringSplitOptions.None));
                        var loginEmailId = s.LastOrDefault()?.ToString();
                        //var user ; //context.Tblusers.FirstOrDefault(m => m.Email == loginEmailId && m.IsActive == true && m.IsDelete == false);
                        //if (user == null || user.Userid == 0)
                        //    return new LoginUser();

                        var loginUser = new LoginUser();
                        //{
                        //    UserId = user.Userid,
                        //    Name = user.FullName,
                        //    Email = user.Email,
                        //    IsLogin = true
                        //};
                        return loginUser;
                    }

                }
                else
                {
                    return new LoginUser();
                }
            }
            catch (Exception e)
            {

                Log.Error(e);
                return new LoginUser();
                throw;
            }
        }

        public static string DateToMillisecond(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var ms = (long)(date - epoch).TotalMilliseconds;
            return ms.ToString();
        }

        public static DateTime MillisecondToDate(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public static string GenerateOTP(int length)
        {
            const string valid = "0123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();

        }

        public static string GenerateAvtarImage(String ImgStr)
        {
            try
            {
                string fileName = ".jpg";
                var path = HttpContext.Current.Server.MapPath("~/Upload/CoverImage/");
                string uniqueFileName = Guid.NewGuid() + "_" + fileName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                ImgStr = ImgStr.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(ImgStr);
                var fs = new FileStream(path + "/" + uniqueFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
                fs.Close();
                fs.Dispose();
                return "~/Upload/CoverImage/" + uniqueFileName;
            }
            catch (Exception e)
            {
                return "";
            }

            return "";
        }
    }
}