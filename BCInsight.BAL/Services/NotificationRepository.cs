using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class NotificationRepository : GenericRepository<Vendor_bcInsightEntities, tblNotification>, INotification
    {
    }
}
