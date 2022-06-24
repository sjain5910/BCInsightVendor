using BCInsight.BAL.Repository;
using BCInsight.DAL;

namespace BCInsight.BAL.Services
{
    public class UsersRepository : GenericRepository<Vendor_bcInsightEntities, tblUser>, IUsers
    {
    }
}
