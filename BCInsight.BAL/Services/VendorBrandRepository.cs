using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class VendorBrandRepository : GenericRepository<Vendor_bcInsightEntities, tblVendorBrand>, IVendorBrand
    {
    }
}
