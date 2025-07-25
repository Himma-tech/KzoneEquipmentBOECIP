USE [KZONE_B20_01_HIS]
GO
/****** Object:  Table [dbo].[SBCS_UNITHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_UNITHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_TERMINALMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_TERMINALMESSAGEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_TANKHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_TANKHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_TACTDATAHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_TACTDATAHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_SVData]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_SVData]
GO
/****** Object:  Table [dbo].[SBCS_RECIPEVALIDATIONRESULTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_RECIPEVALIDATIONRESULTHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_RECIPEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_RECIPEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_PROCESSDATAHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_PROCESSDATAHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_PORTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_PORTHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_OPIHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_OPIHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_NODEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_NODEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_MATERIALHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_MATERIALHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_JOBHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_JOBHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_JobCountHistory]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_JobCountHistory]
GO
/****** Object:  Table [dbo].[SBCS_GLASSQTIMEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_GLASSQTIMEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_EQStatusHistory]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_EQStatusHistory]
GO
/****** Object:  Table [dbo].[SBCS_DVData]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_DVData]
GO
/****** Object:  Table [dbo].[SBCS_DISPATCHHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_DISPATCHHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_DCRRESULTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_DCRRESULTHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_COMMANDHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_COMMANDHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_CIMMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_CIMMESSAGEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_CASSETTEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_CASSETTEHISTORY]
GO
/****** Object:  Table [dbo].[SBCS_ALARMHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP TABLE [dbo].[SBCS_ALARMHISTORY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Unit_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Unit_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_TERMINALMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_TERMINALMESSAGEHISTORY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Tank_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Tank_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Tact_Data_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Tact_Data_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Recipe_Validation_Result_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Recipe_Validation_Result_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Recipe_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Recipe_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Process_Data_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Process_Data_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Port_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Port_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_OPI_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_OPI_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_NODE_HISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_NODE_HISTORY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_MATERIAL_HISTORY]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_MATERIAL_HISTORY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Job_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Job_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_INCOMPLETECST_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_INCOMPLETECST_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_CIMMessage_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_CIMMessage_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Alarm_History]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Delete_Alarm_History]
GO
/****** Object:  StoredProcedure [dbo].[sp_Backup_Database]    Script Date: 2024/11/14 15:47:51 ******/
DROP PROCEDURE [dbo].[sp_Backup_Database]
GO
USE [master]
GO
/****** Object:  Database [KZONE_B20_01_HIS]    Script Date: 2024/11/14 15:47:51 ******/
DROP DATABASE [KZONE_B20_01_HIS]
GO
/****** Object:  Database [KZONE_B20_01_HIS]    Script Date: 2024/11/14 15:47:51 ******/
CREATE DATABASE [KZONE_B20_01_HIS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KZONE_B20_01_HIS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\KZONE_B20_01_HIS.mdf' , SIZE = 167936KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'KZONE_B20_01_HIS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\KZONE_B20_01_HIS_log.ldf' , SIZE = 757248KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KZONE_B20_01_HIS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ARITHABORT OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET  MULTI_USER 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [KZONE_B20_01_HIS]
GO
/****** Object:  StoredProcedure [dbo].[sp_Backup_Database]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create proc [dbo].[sp_Backup_Database] 
@backupPath varchar(250) ,/*路径  D:\Database\backup\ */
@databaseName varchar(250) --Database Name  UniBCS1 & UniBCS2
as 
declare @name varchar(250)
set @name = @backupPath+@databaseName+'_'+convert(varchar(50),getdate(),112)+'.bak'
BACKUP DATABASE @databaseName to 
disk=@name 
with noformat,noinit,
name=@databaseName,
skip,norewind,nounload











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Alarm_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Alarm_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_ALARMHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_CIMMessage_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_CIMMessage_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_CIMMESSAGEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END












GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_INCOMPLETECST_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_INCOMPLETECST_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_INCOMPLETECST where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END










GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Job_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Job_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_JOBHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
	delete from SBCS_DISPATCHHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
	delete from SBCS_DCRRESULTHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END










GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_MATERIAL_HISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_MATERIAL_HISTORY]
	@overTime int
AS
BEGIN
	delete from SBCS_MATERIALHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_NODE_HISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_NODE_HISTORY] 
	@overTime int
AS
BEGIN
	delete from SBCS_NODEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END












GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_OPI_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_OPI_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_OPIHISTORY where DATEDIFF(HH,OPDATETIME,GETDATE())>@overTime*24
	delete from SBCS_GLASSQTIMEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Port_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Port_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_PORTHISTORY
	 where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Process_Data_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Process_Data_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_PROCESSDATAHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Recipe_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Recipe_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_RECIPEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END







GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Recipe_Validation_Result_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Recipe_Validation_Result_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_RECIPEVALIDATIONRESULTHISTORY where DATEDIFF(HH,ReceiveTime,GETDATE())>@overTime*24
   
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Tact_Data_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Tact_Data_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_TACTDATAHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END









GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Tank_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Tank_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_TANKHISTORY where DATEDIFF(HH,STARTTIME,GETDATE())>@overTime*24
   
END







GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_TERMINALMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_TERMINALMESSAGEHISTORY] 
	@overTime int
AS
BEGIN
	delete from SBCS_TERMINALMESSAGEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24;
	delete from SBCS_CIMMESSAGEHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_Unit_History]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Delete_Unit_History] 
	@overTime int
AS
BEGIN
	delete from SBCS_UNITHISTORY where DATEDIFF(HH,UPDATETIME,GETDATE())>@overTime*24
   
END










GO
/****** Object:  Table [dbo].[SBCS_ALARMHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_ALARMHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[EVENTNAME] [varchar](50) NULL,
	[UPDATETIME] [datetime] NULL,
	[ALARMID] [varchar](50) NULL,
	[ALARMADDRESS] [varchar](50) NULL,
	[ALARMCODE] [varchar](50) NULL,
	[ALARMLEVEL] [varchar](50) NULL,
	[ALARMTEXT] [varchar](255) NULL,
	[ALARMSTATUS] [varchar](50) NULL,
	[NODEID] [varchar](50) NULL,
	[ALARMUNIT] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_ALARMHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_ALARMHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_CASSETTEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_CASSETTEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[CASSETTEID] [varchar](50) NULL,
	[CASSETTESEQNO] [int] NULL,
	[CASSETTESTATUS] [varchar](50) NULL,
	[NODEID] [varchar](50) NULL,
	[JOBCOUNT] [int] NULL,
	[PORTID] [varchar](50) NULL,
	[JOBEXISTENCE] [varchar](500) NULL,
	[CASSETTECONTROLCOMMAND] [varchar](50) NULL,
	[COMMANDRETURNCODE] [varchar](50) NULL,
	[OPERATORID] [varchar](50) NULL,
	[COMMPLETEDCASSETTEDATA] [varchar](50) NULL,
	[LOADINGCASSETTETYPE] [varchar](50) NULL,
	[QTIMEFLAG] [int] NULL,
	[PARTIALFULLFLAG] [int] NULL,
	[LOADTIME] [datetime] NULL,
	[PROCESSSTATTIME] [datetime] NULL,
	[PROCESSENDTIME] [datetime] NULL,
 CONSTRAINT [PK_SBCS_CASSETTEHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_CASSETTEHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_CIMMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_CIMMESSAGEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[NODEID] [varchar](50) NOT NULL,
	[NODENO] [varchar](50) NOT NULL,
	[MESSAGEID] [varchar](10) NULL,
	[MESSAGETEXT] [varchar](80) NOT NULL,
	[MESSAGESTATUS] [varchar](15) NULL,
	[OPERATORID] [varchar](50) NULL,
	[REMARK] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_CIMMESSAGEHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_COMMANDHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_COMMANDHISTORY](
	[CommandTime] [datetime] NULL,
	[GlassID] [varchar](max) NULL,
	[Command] [varchar](max) NULL,
	[CommandResult] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_DCRRESULTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_DCRRESULTHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[CASSETTESEQNO] [int] NOT NULL,
	[CASSETTESLOTNO] [int] NOT NULL,
	[GlASSID] [varchar](50) NOT NULL,
	[READGLASSID] [varchar](50) NULL,
	[RESULT] [varchar](50) NULL,
	[DESCRIPTION] [varchar](50) NULL,
	[NODENO] [varchar](50) NULL,
	[DCRNO] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_DCRRESULTHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_DCRRESULTHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_DISPATCHHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_DISPATCHHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NULL,
	[EVENTNAME] [varchar](50) NULL,
	[CASSETTESEQNO] [int] NULL,
	[CASSETTESLOTNO] [int] NULL,
	[GlASSID] [varchar](50) NULL,
	[GROUPNO] [int] NULL,
	[CIMMODE] [varchar](50) NULL,
	[GLASSTYPE] [varchar](50) NULL,
	[GLASSJUDGE] [varchar](50) NULL,
	[GLASSGRADE] [varchar](50) NULL,
	[PPID] [varchar](255) NULL,
	[INSPECTIONRESERVATIONSIGNAL] [varchar](50) NULL,
	[PROCESSRESERVATIONSIGNAL] [varchar](50) NULL,
	[INSPJUDGEDRESULT] [varchar](50) NULL,
	[TRACKINGDATAHISTORY] [varchar](50) NULL,
	[EQUIPMENTSPECIALFLAG] [varchar](50) NULL,
	[DISPATCHNODENO] [varchar](50) NULL,
	[DISPATCHPOINT] [varchar](50) NULL,
	[DISPATCHTARGET] [varchar](250) NULL,
	[SAMPLINGRATE] [varchar](50) NULL,
	[DISPATCHRESULT] [varchar](50) NULL,
	[BATCHID] [varchar](200) NULL,
	[NGMARK] [varchar](50) NULL,
	[REASON] [varchar](500) NULL,
 CONSTRAINT [PK_SBCS_DISPATCHHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_DISPATCHHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_DVData]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_DVData](
	[UpdateTime] [datetime] NOT NULL,
	[DataValues] [varchar](max) NULL,
 CONSTRAINT [PK_SBCS_DVData] PRIMARY KEY CLUSTERED 
(
	[UpdateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_EQStatusHistory]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SBCS_EQStatusHistory](
	[CreateTime] [datetime] NULL,
	[LineID] [nvarchar](10) NULL,
	[Status] [nvarchar](10) NULL,
	[Time] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SBCS_GLASSQTIMEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_GLASSQTIMEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[CASSETTESEQNO] [int] NOT NULL,
	[CASSETTESLOTNO] [int] NOT NULL,
	[GlASSID] [varchar](50) NOT NULL,
	[EVENTNAME] [varchar](50) NULL,
	[QTIMEID] [varchar](50) NOT NULL,
	[SETTIMEVALUE] [int] NOT NULL,
	[STARTDATETIME] [varchar](50) NULL,
	[ENDDATETIME] [varchar](50) NULL,
	[SPENDQTIMEVALUE] [int] NULL,
	[ISOVERQTIME] [varchar](1) NULL,
	[STARTNODEID] [varchar](50) NOT NULL,
	[STARTNODENO] [varchar](50) NOT NULL,
	[STARTNUNITID] [varchar](50) NOT NULL,
	[STARTNUNITNO] [varchar](50) NOT NULL,
	[STARTEVENTMSG] [varchar](50) NOT NULL,
	[ENDNODEID] [varchar](50) NOT NULL,
	[ENDNODENO] [varchar](50) NOT NULL,
	[ENDNUNITID] [varchar](50) NOT NULL,
	[ENDNUNITNO] [varchar](50) NOT NULL,
	[ENDEVENTMSG] [varchar](50) NOT NULL,
	[STARTNODERECIPEID] [varchar](50) NULL,
	[ENABLED] [varchar](1) NOT NULL,
 CONSTRAINT [PK_SBCS_QTIMEHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_QTIMEHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_JobCountHistory]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SBCS_JobCountHistory](
	[CreateTime] [datetime] NULL,
	[LineID] [nvarchar](10) NULL,
	[Count] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SBCS_JOBHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_JOBHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NULL,
	[EVENTNAME] [varchar](50) NULL,
	[NODENO] [varchar](50) NULL,
	[UNITNO] [varchar](50) NULL,
	[Cassette_Sequence_No] [nvarchar](50) NULL,
	[Job_Sequence_No] [varchar](50) NULL,
	[GlassID] [varchar](50) NULL,
	[Lot_ID] [varchar](50) NULL,
	[Product_ID] [varchar](50) NULL,
	[Operation_ID] [varchar](50) NULL,
	[CST_Operation_Mode] [varchar](50) NULL,
	[Substrate_Type] [varchar](50) NULL,
	[Product_Type] [varchar](50) NULL,
	[Job_Type] [varchar](50) NULL,
	[Dummy_Type] [varchar](50) NULL,
	[Skip_Flag] [varchar](50) NULL,
	[Process_Flag] [varchar](50) NULL,
	[Process_Reason_Code] [varchar](50) NULL,
	[LOT_Code] [varchar](50) NULL,
	[Glass_Thickness] [varchar](50) NULL,
	[Glass_Degree] [varchar](50) NULL,
	[Inspection_Flag] [varchar](50) NULL,
	[Job_Judge] [varchar](50) NULL,
	[Job_Grade] [varchar](50) NULL,
	[Job_Recovery_Flag] [varchar](50) NULL,
	[Mode] [varchar](50) NULL,
	[Step_ID] [varchar](50) NULL,
	[VCR_Read_ID] [varchar](50) NULL,
	[Master_Recipe_ID] [varchar](50) NULL,
	[PPID] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_JOBHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_JOBHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_MATERIALHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_MATERIALHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[NODEID] [varchar](50) NOT NULL,
	[UNITNO] [varchar](10) NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[MATERIALID] [varchar](50) NULL,
	[MATERIALCOUNT] [varchar](50) NULL,
	[MATERIALSTATUS] [varchar](50) NULL,
	[MATERIALTYPE] [varchar](50) NULL,
	[OPERATORID] [varchar](50) NULL,
	[EVENT] [varchar](50) NULL,
	[OLDMATERIALID] [varchar](50) NULL,
	[FILENAME] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_MATERIALHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_NODEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_NODEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[LINEID] [varchar](50) NOT NULL,
	[NODEID] [varchar](50) NOT NULL,
	[NODENO] [varchar](50) NOT NULL,
	[CIMMODE] [varchar](50) NULL,
	[CURRENTRECIPEID] [varchar](50) NULL,
	[CURRENTSTATUS] [varchar](50) NULL,
	[AUTOMANUEL] [varchar](50) NULL,
	[UPINLINEMODE] [varchar](50) NULL,
	[DOWNINLINEMODE] [varchar](50) NULL,
	[TFTJOBCOUNT] [int] NULL,
	[HFJOBCOUNT] [int] NULL,
	[DUMMYJOBCOUNT] [int] NULL,
	[UVMASKCOUNT] [int] NULL,
	[MQCJOBCOUNT] [int] NULL,
	[PRODUCTTYPE] [int] NULL,
	[RECIPECHECK] [varchar](50) NULL,
	[GLASSCHECKMODE] [varchar](50) NULL,
	[RECIPEAUTOCHANGE] [varchar](50) NULL,
	[PRODUCTTYPECHECKMODE] [varchar](50) NULL,
	[GROUPINDEXCHECKMODE] [varchar](50) NULL,
	[PRODUCTIDCHECKMODE] [varchar](50) NULL,
	[DUPLICATECHECKMODE] [varchar](50) NULL,
	[ADDLIQUID] [varchar](50) NULL,
	[MINLIQUID] [varchar](50) NULL,
	[VCRENBLE] [varchar](50) NULL,
	[VCRID] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_NODEHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_NODEHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_OPIHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_OPIHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[SESSECTIONID] [varchar](50) NULL,
	[OPDATETIME] [datetime] NOT NULL,
	[OPERATORID] [varchar](40) NOT NULL,
	[DIRECTION] [varchar](10) NOT NULL,
	[TRANSACTIONID] [varchar](50) NULL,
	[MESSAGENAME] [varchar](50) NOT NULL,
	[MESSAGETRX] [varchar](max) NULL,
	[RETURNCODE] [varchar](50) NULL,
	[RETURNMESSAGE] [varchar](max) NULL,
	[COMMANDKEY] [varchar](20) NULL,
	[COMMANDDATA] [varchar](max) NULL,
	[COMMANDTYPE] [varchar](20) NULL,
	[PROCESSRESULT] [varchar](50) NULL,
	[PROCESSNGMESSAGE] [varchar](max) NULL,
 CONSTRAINT [PK_SBCS_OPIHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_OPIHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_PORTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_PORTHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NULL,
	[LINEID] [varchar](50) NULL,
	[NODEID] [varchar](50) NULL,
	[PORTID] [varchar](50) NULL,
	[PORTNO] [int] NULL,
	[PORTTYPE] [varchar](50) NULL,
	[PORTENABLEMODE] [varchar](50) NULL,
	[PORTTRANSFERMODE] [varchar](50) NULL,
	[CASSETTESEQNO] [int] NULL,
	[PORTCASSETTESTATUS] [varchar](50) NULL,
	[CASSETTEID] [varchar](50) NULL,
	[CRITERIALNUMBER] [int] NULL,
	[PORTGRADE] [varchar](50) NULL,
	[PORTGROUPNO] [varchar](50) NULL,
	[SORTGRADE] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_PORTHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_PORTHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_PROCESSDATAHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_PROCESSDATAHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[CASSETTESEQNO] [int] NULL,
	[CASSETTESLOTNO] [int] NULL,
	[JOBID] [varchar](50) NULL,
	[TRXID] [varchar](50) NULL,
	[MESCONTROLSTATE] [varchar](50) NULL,
	[NODEID] [varchar](50) NULL,
	[UPDATETIME] [datetime] NULL,
	[FILENAMA] [varchar](100) NULL,
	[LOCALPROCESSSTARTTIME] [varchar](50) NULL,
	[LOCALPROCSSSENDTIME] [varchar](50) NULL,
	[PROCESSTIME] [int] NULL,
 CONSTRAINT [PK_SBCS_PROCESSDATAHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_PROCESSDATAHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_RECIPEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_RECIPEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[NODENAME] [varchar](50) NOT NULL,
	[RECIPENO] [varchar](50) NULL,
	[RECIPEID] [varchar](50) NULL,
	[CREATETIME] [datetime] NULL,
	[VERSIONNO] [varchar](50) NULL,
	[EVENT] [varchar](50) NULL,
	[FILENAME] [varchar](500) NULL,
	[OPERATORID] [varchar](50) NULL,
	[RECIPESTATUS] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_INCOMPLETECST] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_INCOMPLETECST] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_RECIPEVALIDATIONRESULTHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_RECIPEVALIDATIONRESULTHISTORY](
	[NO] [bigint] IDENTITY(1,1) NOT NULL,
	[RECEIVETIME] [datetime] NULL,
	[MASTERRECIPEID] [varchar](50) NULL,
	[LOCALRECIPEID] [varchar](50) NULL,
	[RMS_RESULT] [varchar](50) NULL,
	[RMS_RESULTTEXT] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_RECIPEVALIDATIONRESULTHISTORY] PRIMARY KEY CLUSTERED 
(
	[NO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_SVData]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_SVData](
	[UpdateTime] [datetime] NOT NULL,
	[DataValues] [varchar](max) NULL,
 CONSTRAINT [PK_SBCS_SVData] PRIMARY KEY CLUSTERED 
(
	[UpdateTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_TACTDATAHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_TACTDATAHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NULL,
	[CASSETTESEQNO] [int] NULL,
	[CASSETTESLOTNO] [int] NULL,
	[JOBID] [varchar](50) NULL,
	[FILENAMA] [varchar](100) NULL,
	[LOCALPROCESSSTARTTIME] [varchar](50) NULL,
	[LOCALPROCSSSENDTIME] [varchar](50) NULL,
	[PROCESSTIME] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_TANKHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_TANKHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[NODEID] [varchar](50) NOT NULL,
	[TANKID] [varchar](50) NOT NULL,
	[TANKEVENT] [varchar](50) NOT NULL,
	[STARTTIME] [datetime] NULL,
	[ENDTIME] [datetime] NULL,
	[TOTALTIME] [varchar](50) NULL,
	[QUANTITY] [varchar](50) NULL,
	[SPEED] [varchar](50) NULL,
	[OPERATORID] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_ASSEMBLYHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_ASSEMBLYHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_TERMINALMESSAGEHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_TERMINALMESSAGEHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[TRANSACTIONID] [varchar](50) NOT NULL,
	[LINEID] [varchar](50) NOT NULL,
	[CAPTION] [varchar](50) NOT NULL,
	[TERMINALTEXT] [varchar](5000) NOT NULL,
 CONSTRAINT [PK_SBCS_TERMINALMESSAGE_TRX] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_TERMINALMESSAGE_TRX] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SBCS_UNITHISTORY]    Script Date: 2024/11/14 15:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SBCS_UNITHISTORY](
	[OBJECTKEY] [bigint] IDENTITY(1,1) NOT NULL,
	[UPDATETIME] [datetime] NOT NULL,
	[NODEID] [varchar](50) NULL,
	[NODENO] [varchar](50) NULL,
	[UNITNO] [int] NULL,
	[UNITID] [varchar](50) NULL,
	[UNITSTATUS] [varchar](50) NULL,
	[UNITTYPE] [varchar](50) NULL,
	[UNITRECIPEID] [varchar](50) NULL,
 CONSTRAINT [PK_SBCS_UNITHISTORY] PRIMARY KEY CLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_SBCS_UNITHISTORY] UNIQUE NONCLUSTERED 
(
	[OBJECTKEY] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [KZONE_B20_01_HIS] SET  READ_WRITE 
GO
