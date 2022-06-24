using BCInsight.BAL.Repository;
using BCInsight.DAL;
using System.Collections.Generic;
using System.Linq;

namespace BCInsight.BAL.Services
{
    public class StockRepository : GenericRepository<Vendor_bcInsightEntities, tblstock>, IStock
    {
        //public void GetAllStock()
        //{
        //    using (bcInsightEntities entities = new bcInsightEntities())
        //    {
        //        var stockall = (from st in entities.tblstocks 
        //                     join tsite in entities.tblSites on st.site_id equals tsite.site_id
        //                        join tdiv in entities.tblDivisions on st.div_id equals tdiv.div_id
        //                        join tsec in entities.tblSections on st.sec_id equals tsec.sec_id
        //                        join tdep in entities.tblDepartments on st.dep_id equals tdep.dep_id
        //                        join tbrnd in entities.tblBrands on st.brand_id equals tbrnd.brand_id
        //                        join tcol in entities.tblColors on st.color_id equals tcol.color_id
        //                        join tsize in entities.tblSizes on st.size_id equals tsize.size_id
        //                        select new
        //                        {
        //                            stock_id = st.stock_id,
        //                            barcode = st.barcode,
        //                            site_name = tsite.site_name,
        //                            divisionName = tdiv.divisionName,
        //                            sectionName = tsec.sectionName,
        //                            departmentName = tdep.departmentName,
        //                            brandName = tbrnd.brandName,
        //                            styleCode = st.styleCode,
        //                            colorName = tcol.colorName,
        //                            sizeName = tsize.sizeName,
        //                            fit = st.fit,
        //                            closingTotal = st.closingTotal,
        //                            combination = st.combination,
        //                            siteCuid = st.siteCuid,
        //                            category2 = st.category2,
        //                            desc4 = st.desc4,
        //                            desc6 = st.desc6,
        //                            mrp = st.mrp,
        //                            vendorName = st.vendorName

        //                        }).ToList();
        //    };
        //}
    }
}
