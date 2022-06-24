using BCInsight.BAL.Repository;
using BCInsight.DAL;
using System;
using System.Linq;

namespace BCInsight.BAL.Services
{
    public class WeeksRepository : GenericRepository<Vendor_bcInsightEntities, tblWeekMaster>, IWeeks  
    {            
        public void Startshedular()
        {
            string fy = GetCurrentFinancialYear();
            tblWeekMaster weekmodel = FindBy(c => c.finYear == fy).FirstOrDefault();
            //var weekmodel = entities.tblWeekMasters.Where(c => c.finYear == fy).FirstOrDefault();
            if (weekmodel == null)
            {
                int CurrentYear = DateTime.Today.Year;
                int NextYear = DateTime.Today.Year + 1;
                DateTime dt = new DateTime(CurrentYear, 4, 1);
                while (dt.DayOfWeek != DayOfWeek.Monday)
                {
                    dt = dt.AddDays(1);
                }
                DateTime dt3 = new DateTime(NextYear, 3, 31);
                while (dt3.DayOfWeek != DayOfWeek.Sunday)
                {
                    dt3 = dt3.AddDays(1);
                }
                DateTime yearstartdate = Convert.ToDateTime(dt);
                DateTime yearenddate = Convert.ToDateTime(dt3);
                int Noofweek = NumberOfWeeks(yearstartdate, yearenddate);
                DateTime Weekstartdate = Convert.ToDateTime(dt); //DateTime.ParseExact(model.startDatestr, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime Weekenddate = Weekstartdate.AddDays(6);
                for (int i = 1; i <= Noofweek; i++)
                {
                    tblWeekMaster saveweek = new tblWeekMaster();
                    saveweek.finYear = fy;
                    saveweek.weekNo = i;
                    saveweek.startDate = Weekstartdate;
                    saveweek.endDate = Weekenddate;
                    Add(saveweek);
                    Save();
                    Weekstartdate = Weekenddate.AddDays(1);
                    Weekenddate = Weekstartdate.AddDays(6);
                }
            }

        }

        public static int NumberOfWeeks(DateTime dateFrom, DateTime dateTo)
        {
            TimeSpan Span = dateTo.Subtract(dateFrom);

            if (Span.Days <= 7)
            {
                if (dateFrom.DayOfWeek > dateTo.DayOfWeek)
                {
                    return 2;
                }

                return 1;
            }

            int Days = Span.Days - 7 + (int)dateFrom.DayOfWeek;
            int WeekCount = 1;
            int DayCount = 0;

            for (WeekCount = 1; DayCount < Days; WeekCount++)
            {
                DayCount += 7;
            }

            return WeekCount;
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
    }
}
