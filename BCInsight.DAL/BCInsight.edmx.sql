
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/10/2022 10:53:19
-- Generated from EDMX file: D:\Aegis Project\BCInsight\BCInsight\BCInsight.DAL\BCInsight.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [admin_bcInsight];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TblAdminLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TblAdminLog];
GO
IF OBJECT_ID(N'[dbo].[tblAdminUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblAdminUser];
GO
IF OBJECT_ID(N'[dbo].[TblApiLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TblApiLog];
GO
IF OBJECT_ID(N'[dbo].[tblAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblAttendance];
GO
IF OBJECT_ID(N'[dbo].[tblbaseqty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblbaseqty];
GO
IF OBJECT_ID(N'[dbo].[tblBrand]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblBrand];
GO
IF OBJECT_ID(N'[dbo].[tblColor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblColor];
GO
IF OBJECT_ID(N'[dbo].[tblDepartment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDepartment];
GO
IF OBJECT_ID(N'[dbo].[tblDesignation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDesignation];
GO
IF OBJECT_ID(N'[dbo].[tblDivision]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblDivision];
GO
IF OBJECT_ID(N'[dbo].[TblLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TblLog];
GO
IF OBJECT_ID(N'[dbo].[tblMultiBrandIncentiveRule]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblMultiBrandIncentiveRule];
GO
IF OBJECT_ID(N'[dbo].[tblNotification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblNotification];
GO
IF OBJECT_ID(N'[dbo].[tblPvtLabelGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPvtLabelGroups];
GO
IF OBJECT_ID(N'[dbo].[tblPvtLabelName]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPvtLabelName];
GO
IF OBJECT_ID(N'[dbo].[tblPvtLabelSlabs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblPvtLabelSlabs];
GO
IF OBJECT_ID(N'[dbo].[tblQuarters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblQuarters];
GO
IF OBJECT_ID(N'[dbo].[tblsales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblsales];
GO
IF OBJECT_ID(N'[dbo].[tblSection]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSection];
GO
IF OBJECT_ID(N'[dbo].[tblSites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSites];
GO
IF OBJECT_ID(N'[dbo].[tblSiteSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSiteSetting];
GO
IF OBJECT_ID(N'[dbo].[tblSize]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSize];
GO
IF OBJECT_ID(N'[dbo].[tblSlabs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblSlabs];
GO
IF OBJECT_ID(N'[dbo].[tblstock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblstock];
GO
IF OBJECT_ID(N'[dbo].[tblUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblUsers];
GO
IF OBJECT_ID(N'[dbo].[tblVendor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblVendor];
GO
IF OBJECT_ID(N'[dbo].[tblVendorBrands]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblVendorBrands];
GO
IF OBJECT_ID(N'[dbo].[tblVendorLoginHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblVendorLoginHistory];
GO
IF OBJECT_ID(N'[dbo].[tblVendorsalesperson]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblVendorsalesperson];
GO
IF OBJECT_ID(N'[dbo].[tblWeeklytargetincentive]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblWeeklytargetincentive];
GO
IF OBJECT_ID(N'[dbo].[tblWeekMaster]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tblWeekMaster];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tblSites'
CREATE TABLE [dbo].[tblSites] (
    [site_id] tinyint IDENTITY(1,1) NOT NULL,
    [site_cuid] varchar(50)  NOT NULL,
    [site_name] varchar(40)  NOT NULL,
    [site_location] varchar(50)  NULL,
    [site_city] varchar(50)  NULL,
    [site_email] varchar(50)  NULL,
    [contact_no] varchar(40)  NULL
);
GO

-- Creating table 'TblLogs'
CREATE TABLE [dbo].[TblLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Thread] varchar(255)  NOT NULL,
    [Level] varchar(50)  NOT NULL,
    [Logger] varchar(255)  NOT NULL,
    [Message] varchar(4000)  NOT NULL,
    [Exception] varchar(2000)  NULL
);
GO

-- Creating table 'tblWeekMasters'
CREATE TABLE [dbo].[tblWeekMasters] (
    [weekid] int IDENTITY(1,1) NOT NULL,
    [finYear] varchar(10)  NULL,
    [weekNo] int  NULL,
    [startDate] datetime  NULL,
    [endDate] datetime  NULL
);
GO

-- Creating table 'tblBrands'
CREATE TABLE [dbo].[tblBrands] (
    [brand_id] int IDENTITY(1,1) NOT NULL,
    [brandName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblColors'
CREATE TABLE [dbo].[tblColors] (
    [color_id] int IDENTITY(1,1) NOT NULL,
    [colorName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblDepartments'
CREATE TABLE [dbo].[tblDepartments] (
    [dep_id] int IDENTITY(1,1) NOT NULL,
    [departmentName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblDivisions'
CREATE TABLE [dbo].[tblDivisions] (
    [div_id] int IDENTITY(1,1) NOT NULL,
    [divisionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblsales'
CREATE TABLE [dbo].[tblsales] (
    [sale_id] int IDENTITY(1,1) NOT NULL,
    [import_date] datetime  NOT NULL,
    [saleDate] datetime  NOT NULL,
    [uniqueBillId] nvarchar(150)  NOT NULL,
    [billNo] nvarchar(100)  NOT NULL,
    [siteCuid] nvarchar(100)  NOT NULL,
    [department] nvarchar(100)  NOT NULL,
    [section] nvarchar(100)  NOT NULL,
    [product] nvarchar(100)  NOT NULL,
    [brandName] nvarchar(100)  NOT NULL,
    [cat6] nvarchar(100)  NOT NULL,
    [cat3] nvarchar(100)  NOT NULL,
    [cat4] nvarchar(100)  NOT NULL,
    [salesPersonNo] nvarchar(50)  NOT NULL,
    [itemDesc4] nvarchar(100)  NOT NULL,
    [cat2] nvarchar(100)  NOT NULL,
    [customerName] nvarchar(500)  NOT NULL,
    [customerMobile] nvarchar(100)  NOT NULL,
    [saleQty] int  NOT NULL,
    [mrpAmt] int  NOT NULL,
    [netAmt] decimal(18,2)  NOT NULL,
    [PrmoAmt] nvarchar(100)  NOT NULL,
    [ItemDesc6] nvarchar(100)  NOT NULL,
    [ItemDiscountAmt] nvarchar(100)  NOT NULL,
    [BillDiscountAmt] nvarchar(100)  NOT NULL,
    [LPDiscountAmt] nvarchar(100)  NOT NULL,
    [ExTaxAmtFactor] nvarchar(100)  NOT NULL,
    [flag_delete] int  NOT NULL,
    [pvt_label_group_id] int  NULL,
    [vendorName] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tblSections'
CREATE TABLE [dbo].[tblSections] (
    [sec_id] int IDENTITY(1,1) NOT NULL,
    [sectionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblSizes'
CREATE TABLE [dbo].[tblSizes] (
    [size_id] int IDENTITY(1,1) NOT NULL,
    [sizeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblQuarters'
CREATE TABLE [dbo].[tblQuarters] (
    [qtrid] int IDENTITY(1,1) NOT NULL,
    [finYear] varchar(10)  NOT NULL,
    [qtrNo] int  NOT NULL,
    [startDate] datetime  NOT NULL,
    [endDate] datetime  NOT NULL
);
GO

-- Creating table 'tblbaseqties'
CREATE TABLE [dbo].[tblbaseqties] (
    [baseid] int IDENTITY(1,1) NOT NULL,
    [barcode] nvarchar(50)  NOT NULL,
    [siteCuid] nvarchar(50)  NOT NULL,
    [division] nvarchar(50)  NOT NULL,
    [section] nvarchar(50)  NOT NULL,
    [department] nvarchar(50)  NOT NULL,
    [brand] nvarchar(50)  NOT NULL,
    [styleCode] nvarchar(50)  NOT NULL,
    [color] nvarchar(50)  NOT NULL,
    [fit] nvarchar(50)  NOT NULL,
    [size] nvarchar(50)  NOT NULL,
    [baseQty] int  NOT NULL,
    [lastUpdateDate] datetime  NOT NULL,
    [initQty] int  NOT NULL,
    [rptcurrQty] int  NOT NULL,
    [calreqtoAddQty] int  NOT NULL,
    [calrecvdQty] int  NOT NULL,
    [combination] nvarchar(100)  NOT NULL,
    [whinitqty] int  NOT NULL
);
GO

-- Creating table 'tblSlabs'
CREATE TABLE [dbo].[tblSlabs] (
    [id] int IDENTITY(1,1) NOT NULL,
    [lastUpdate] datetime  NOT NULL,
    [salesPersonNo] int  NOT NULL,
    [finYear] varchar(7)  NOT NULL,
    [qtrNo] int  NOT NULL,
    [slabNo] int  NOT NULL,
    [slabAmount] decimal(18,2)  NOT NULL,
    [monthlyIncentive] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'tblWeeklytargetincentives'
CREATE TABLE [dbo].[tblWeeklytargetincentives] (
    [id] int IDENTITY(1,1) NOT NULL,
    [salesPersonNo] int  NOT NULL,
    [finYear] nvarchar(10)  NOT NULL,
    [weekNo] int  NOT NULL,
    [weekTargetAmt] bigint  NOT NULL,
    [incentivePercent] decimal(4,2)  NOT NULL,
    [weekSalesAmt] decimal(9,2)  NOT NULL,
    [incentiveEarnedAmt] decimal(7,2)  NOT NULL,
    [dateEntered] datetime  NULL,
    [lastUpdatedDate] datetime  NULL,
    [lastCalcDate] datetime  NULL
);
GO

-- Creating table 'tblVendors'
CREATE TABLE [dbo].[tblVendors] (
    [id] int IDENTITY(1,1) NOT NULL,
    [email] nvarchar(100)  NULL,
    [firstName] nvarchar(50)  NULL,
    [lastName] nvarchar(50)  NULL,
    [password] nvarchar(15)  NULL,
    [dateCreated] datetime  NULL,
    [lastLoginDate] datetime  NULL,
    [lastLoginIp] int  NULL,
    [isActive] bit  NOT NULL,
    [vendorName] nvarchar(100)  NULL
);
GO

-- Creating table 'tblVendorBrands'
CREATE TABLE [dbo].[tblVendorBrands] (
    [id] int IDENTITY(1,1) NOT NULL,
    [vendorId] int  NULL,
    [brandId] int  NULL
);
GO

-- Creating table 'tblVendorLoginHistories'
CREATE TABLE [dbo].[tblVendorLoginHistories] (
    [id] int IDENTITY(1,1) NOT NULL,
    [vendorId] int  NULL,
    [loginDate] datetime  NULL,
    [ip] nvarchar(100)  NULL
);
GO

-- Creating table 'tblAdminUsers'
CREATE TABLE [dbo].[tblAdminUsers] (
    [admin_id] int IDENTITY(1,1) NOT NULL,
    [loginId] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [fullName] nvarchar(100)  NOT NULL,
    [userType] nvarchar(50)  NOT NULL,
    [isActive] bit  NOT NULL
);
GO

-- Creating table 'TblApiLogs'
CREATE TABLE [dbo].[TblApiLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [queryString] nvarchar(max)  NULL,
    [inputData] nvarchar(max)  NOT NULL,
    [ip] nvarchar(50)  NOT NULL,
    [logDate] datetime  NOT NULL
);
GO

-- Creating table 'tblstocks'
CREATE TABLE [dbo].[tblstocks] (
    [stock_id] int IDENTITY(1,1) NOT NULL,
    [barcode] nvarchar(50)  NOT NULL,
    [site_id] int  NOT NULL,
    [div_id] int  NOT NULL,
    [sec_id] int  NOT NULL,
    [dep_id] int  NOT NULL,
    [brand_id] int  NOT NULL,
    [styleCode] nvarchar(50)  NOT NULL,
    [color_id] int  NOT NULL,
    [size_id] int  NOT NULL,
    [fit] nvarchar(50)  NOT NULL,
    [closingTotal] int  NOT NULL,
    [combination] nvarchar(150)  NOT NULL,
    [siteCuid] nvarchar(50)  NOT NULL,
    [category2] nvarchar(50)  NULL,
    [desc4] nvarchar(50)  NULL,
    [desc6] nvarchar(50)  NULL,
    [mrp] nvarchar(50)  NULL,
    [vendorName] nvarchar(100)  NULL,
    [lastStockUpdate] datetime  NULL
);
GO

-- Creating table 'tblNotifications'
CREATE TABLE [dbo].[tblNotifications] (
    [notif_id] int IDENTITY(1,1) NOT NULL,
    [notificationType] nvarchar(20)  NULL,
    [fromUserId] int  NOT NULL,
    [fromSiteId] int  NOT NULL,
    [toSiteId] int  NOT NULL,
    [toEmail] nvarchar(50)  NOT NULL,
    [toMobile] nvarchar(10)  NOT NULL,
    [notificationText] nvarchar(max)  NOT NULL,
    [stockList] nvarchar(max)  NOT NULL,
    [statusFlag] nvarchar(20)  NOT NULL,
    [notificationDate] datetime  NOT NULL,
    [actionDate] datetime  NULL,
    [requestNote] nvarchar(20)  NULL,
    [clientReference] nvarchar(50)  NULL,
    [styleCode] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblPvtLabelGroups'
CREATE TABLE [dbo].[tblPvtLabelGroups] (
    [id] int IDENTITY(1,1) NOT NULL,
    [groupId] int  NULL,
    [department] nvarchar(50)  NULL,
    [brandName] nvarchar(50)  NULL,
    [product] nvarchar(50)  NULL
);
GO

-- Creating table 'tblPvtLabelNames'
CREATE TABLE [dbo].[tblPvtLabelNames] (
    [groupId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'tblPvtLabelSlabs'
CREATE TABLE [dbo].[tblPvtLabelSlabs] (
    [id] int IDENTITY(1,1) NOT NULL,
    [groupId] int  NULL,
    [finyear] nvarchar(50)  NULL,
    [qtrNo] nvarchar(10)  NULL,
    [startDate] datetime  NULL,
    [endDate] datetime  NULL,
    [qty] int  NULL,
    [amount] int  NULL
);
GO

-- Creating table 'tblUsers'
CREATE TABLE [dbo].[tblUsers] (
    [user_id] int IDENTITY(1,1) NOT NULL,
    [userId] varchar(15)  NOT NULL,
    [loginId] varchar(10)  NOT NULL,
    [password] varchar(15)  NOT NULL,
    [firstName] varchar(20)  NOT NULL,
    [lastName] varchar(20)  NOT NULL,
    [designationId] int  NOT NULL,
    [siteId] int  NOT NULL,
    [department] varchar(30)  NULL,
    [brand_names] varchar(255)  NULL,
    [contactNo] varchar(10)  NOT NULL,
    [imei] varchar(50)  NOT NULL,
    [isActive] bit  NOT NULL,
    [lastLogin] datetime  NOT NULL,
    [salesPersonNo] int  NULL,
    [visible_flag] tinyint  NULL,
    [notification_enabled] bit  NOT NULL,
    [notification_token] varchar(255)  NULL,
    [isAdmin] bit  NOT NULL,
    [isCalculationActive] bit  NOT NULL,
    [appVersion] varchar(5)  NULL
);
GO

-- Creating table 'tblDesignations'
CREATE TABLE [dbo].[tblDesignations] (
    [designationId] int IDENTITY(1,1) NOT NULL,
    [designationName] nvarchar(50)  NOT NULL,
    [designation_Id] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblSiteSettings'
CREATE TABLE [dbo].[tblSiteSettings] (
    [SettingId] int IDENTITY(1,1) NOT NULL,
    [AdminEmail] nvarchar(50)  NULL,
    [AdminMobile] nvarchar(50)  NOT NULL,
    [ApiVersion] nvarchar(20)  NOT NULL,
    [EnableSmsNotification] nvarchar(10)  NOT NULL,
    [SmsUrl] nvarchar(50)  NOT NULL,
    [SmsUserName] nvarchar(20)  NOT NULL,
    [SmsPassword] nvarchar(20)  NOT NULL,
    [SmsApiKey] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'tblMultiBrandIncentiveRules'
CREATE TABLE [dbo].[tblMultiBrandIncentiveRules] (
    [RuleId] int IDENTITY(1,1) NOT NULL,
    [RuleName] nvarchar(100)  NOT NULL,
    [SiteCuid] nvarchar(50)  NOT NULL,
    [Dpeartment] nvarchar(50)  NOT NULL,
    [Brand] nvarchar(50)  NOT NULL,
    [Section] nvarchar(50)  NOT NULL,
    [Product] nvarchar(50)  NOT NULL,
    [StyleCode] nvarchar(20)  NOT NULL,
    [IncentiveAmount] decimal(6,2)  NOT NULL,
    [ActiveFrom] datetime  NULL,
    [ActiveTo] datetime  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'tblAttendances'
CREATE TABLE [dbo].[tblAttendances] (
    [id] int IDENTITY(1,1) NOT NULL,
    [salePersonNo] int  NULL,
    [AttendanceDate] datetime  NULL,
    [status] nvarchar(10)  NULL,
    [firstintime] nvarchar(10)  NULL,
    [secondouttime] nvarchar(10)  NULL,
    [thirdintime] nvarchar(10)  NULL,
    [fourthouttime] nvarchar(10)  NULL,
    [totalHours] nvarchar(10)  NULL
);
GO

-- Creating table 'TblAdminLogs'
CREATE TABLE [dbo].[TblAdminLogs] (
    [LogId] int IDENTITY(1,1) NOT NULL,
    [SectionName] nvarchar(max)  NULL,
    [ModuleName] nvarchar(max)  NULL,
    [Action] nvarchar(max)  NULL,
    [FileName] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NOT NULL,
    [LogDate] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [Ip] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'tblVendorsalespersons'
CREATE TABLE [dbo].[tblVendorsalespersons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_vendor] int  NULL,
    [site_cuid] nvarchar(50)  NULL,
    [department] nvarchar(100)  NULL,
    [brand] nvarchar(50)  NULL,
    [sales_person_no] int  NULL,
    [insert_date] datetime  NULL,
    [last_update_date] datetime  NULL
);
GO

-- Creating table 'TblAdminLog1'
CREATE TABLE [dbo].[TblAdminLog1] (
    [LogId] int IDENTITY(1,1) NOT NULL,
    [SectionName] nvarchar(max)  NULL,
    [ModuleName] nvarchar(max)  NULL,
    [Action] nvarchar(max)  NULL,
    [FileName] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NOT NULL,
    [LogDate] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [Ip] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'tblAdminUser1'
CREATE TABLE [dbo].[tblAdminUser1] (
    [admin_id] int IDENTITY(1,1) NOT NULL,
    [loginId] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [fullName] nvarchar(100)  NOT NULL,
    [userType] nvarchar(50)  NOT NULL,
    [isActive] bit  NOT NULL
);
GO

-- Creating table 'TblApiLog1'
CREATE TABLE [dbo].[TblApiLog1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [queryString] nvarchar(max)  NULL,
    [inputData] nvarchar(max)  NOT NULL,
    [ip] nvarchar(50)  NOT NULL,
    [logDate] datetime  NOT NULL
);
GO

-- Creating table 'tblAttendance1'
CREATE TABLE [dbo].[tblAttendance1] (
    [id] int IDENTITY(1,1) NOT NULL,
    [salePersonNo] int  NULL,
    [AttendanceDate] datetime  NULL,
    [status] nvarchar(10)  NULL,
    [firstintime] nvarchar(10)  NULL,
    [secondouttime] nvarchar(10)  NULL,
    [thirdintime] nvarchar(10)  NULL,
    [fourthouttime] nvarchar(10)  NULL,
    [totalHours] nvarchar(10)  NULL
);
GO

-- Creating table 'tblbaseqty1'
CREATE TABLE [dbo].[tblbaseqty1] (
    [baseid] int IDENTITY(1,1) NOT NULL,
    [barcode] nvarchar(50)  NOT NULL,
    [siteCuid] nvarchar(50)  NOT NULL,
    [division] nvarchar(50)  NOT NULL,
    [section] nvarchar(50)  NOT NULL,
    [department] nvarchar(50)  NOT NULL,
    [brand] nvarchar(50)  NOT NULL,
    [styleCode] nvarchar(50)  NOT NULL,
    [color] nvarchar(50)  NOT NULL,
    [fit] nvarchar(50)  NOT NULL,
    [size] nvarchar(50)  NOT NULL,
    [baseQty] int  NOT NULL,
    [lastUpdateDate] datetime  NOT NULL,
    [initQty] int  NOT NULL,
    [rptcurrQty] int  NOT NULL,
    [calreqtoAddQty] int  NOT NULL,
    [calrecvdQty] int  NOT NULL,
    [combination] nvarchar(100)  NOT NULL,
    [whinitqty] int  NOT NULL
);
GO

-- Creating table 'tblBrand1'
CREATE TABLE [dbo].[tblBrand1] (
    [brand_id] int IDENTITY(1,1) NOT NULL,
    [brandName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblColor1'
CREATE TABLE [dbo].[tblColor1] (
    [color_id] int IDENTITY(1,1) NOT NULL,
    [colorName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblDepartment1'
CREATE TABLE [dbo].[tblDepartment1] (
    [dep_id] int IDENTITY(1,1) NOT NULL,
    [departmentName] varchar(20)  NOT NULL
);
GO

-- Creating table 'tblDesignation1'
CREATE TABLE [dbo].[tblDesignation1] (
    [designationId] int IDENTITY(1,1) NOT NULL,
    [designation_Id] nvarchar(50)  NOT NULL,
    [designationName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblDivision1'
CREATE TABLE [dbo].[tblDivision1] (
    [div_id] int IDENTITY(1,1) NOT NULL,
    [divisionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'TblLog1'
CREATE TABLE [dbo].[TblLog1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Thread] varchar(255)  NOT NULL,
    [Level] varchar(50)  NOT NULL,
    [Logger] varchar(255)  NOT NULL,
    [Message] varchar(4000)  NOT NULL,
    [Exception] varchar(2000)  NULL
);
GO

-- Creating table 'tblMultiBrandIncentiveRule1'
CREATE TABLE [dbo].[tblMultiBrandIncentiveRule1] (
    [RuleId] int IDENTITY(1,1) NOT NULL,
    [RuleName] nvarchar(100)  NOT NULL,
    [SiteCuid] nvarchar(50)  NOT NULL,
    [Dpeartment] nvarchar(50)  NOT NULL,
    [Brand] nvarchar(50)  NOT NULL,
    [Section] nvarchar(50)  NOT NULL,
    [Product] nvarchar(50)  NOT NULL,
    [StyleCode] nvarchar(20)  NOT NULL,
    [IncentiveAmount] decimal(6,2)  NOT NULL,
    [ActiveFrom] datetime  NULL,
    [ActiveTo] datetime  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'tblNotification1'
CREATE TABLE [dbo].[tblNotification1] (
    [notif_id] int IDENTITY(1,1) NOT NULL,
    [notificationType] nvarchar(20)  NULL,
    [fromUserId] int  NOT NULL,
    [fromSiteId] int  NOT NULL,
    [toSiteId] int  NOT NULL,
    [toEmail] nvarchar(50)  NOT NULL,
    [toMobile] nvarchar(10)  NOT NULL,
    [notificationText] nvarchar(max)  NOT NULL,
    [stockList] nvarchar(max)  NOT NULL,
    [statusFlag] nvarchar(20)  NOT NULL,
    [notificationDate] datetime  NOT NULL,
    [actionDate] datetime  NULL,
    [requestNote] nvarchar(20)  NULL,
    [clientReference] nvarchar(50)  NULL,
    [styleCode] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'tblPvtLabelName1'
CREATE TABLE [dbo].[tblPvtLabelName1] (
    [groupId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'tblSection1'
CREATE TABLE [dbo].[tblSection1] (
    [sec_id] int IDENTITY(1,1) NOT NULL,
    [sectionName] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblSiteSetting1'
CREATE TABLE [dbo].[tblSiteSetting1] (
    [SettingId] int IDENTITY(1,1) NOT NULL,
    [AdminEmail] nvarchar(50)  NULL,
    [AdminMobile] nvarchar(50)  NOT NULL,
    [ApiVersion] nvarchar(20)  NOT NULL,
    [EnableSmsNotification] nvarchar(10)  NOT NULL,
    [SmsUrl] nvarchar(50)  NOT NULL,
    [SmsUserName] nvarchar(20)  NOT NULL,
    [SmsPassword] nvarchar(20)  NOT NULL,
    [SmsApiKey] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'tblSize1'
CREATE TABLE [dbo].[tblSize1] (
    [size_id] int IDENTITY(1,1) NOT NULL,
    [sizeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'tblstock1'
CREATE TABLE [dbo].[tblstock1] (
    [stock_id] int IDENTITY(1,1) NOT NULL,
    [barcode] nvarchar(50)  NOT NULL,
    [site_id] int  NOT NULL,
    [div_id] int  NOT NULL,
    [sec_id] int  NOT NULL,
    [dep_id] int  NOT NULL,
    [brand_id] int  NOT NULL,
    [styleCode] nvarchar(50)  NOT NULL,
    [color_id] int  NOT NULL,
    [size_id] int  NOT NULL,
    [fit] nvarchar(50)  NOT NULL,
    [closingTotal] int  NOT NULL,
    [combination] nvarchar(150)  NOT NULL,
    [siteCuid] nvarchar(50)  NOT NULL,
    [category2] nvarchar(50)  NULL,
    [desc4] nvarchar(50)  NULL,
    [desc6] nvarchar(50)  NULL,
    [mrp] nvarchar(50)  NULL,
    [vendorName] nvarchar(100)  NULL,
    [lastStockUpdate] datetime  NULL
);
GO

-- Creating table 'tblVendor1'
CREATE TABLE [dbo].[tblVendor1] (
    [id] int IDENTITY(1,1) NOT NULL,
    [email] nvarchar(100)  NULL,
    [firstName] nvarchar(50)  NULL,
    [lastName] nvarchar(50)  NULL,
    [password] nvarchar(15)  NULL,
    [dateCreated] datetime  NULL,
    [lastLoginDate] datetime  NULL,
    [lastLoginIp] int  NULL,
    [isActive] bit  NOT NULL,
    [vendorName] nvarchar(100)  NULL
);
GO

-- Creating table 'tblVendorLoginHistory1'
CREATE TABLE [dbo].[tblVendorLoginHistory1] (
    [id] int IDENTITY(1,1) NOT NULL,
    [vendorId] int  NULL,
    [loginDate] datetime  NULL,
    [ip] nvarchar(100)  NULL
);
GO

-- Creating table 'tblVendorsalesperson1'
CREATE TABLE [dbo].[tblVendorsalesperson1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_vendor] int  NULL,
    [site_cuid] nvarchar(50)  NULL,
    [department] nvarchar(100)  NULL,
    [brand] nvarchar(50)  NULL,
    [sales_person_no] int  NULL,
    [insert_date] datetime  NULL,
    [last_update_date] datetime  NULL
);
GO

-- Creating table 'tblWeeklytargetincentive1'
CREATE TABLE [dbo].[tblWeeklytargetincentive1] (
    [id] int IDENTITY(1,1) NOT NULL,
    [salesPersonNo] int  NOT NULL,
    [finYear] nvarchar(10)  NOT NULL,
    [weekNo] int  NOT NULL,
    [weekTargetAmt] bigint  NOT NULL,
    [incentivePercent] decimal(4,2)  NOT NULL,
    [weekSalesAmt] decimal(9,2)  NOT NULL,
    [incentiveEarnedAmt] decimal(7,2)  NOT NULL,
    [dateEntered] datetime  NULL,
    [lastUpdatedDate] datetime  NULL,
    [lastCalcDate] datetime  NULL
);
GO

-- Creating table 'tblWeekMaster1'
CREATE TABLE [dbo].[tblWeekMaster1] (
    [weekid] int IDENTITY(1,1) NOT NULL,
    [finYear] varchar(10)  NULL,
    [weekNo] int  NULL,
    [startDate] datetime  NULL,
    [endDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [site_id] in table 'tblSites'
ALTER TABLE [dbo].[tblSites]
ADD CONSTRAINT [PK_tblSites]
    PRIMARY KEY CLUSTERED ([site_id] ASC);
GO

-- Creating primary key on [Id] in table 'TblLogs'
ALTER TABLE [dbo].[TblLogs]
ADD CONSTRAINT [PK_TblLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [weekid] in table 'tblWeekMasters'
ALTER TABLE [dbo].[tblWeekMasters]
ADD CONSTRAINT [PK_tblWeekMasters]
    PRIMARY KEY CLUSTERED ([weekid] ASC);
GO

-- Creating primary key on [brand_id] in table 'tblBrands'
ALTER TABLE [dbo].[tblBrands]
ADD CONSTRAINT [PK_tblBrands]
    PRIMARY KEY CLUSTERED ([brand_id] ASC);
GO

-- Creating primary key on [color_id] in table 'tblColors'
ALTER TABLE [dbo].[tblColors]
ADD CONSTRAINT [PK_tblColors]
    PRIMARY KEY CLUSTERED ([color_id] ASC);
GO

-- Creating primary key on [dep_id] in table 'tblDepartments'
ALTER TABLE [dbo].[tblDepartments]
ADD CONSTRAINT [PK_tblDepartments]
    PRIMARY KEY CLUSTERED ([dep_id] ASC);
GO

-- Creating primary key on [div_id] in table 'tblDivisions'
ALTER TABLE [dbo].[tblDivisions]
ADD CONSTRAINT [PK_tblDivisions]
    PRIMARY KEY CLUSTERED ([div_id] ASC);
GO

-- Creating primary key on [sale_id] in table 'tblsales'
ALTER TABLE [dbo].[tblsales]
ADD CONSTRAINT [PK_tblsales]
    PRIMARY KEY CLUSTERED ([sale_id] ASC);
GO

-- Creating primary key on [sec_id] in table 'tblSections'
ALTER TABLE [dbo].[tblSections]
ADD CONSTRAINT [PK_tblSections]
    PRIMARY KEY CLUSTERED ([sec_id] ASC);
GO

-- Creating primary key on [size_id] in table 'tblSizes'
ALTER TABLE [dbo].[tblSizes]
ADD CONSTRAINT [PK_tblSizes]
    PRIMARY KEY CLUSTERED ([size_id] ASC);
GO

-- Creating primary key on [qtrid] in table 'tblQuarters'
ALTER TABLE [dbo].[tblQuarters]
ADD CONSTRAINT [PK_tblQuarters]
    PRIMARY KEY CLUSTERED ([qtrid] ASC);
GO

-- Creating primary key on [baseid] in table 'tblbaseqties'
ALTER TABLE [dbo].[tblbaseqties]
ADD CONSTRAINT [PK_tblbaseqties]
    PRIMARY KEY CLUSTERED ([baseid] ASC);
GO

-- Creating primary key on [id] in table 'tblSlabs'
ALTER TABLE [dbo].[tblSlabs]
ADD CONSTRAINT [PK_tblSlabs]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tblWeeklytargetincentives'
ALTER TABLE [dbo].[tblWeeklytargetincentives]
ADD CONSTRAINT [PK_tblWeeklytargetincentives]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tblVendors'
ALTER TABLE [dbo].[tblVendors]
ADD CONSTRAINT [PK_tblVendors]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tblVendorBrands'
ALTER TABLE [dbo].[tblVendorBrands]
ADD CONSTRAINT [PK_tblVendorBrands]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tblVendorLoginHistories'
ALTER TABLE [dbo].[tblVendorLoginHistories]
ADD CONSTRAINT [PK_tblVendorLoginHistories]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [admin_id] in table 'tblAdminUsers'
ALTER TABLE [dbo].[tblAdminUsers]
ADD CONSTRAINT [PK_tblAdminUsers]
    PRIMARY KEY CLUSTERED ([admin_id] ASC);
GO

-- Creating primary key on [Id] in table 'TblApiLogs'
ALTER TABLE [dbo].[TblApiLogs]
ADD CONSTRAINT [PK_TblApiLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [stock_id] in table 'tblstocks'
ALTER TABLE [dbo].[tblstocks]
ADD CONSTRAINT [PK_tblstocks]
    PRIMARY KEY CLUSTERED ([stock_id] ASC);
GO

-- Creating primary key on [notif_id] in table 'tblNotifications'
ALTER TABLE [dbo].[tblNotifications]
ADD CONSTRAINT [PK_tblNotifications]
    PRIMARY KEY CLUSTERED ([notif_id] ASC);
GO

-- Creating primary key on [id] in table 'tblPvtLabelGroups'
ALTER TABLE [dbo].[tblPvtLabelGroups]
ADD CONSTRAINT [PK_tblPvtLabelGroups]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [groupId] in table 'tblPvtLabelNames'
ALTER TABLE [dbo].[tblPvtLabelNames]
ADD CONSTRAINT [PK_tblPvtLabelNames]
    PRIMARY KEY CLUSTERED ([groupId] ASC);
GO

-- Creating primary key on [id] in table 'tblPvtLabelSlabs'
ALTER TABLE [dbo].[tblPvtLabelSlabs]
ADD CONSTRAINT [PK_tblPvtLabelSlabs]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [user_id] in table 'tblUsers'
ALTER TABLE [dbo].[tblUsers]
ADD CONSTRAINT [PK_tblUsers]
    PRIMARY KEY CLUSTERED ([user_id] ASC);
GO

-- Creating primary key on [designationId] in table 'tblDesignations'
ALTER TABLE [dbo].[tblDesignations]
ADD CONSTRAINT [PK_tblDesignations]
    PRIMARY KEY CLUSTERED ([designationId] ASC);
GO

-- Creating primary key on [SettingId] in table 'tblSiteSettings'
ALTER TABLE [dbo].[tblSiteSettings]
ADD CONSTRAINT [PK_tblSiteSettings]
    PRIMARY KEY CLUSTERED ([SettingId] ASC);
GO

-- Creating primary key on [RuleId] in table 'tblMultiBrandIncentiveRules'
ALTER TABLE [dbo].[tblMultiBrandIncentiveRules]
ADD CONSTRAINT [PK_tblMultiBrandIncentiveRules]
    PRIMARY KEY CLUSTERED ([RuleId] ASC);
GO

-- Creating primary key on [id] in table 'tblAttendances'
ALTER TABLE [dbo].[tblAttendances]
ADD CONSTRAINT [PK_tblAttendances]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [LogId] in table 'TblAdminLogs'
ALTER TABLE [dbo].[TblAdminLogs]
ADD CONSTRAINT [PK_TblAdminLogs]
    PRIMARY KEY CLUSTERED ([LogId] ASC);
GO

-- Creating primary key on [Id] in table 'tblVendorsalespersons'
ALTER TABLE [dbo].[tblVendorsalespersons]
ADD CONSTRAINT [PK_tblVendorsalespersons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LogId] in table 'TblAdminLog1'
ALTER TABLE [dbo].[TblAdminLog1]
ADD CONSTRAINT [PK_TblAdminLog1]
    PRIMARY KEY CLUSTERED ([LogId] ASC);
GO

-- Creating primary key on [admin_id] in table 'tblAdminUser1'
ALTER TABLE [dbo].[tblAdminUser1]
ADD CONSTRAINT [PK_tblAdminUser1]
    PRIMARY KEY CLUSTERED ([admin_id] ASC);
GO

-- Creating primary key on [Id] in table 'TblApiLog1'
ALTER TABLE [dbo].[TblApiLog1]
ADD CONSTRAINT [PK_TblApiLog1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'tblAttendance1'
ALTER TABLE [dbo].[tblAttendance1]
ADD CONSTRAINT [PK_tblAttendance1]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [baseid] in table 'tblbaseqty1'
ALTER TABLE [dbo].[tblbaseqty1]
ADD CONSTRAINT [PK_tblbaseqty1]
    PRIMARY KEY CLUSTERED ([baseid] ASC);
GO

-- Creating primary key on [brand_id] in table 'tblBrand1'
ALTER TABLE [dbo].[tblBrand1]
ADD CONSTRAINT [PK_tblBrand1]
    PRIMARY KEY CLUSTERED ([brand_id] ASC);
GO

-- Creating primary key on [color_id] in table 'tblColor1'
ALTER TABLE [dbo].[tblColor1]
ADD CONSTRAINT [PK_tblColor1]
    PRIMARY KEY CLUSTERED ([color_id] ASC);
GO

-- Creating primary key on [dep_id] in table 'tblDepartment1'
ALTER TABLE [dbo].[tblDepartment1]
ADD CONSTRAINT [PK_tblDepartment1]
    PRIMARY KEY CLUSTERED ([dep_id] ASC);
GO

-- Creating primary key on [designationId] in table 'tblDesignation1'
ALTER TABLE [dbo].[tblDesignation1]
ADD CONSTRAINT [PK_tblDesignation1]
    PRIMARY KEY CLUSTERED ([designationId] ASC);
GO

-- Creating primary key on [div_id] in table 'tblDivision1'
ALTER TABLE [dbo].[tblDivision1]
ADD CONSTRAINT [PK_tblDivision1]
    PRIMARY KEY CLUSTERED ([div_id] ASC);
GO

-- Creating primary key on [Id] in table 'TblLog1'
ALTER TABLE [dbo].[TblLog1]
ADD CONSTRAINT [PK_TblLog1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [RuleId] in table 'tblMultiBrandIncentiveRule1'
ALTER TABLE [dbo].[tblMultiBrandIncentiveRule1]
ADD CONSTRAINT [PK_tblMultiBrandIncentiveRule1]
    PRIMARY KEY CLUSTERED ([RuleId] ASC);
GO

-- Creating primary key on [notif_id] in table 'tblNotification1'
ALTER TABLE [dbo].[tblNotification1]
ADD CONSTRAINT [PK_tblNotification1]
    PRIMARY KEY CLUSTERED ([notif_id] ASC);
GO

-- Creating primary key on [groupId] in table 'tblPvtLabelName1'
ALTER TABLE [dbo].[tblPvtLabelName1]
ADD CONSTRAINT [PK_tblPvtLabelName1]
    PRIMARY KEY CLUSTERED ([groupId] ASC);
GO

-- Creating primary key on [sec_id] in table 'tblSection1'
ALTER TABLE [dbo].[tblSection1]
ADD CONSTRAINT [PK_tblSection1]
    PRIMARY KEY CLUSTERED ([sec_id] ASC);
GO

-- Creating primary key on [SettingId] in table 'tblSiteSetting1'
ALTER TABLE [dbo].[tblSiteSetting1]
ADD CONSTRAINT [PK_tblSiteSetting1]
    PRIMARY KEY CLUSTERED ([SettingId] ASC);
GO

-- Creating primary key on [size_id] in table 'tblSize1'
ALTER TABLE [dbo].[tblSize1]
ADD CONSTRAINT [PK_tblSize1]
    PRIMARY KEY CLUSTERED ([size_id] ASC);
GO

-- Creating primary key on [stock_id] in table 'tblstock1'
ALTER TABLE [dbo].[tblstock1]
ADD CONSTRAINT [PK_tblstock1]
    PRIMARY KEY CLUSTERED ([stock_id] ASC);
GO

-- Creating primary key on [id] in table 'tblVendor1'
ALTER TABLE [dbo].[tblVendor1]
ADD CONSTRAINT [PK_tblVendor1]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tblVendorLoginHistory1'
ALTER TABLE [dbo].[tblVendorLoginHistory1]
ADD CONSTRAINT [PK_tblVendorLoginHistory1]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'tblVendorsalesperson1'
ALTER TABLE [dbo].[tblVendorsalesperson1]
ADD CONSTRAINT [PK_tblVendorsalesperson1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'tblWeeklytargetincentive1'
ALTER TABLE [dbo].[tblWeeklytargetincentive1]
ADD CONSTRAINT [PK_tblWeeklytargetincentive1]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [weekid] in table 'tblWeekMaster1'
ALTER TABLE [dbo].[tblWeekMaster1]
ADD CONSTRAINT [PK_tblWeekMaster1]
    PRIMARY KEY CLUSTERED ([weekid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------