using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class VendorLoginRepository : GenericRepository<Vendor_bcInsightEntities, tblVendorLoginHistory>, IvendorLogin
    {
    }
}
