using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class VendorRepository : GenericRepository<Vendor_bcInsightEntities, tblVendor>, IVendor
    {
    }
}
