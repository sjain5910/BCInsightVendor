using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class SectionRepository : GenericRepository<Vendor_bcInsightEntities, tblSection>, ISection
    {
    }
}
