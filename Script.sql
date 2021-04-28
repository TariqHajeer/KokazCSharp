USE [master]
GO
/****** Object:  Database [Kokaz]    Script Date: 4/29/2021 12:55:35 AM ******/
CREATE DATABASE [Kokaz]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Kokaz', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kokaz.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Kokaz_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kokaz_log.ldf' , SIZE = 102144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Kokaz] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Kokaz].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Kokaz] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Kokaz] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Kokaz] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Kokaz] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Kokaz] SET ARITHABORT OFF 
GO
ALTER DATABASE [Kokaz] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Kokaz] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Kokaz] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Kokaz] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Kokaz] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Kokaz] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Kokaz] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Kokaz] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Kokaz] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Kokaz] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Kokaz] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Kokaz] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Kokaz] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Kokaz] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Kokaz] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Kokaz] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Kokaz] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Kokaz] SET RECOVERY FULL 
GO
ALTER DATABASE [Kokaz] SET  MULTI_USER 
GO
ALTER DATABASE [Kokaz] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Kokaz] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Kokaz] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Kokaz] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Kokaz] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Kokaz', N'ON'
GO
USE [Kokaz]
GO
/****** Object:  Table [dbo].[AgnetPrint]    Script Date: 4/29/2021 12:55:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgnetPrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrintId] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[ClientName] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_AgnetPrint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clientPhones]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[clientPhones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Phone] [varchar](50) NOT NULL,
 CONSTRAINT [PK_clientPhones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientPrint]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrintId] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[LastTotal] [nvarchar](50) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[DeliveCost] [decimal](18, 2) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ClientPrint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CountryId] [int] NULL,
	[Address] [nvarchar](50) NULL,
	[FirstDate] [date] NOT NULL,
	[Note] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupPrivilege]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupPrivilege](
	[GroupId] [int] NOT NULL,
	[PrivilegId] [int] NOT NULL,
 CONSTRAINT [PK_GroupPrivilege] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[PrivilegId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Income]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Income](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[Earining] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[userId] [int] NOT NULL,
	[IncomeTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Income] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomeType]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomeType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_IncomeType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoenyPlaced]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoenyPlaced](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MoenyPlaced] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[ClientId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[OldCost] [decimal](18, 2) NULL,
	[AgentCost] [decimal](18, 2) NOT NULL,
	[RecipientName] [nvarchar](50) NULL,
	[RecipientPhones] [nvarchar](max) NOT NULL,
	[RegionId] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[ClientNote] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[MoenyPlacedId] [int] NOT NULL,
	[OrderplacedId] [int] NOT NULL,
	[Date] [date] NULL,
	[DiliveryDate] [date] NULL,
	[Note] [nvarchar](max) NULL,
	[AgentId] [int] NULL,
	[seen] [bit] NULL,
	[IsClientDiliverdMoney] [bit] NOT NULL,
	[IsSync] [bit] NOT NULL,
	[OrderStateId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[OrderId] [int] NOT NULL,
	[OrderTpyeId] [int] NOT NULL,
	[Count] [real] NULL,
 CONSTRAINT [PK_OrderOrderType] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[OrderTpyeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderPlaced]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPlaced](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderPlaced] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderPrint]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPrint](
	[OrderId] [int] NOT NULL,
	[PrintId] [int] NOT NULL,
 CONSTRAINT [PK_OrderPrint] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[PrintId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderState]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderState](
	[Id] [int] NOT NULL,
	[State] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OrderState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderType]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutCome]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutCome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[userId] [int] NOT NULL,
	[OutComeTypeId] [int] NOT NULL,
 CONSTRAINT [PK_OutCome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutComeType]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutComeType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OutComeType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Printed]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Printed](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrintNmber] [int] NOT NULL,
	[PrinterName] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[DestinationName] [nvarchar](50) NOT NULL,
	[DestinationPhone] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Printed] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Privilege]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Privilege](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SysName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Privilege] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receipt]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[About] [nvarchar](max) NULL,
	[Manager] [nvarchar](max) NOT NULL,
	[IsPay] [bit] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Reginos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroup](
	[UserId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPhone]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPhone](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserPhone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/29/2021 12:55:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Experince] [nvarchar](max) NULL,
	[Adress] [nvarchar](max) NULL,
	[HireDate] [date] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CanWorkAsAgent] [bit] NOT NULL,
	[CountryId] [int] NULL,
	[Salary] [decimal](18, 2) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AgnetPrint] ON 

INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (17, 1021, N'1', CAST(10000.00 AS Decimal(18, 2)), N'بغداد', N'13223136779', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (18, 1021, N'2', CAST(20000.00 AS Decimal(18, 2)), N'بغداد', N'23132132131', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (19, 1021, N'3', CAST(300000.00 AS Decimal(18, 2)), N'بغداد', N'12324567891', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (20, 1021, N'4', CAST(40000.00 AS Decimal(18, 2)), N'بغداد', N'12345678912', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (21, 1021, N'5', CAST(50000.00 AS Decimal(18, 2)), N'بغداد', N'32132132159', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (22, 1021, N'6', CAST(60000.00 AS Decimal(18, 2)), N'بغداد', N'12315497489', N'عميل 1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (23, 1024, N'1', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'22311321564', N'عميل1', N'ملاحظات')
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (24, 1024, N'1', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'13213213216', N'عميل 2', N'ملاحظات')
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (25, 1025, N'1', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'12345648979', N'عميل3', N'ءؤم')
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (26, 1026, N'1', CAST(100000.00 AS Decimal(18, 2)), N'مدينة  1', N'12345678910', N'عميل1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (27, 1028, N'3', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'15000000000', N'عميل1', N'ملاحظات')
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (28, 1030, N'23', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'21321321321', N'عميل1', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note]) VALUES (29, 1032, N'50', CAST(150000.00 AS Decimal(18, 2)), N'مدينة  1', N'12345648979', N'عميل1', NULL)
SET IDENTITY_INSERT [dbo].[AgnetPrint] OFF
SET IDENTITY_INSERT [dbo].[clientPhones] ON 

INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (5, 7, N'12345687901')
SET IDENTITY_INSERT [dbo].[clientPhones] OFF
SET IDENTITY_INSERT [dbo].[ClientPrint] ON 

INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (10, 1022, N'1', NULL, CAST(10000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'13223136779')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (11, 1022, N'2', NULL, CAST(20000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'23132132131')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (12, 1022, N'3', NULL, CAST(300000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'12324567891')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (13, 1023, N'1', NULL, CAST(10000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'13223136779')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (14, 1023, N'2', NULL, CAST(20000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'23132132131')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (15, 1023, N'3', NULL, CAST(150000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'12324567891')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (16, 1023, N'4', NULL, CAST(40000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'12345678912')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (17, 1023, N'5', NULL, CAST(50000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'32132132159')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (18, 1023, N'6', NULL, CAST(6000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), N'بغداد', N'12315497489')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (19, 1027, N'1', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'مدينة  1', N'12345678910')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (20, 1029, N'3', NULL, CAST(150000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'مدينة  1', N'15000000000')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (21, 1031, N'23', NULL, CAST(150000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'مدينة  1', N'21321321321')
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone]) VALUES (22, 1033, N'50', NULL, CAST(150000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'مدينة  1', N'12345648979')
SET IDENTITY_INSERT [dbo].[ClientPrint] OFF
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Total]) VALUES (7, N'عميل1', NULL, NULL, CAST(N'2021-04-25' AS Date), NULL, N'amile1', N'25d55ad283aa400af464c76d713c07ad', 1, CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Clients] OFF
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost]) VALUES (1017, N'مدينة  1', CAST(5000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[Group] ON 

INSERT [dbo].[Group] ([Id], [Name]) VALUES (1, N'مجموعة المدراء')
INSERT [dbo].[Group] ([Id], [Name]) VALUES (2, N'الموظفين')
SET IDENTITY_INSERT [dbo].[Group] OFF
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 1)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 2)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 3)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 4)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 5)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 6)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 7)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 8)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 9)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 10)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 11)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 12)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 13)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 14)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 15)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 16)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 17)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 18)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 19)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 20)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 21)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 22)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 23)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 24)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 25)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 26)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 27)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 28)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 29)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 30)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 31)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 33)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 34)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 35)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 36)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 37)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 38)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 39)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 40)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 41)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 42)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 43)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 44)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 45)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 46)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 47)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 48)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 49)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 50)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 51)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 52)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 53)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 54)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 55)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 9)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 10)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 11)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 12)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 13)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 14)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 15)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 16)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 17)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 18)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 19)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 20)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 21)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 22)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 23)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (2, 24)
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (1, N'خارج الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (2, N'مندوب')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (3, N'داخل الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (4, N'تم تسليمها')
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId]) VALUES (4068, N'1', 7, 1017, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(5000.00 AS Decimal(18, 2)), NULL, N'12345678910', NULL, NULL, NULL, NULL, 1, 3, CAST(N'2021-04-26' AS Date), NULL, NULL, 3, 1, 1, 0, 1)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId]) VALUES (4069, N'3', 7, 1017, CAST(5000.00 AS Decimal(18, 2)), CAST(150000.00 AS Decimal(18, 2)), NULL, CAST(5000.00 AS Decimal(18, 2)), NULL, N'15000000000', NULL, NULL, NULL, NULL, 3, 4, CAST(N'2021-04-27' AS Date), NULL, N'ملاحظات', 3, 1, 1, 0, 3)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId]) VALUES (4070, N'23', 7, 1017, CAST(5000.00 AS Decimal(18, 2)), CAST(150000.00 AS Decimal(18, 2)), NULL, CAST(5000.00 AS Decimal(18, 2)), NULL, N'21321321321', NULL, NULL, NULL, NULL, 3, 4, CAST(N'2021-04-27' AS Date), NULL, NULL, 3, 1, 1, 0, 3)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId]) VALUES (4071, N'50', 7, 1017, CAST(5000.00 AS Decimal(18, 2)), CAST(150000.00 AS Decimal(18, 2)), NULL, CAST(5000.00 AS Decimal(18, 2)), NULL, N'12345648979', NULL, NULL, NULL, NULL, 3, 4, CAST(N'2021-04-27' AS Date), NULL, NULL, 4, 1, 1, 0, 3)
SET IDENTITY_INSERT [dbo].[Order] OFF
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (1, N'عند العميل')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (2, N'في المخزن')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (3, N'في الطريق')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (4, N'تم التسليم')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (5, N'مرتجع كلي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (6, N'مرتجع جزئي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (7, N'مرفوض')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (8, N'مؤجل')
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4068, 1026)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4068, 1027)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4069, 1028)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4069, 1029)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4070, 1030)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4070, 1031)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4071, 1032)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (4071, 1033)
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (1, N'قيد المعالجة')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (2, N'يحب اخذ النقود من العميل')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (3, N'منتهية')
SET IDENTITY_INSERT [dbo].[OrderType] ON 

INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (13, N'نوع 1')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (14, N'نوع 2')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (15, N'نوع 3')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (16, N'new order type xx')
SET IDENTITY_INSERT [dbo].[OrderType] OFF
SET IDENTITY_INSERT [dbo].[Printed] ON 

INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1021, 1, N'مدير 1', CAST(N'2021-03-25T05:13:05.427' AS DateTime), N'مندوب 1', N'46545646545', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1022, 1, N'مدير 1', CAST(N'2021-03-25T05:14:26.600' AS DateTime), N'عميل 1', N'45465456546', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1023, 2, N'مدير 1', CAST(N'2021-03-25T23:54:23.160' AS DateTime), N'عميل 1', N'45465456546', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1024, 2, N'مدير 1', CAST(N'2021-04-24T02:02:02.147' AS DateTime), N'مندوب 1', N'13321131222', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1025, 3, N'مدير 1', CAST(N'2021-04-25T16:07:15.793' AS DateTime), N'مندوب 1', N'13321131222', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1026, 4, N'مدير 1', CAST(N'2021-04-26T17:58:54.497' AS DateTime), N'مندوب 1', N'13321131222', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1027, 3, N'مدير 1', CAST(N'2021-04-26T18:13:22.437' AS DateTime), N'عميل1', N'12345687901', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1028, 5, N'مدير 1', CAST(N'2021-04-27T01:57:21.103' AS DateTime), N'مندوب 1', N'13321131222', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1029, 4, N'مدير 1', CAST(N'2021-04-27T02:13:40.407' AS DateTime), N'عميل1', N'12345687901', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1030, 6, N'مدير 1', CAST(N'2021-04-27T02:14:12.413' AS DateTime), N'مندوب 1', N'13321131222', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1031, 5, N'مدير 1', CAST(N'2021-04-27T02:14:44.033' AS DateTime), N'عميل1', N'12345687901', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1032, 7, N'مدير 1', CAST(N'2021-04-27T02:28:56.847' AS DateTime), N'مندوب2', N'', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (1033, 6, N'مدير 1', CAST(N'2021-04-27T02:29:26.417' AS DateTime), N'عميل1', N'12345687901', N'Client')
SET IDENTITY_INSERT [dbo].[Printed] OFF
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (1, N'عرض المجموعات', N'ShowGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (2, N'إضافة مجموعات', N'AddGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (3, N'التعديل على المجموعات', N'EditGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (4, N'حذف مجموعات', N'DeleteGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (5, N'عرض الموظفين', N'ShowUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (6, N'إضافة موظفين', N'AddUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (7, N'تعديل الموظفين', N'EditUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (8, N'حذف موظفين', N'DeleteUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (9, N'عرض انواع الطلبات', N'ShowOrderType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (10, N'إضافة انواع الطلبات', N'AddOrderType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (11, N'تعديل انواع الطلبات', N'EditOrderType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (12, N'حذف انواع الطلبات', N'DeleteOrderType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (13, N'عرض المدن', N'ShowCountry')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (14, N'إضافة المدن', N'AddCountry')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (15, N'تعديل المدن', N'EditCountry')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (16, N'حذف المدن', N'DeleteCountry')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (17, N'إضافة منطقة', N'AddRegion')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (18, N'تعديل منطقة', N'EditRegion')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (19, N'عرض المناطق', N'ShowRegion')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (20, N'حذف منقطة', N'DeleteRegion')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (21, N'إضافة عملاء', N'AddClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (22, N'عرض العملاء', N'ShowClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (23, N'تعديل العملاء', N'UpdateClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (24, N'حذف العملاء', N'DeleteClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (25, N'عرض العملات', N'ShowCurrency')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (26, N'إضافة عملات', N'AddCurrency')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (27, N'تعديل العملات', N'UpdateCurrency')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (28, N'حذف العملات', N'DeleteCurrency')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (29, N'عرض الأقسام', N'ShowDepartment')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (30, N'إضافة قسم', N'AddDepartment')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (31, N'تعديل قسم', N'UpdateDepartment')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (33, N'عرض انواع الواردات', N'ShowIncomeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (34, N'إضافة انواع الواردات', N'AddIncomeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (35, N'تعديل انواع الواردات', N'UpdateIncomeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (36, N'حذف انواع الواردات', N'DeleteIncomeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (37, N'عرض انواع الصادرات', N'ShowOutComeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (38, N'إضافة انواع الصادرات', N'AddOutComeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (39, N'تعديل انواع الصادرات', N'UpdateOutComeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (40, N'حذف انواع الصادرات', N'DeleteOutComeType')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (41, N'إضافة طلبات', N'AddOrder')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (42, N'تعديل الطلبات', N'UpdateOrder')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (43, N'حذف الطلبات', N'DeleteOrder')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (44, N'عرض الطلبات', N'ShowOrder')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (45, N'اضافة صادرات', N'AddOutCome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (46, N'عرض الصادرات', N'ShowOutCome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (47, N'تعديل الصادرات', N'UpdateOutCome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (48, N'حذف الصادرات', N'DeleteOutCome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (49, N'إضافة واردات', N'AddIncome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (50, N'عرض الواردات', N'ShowIncome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (51, N'تعديل الواردات', N'UpdateIncome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (52, N'حذف الواردات', N'DeleteIncome')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (53, N'عرض التقارير', N'ShowReports')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (54, N'طباعة عميل', N'PrintClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (55, N'طباعة مندوب', N'PrintAgent')
SET IDENTITY_INSERT [dbo].[Region] ON 

INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (10, N'منطقة 1', 1017)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (11, N'منطقة 2', 1017)
SET IDENTITY_INSERT [dbo].[Region] OFF
INSERT [dbo].[UserGroup] ([UserId], [GroupId]) VALUES (1, 1)
SET IDENTITY_INSERT [dbo].[UserPhone] ON 

INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (14, 3, N'13321131222')
SET IDENTITY_INSERT [dbo].[UserPhone] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [CountryId], [Salary], [UserName], [Password], [IsActive]) VALUES (1, N'مدير 1', N'', N'', CAST(N'2020-12-26' AS Date), N'', 0, NULL, CAST(0.00 AS Decimal(18, 2)), N'admin', N'21232f297a57a5a743894a0e4a801fc3', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [CountryId], [Salary], [UserName], [Password], [IsActive]) VALUES (3, N'مندوب 1', NULL, NULL, CAST(N'2021-04-23' AS Date), N'ملاحظات', 1, 1017, CAST(5000.00 AS Decimal(18, 2)), NULL, NULL, 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [CountryId], [Salary], [UserName], [Password], [IsActive]) VALUES (4, N'مندوب2', NULL, NULL, CAST(N'2021-04-25' AS Date), N'ملاحظات', 1, 1017, CAST(5000.00 AS Decimal(18, 2)), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[AgnetPrint]  WITH CHECK ADD  CONSTRAINT [FK_AgnetPrint_Printed] FOREIGN KEY([PrintId])
REFERENCES [dbo].[Printed] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgnetPrint] CHECK CONSTRAINT [FK_AgnetPrint_Printed]
GO
ALTER TABLE [dbo].[clientPhones]  WITH CHECK ADD  CONSTRAINT [FK_clientPhones_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[clientPhones] CHECK CONSTRAINT [FK_clientPhones_Clients]
GO
ALTER TABLE [dbo].[ClientPrint]  WITH CHECK ADD  CONSTRAINT [FK_ClientPrint_Printed] FOREIGN KEY([PrintId])
REFERENCES [dbo].[Printed] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientPrint] CHECK CONSTRAINT [FK_ClientPrint_Printed]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Country]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Users]
GO
ALTER TABLE [dbo].[GroupPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_GroupPrivilege_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupPrivilege] CHECK CONSTRAINT [FK_GroupPrivilege_Group]
GO
ALTER TABLE [dbo].[GroupPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_GroupPrivilege_Privilege] FOREIGN KEY([PrivilegId])
REFERENCES [dbo].[Privilege] ([Id])
GO
ALTER TABLE [dbo].[GroupPrivilege] CHECK CONSTRAINT [FK_GroupPrivilege_Privilege]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [FK_Income_IncomeType] FOREIGN KEY([IncomeTypeId])
REFERENCES [dbo].[IncomeType] ([Id])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [FK_Income_IncomeType]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [FK_Income_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [FK_Income_Users]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Clients]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Country]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_MoenyPlaced] FOREIGN KEY([MoenyPlacedId])
REFERENCES [dbo].[MoenyPlaced] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_MoenyPlaced]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderPlaced] FOREIGN KEY([OrderplacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderPlaced]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderState] FOREIGN KEY([OrderStateId])
REFERENCES [dbo].[OrderState] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_OrderState]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Region]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Users] FOREIGN KEY([AgentId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Users]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderOrderType_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderOrderType_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderOrderType_OrderType] FOREIGN KEY([OrderTpyeId])
REFERENCES [dbo].[OrderType] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderOrderType_OrderType]
GO
ALTER TABLE [dbo].[OrderPrint]  WITH CHECK ADD  CONSTRAINT [FK_OrderPrint_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPrint] CHECK CONSTRAINT [FK_OrderPrint_Order]
GO
ALTER TABLE [dbo].[OrderPrint]  WITH CHECK ADD  CONSTRAINT [FK_OrderPrint_Printed1] FOREIGN KEY([PrintId])
REFERENCES [dbo].[Printed] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPrint] CHECK CONSTRAINT [FK_OrderPrint_Printed1]
GO
ALTER TABLE [dbo].[OutCome]  WITH CHECK ADD  CONSTRAINT [FK_OutCome_OutComeType] FOREIGN KEY([OutComeTypeId])
REFERENCES [dbo].[OutComeType] ([Id])
GO
ALTER TABLE [dbo].[OutCome] CHECK CONSTRAINT [FK_OutCome_OutComeType]
GO
ALTER TABLE [dbo].[OutCome]  WITH CHECK ADD  CONSTRAINT [FK_OutCome_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[OutCome] CHECK CONSTRAINT [FK_OutCome_Users]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Clients]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Reginos_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Reginos_Country]
GO
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_Group]
GO
ALTER TABLE [dbo].[UserGroup]  WITH CHECK ADD  CONSTRAINT [FK_UserGroup_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserGroup] CHECK CONSTRAINT [FK_UserGroup_Users]
GO
ALTER TABLE [dbo].[UserPhone]  WITH CHECK ADD  CONSTRAINT [FK_UserPhone_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserPhone] CHECK CONSTRAINT [FK_UserPhone_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Country]
GO
USE [master]
GO
ALTER DATABASE [Kokaz] SET  READ_WRITE 
GO
