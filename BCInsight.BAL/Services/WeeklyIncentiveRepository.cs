using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class WeeklyIncentiveRepository : GenericRepository<Vendor_bcInsightEntities, tblWeeklytargetincentive>, IWeeklyIncentive
    {
    }
}
