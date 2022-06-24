using BCInsight.BAL.Repository;
using BCInsight.DAL;


namespace BCInsight.BAL.Services
{
    public class SalesRepository : GenericRepository<Vendor_bcInsightEntities, tblsale>, ISales
    {
    }
}
