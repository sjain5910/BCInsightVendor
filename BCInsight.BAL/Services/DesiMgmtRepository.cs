using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class DesiMgmtRepository : GenericRepository<Vendor_bcInsightEntities, tblDesignation>, IDesiMgmt
    {
    }
}
