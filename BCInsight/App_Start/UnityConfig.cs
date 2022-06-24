using BCInsight.BAL.Repository;
using BCInsight.BAL.Services;
using System;

using Unity;

namespace BCInsight
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ISite, SiteRepository>();
            container.RegisterType<IDesiMgmt, DesiMgmtRepository>();
            container.RegisterType<IUsers, UsersRepository>();
            container.RegisterType<IWeeks, WeeksRepository>();
            container.RegisterType<ISales, SalesRepository>();
            container.RegisterType<IStock, StockRepository>();
            container.RegisterType<IQuarter, QuartersRepository>();
            container.RegisterType<IQuantity, BaseQtyRepository>();
            container.RegisterType<ISection, SectionRepository>();
            container.RegisterType<IDivision, DivisionRepository>();
            container.RegisterType<IDepartment, DepartmentsRepository>();
            container.RegisterType<IBrand, BrandsRepository>();
            container.RegisterType<IColor, ColorRepository>();
            container.RegisterType<ISize, SizeRepository>();
            container.RegisterType<ISlabs, SlabsRepository>();
            container.RegisterType<IWeeklyIncentive, WeeklyIncentiveRepository>();
            container.RegisterType<IAttendance, AttendanceRepository>();
            container.RegisterType<IVendor, VendorRepository>();
            container.RegisterType<IVendorBrand, VendorBrandRepository>();
            container.RegisterType<IvendorLogin, VendorLoginRepository>();
            container.RegisterType<IAdminUser, AdminUserRepository>();
            container.RegisterType<INotification, NotificationRepository>();
            container.RegisterType<IPvtLabelGroup, PvtLabelGroupRepository>();
            container.RegisterType<IPvtLabelName, PvtLabelNameRepository>();
            container.RegisterType<ILabelSlab, LabelSlabRepository>();
            container.RegisterType<ISiteSetting, SiteSetttingRepository>();
            container.RegisterType<ImultibrandIncentiveRule, MultibrandIncentiveRepository>();
            container.RegisterType<IVendorsaleperson, VendorsalePersonRepository>();
            
        }
    }
}