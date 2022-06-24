using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCInsight.Models
{
    public class AttendanceMgmtViewModel
    {
        public AttendanceMgmtViewModel()
        {

        }

        public AttendanceMgmtViewModel(tblAttendance attendance)
        {
            if(attendance!=null)
            {
                salePersonNo = attendance.salePersonNo;
                AttendanceDate = attendance.AttendanceDate;
                status = attendance.status;
                firstintime = attendance.firstintime;
                secondouttime = attendance.secondouttime;
                thirdintime = attendance.thirdintime;
                fourthouttime = attendance.fourthouttime;
                totalHours = attendance.totalHours;
            }
        }

        public int id { get; set; }
        public int? salePersonNo { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string status { get; set; }
        public string firstintime { get; set; }
        public string secondouttime { get; set; }
        public string thirdintime { get; set; }
        public string fourthouttime { get; set; }
        public string totalHours { get; set; }
    }
}