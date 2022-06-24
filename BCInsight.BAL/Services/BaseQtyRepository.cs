using BCInsight.BAL.Repository;
using BCInsight.DAL;


namespace BCInsight.BAL.Services
{
    public class BaseQtyRepository : GenericRepository<Vendor_bcInsightEntities, tblbaseqty>, IQuantity
    {
    }
}
