﻿using BCInsight.BAL.Repository;
using BCInsight.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCInsight.BAL.Services
{
    public class SiteSetttingRepository:GenericRepository<Vendor_bcInsightEntities, tblSiteSetting>,ISiteSetting
    {
    }
}
