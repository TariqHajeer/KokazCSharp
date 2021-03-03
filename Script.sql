USE [master]
GO
/****** Object:  Database [Kokaz]    Script Date: 3/3/2021 8:53:11 PM ******/
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
/****** Object:  Table [dbo].[clientPhones]    Script Date: 3/3/2021 8:53:11 PM ******/
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
/****** Object:  Table [dbo].[Clients]    Script Date: 3/3/2021 8:53:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[RegionId] [int] NULL,
	[Address] [nvarchar](50) NULL,
	[FirstDate] [date] NOT NULL,
	[Note] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Currency]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Group]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[GroupPrivilege]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Income]    Script Date: 3/3/2021 8:53:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Income](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
	[Source] [nvarchar](50) NOT NULL,
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
/****** Object:  Table [dbo].[IncomeType]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[MoenyPlaced]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Order]    Script Date: 3/3/2021 8:53:12 PM ******/
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
	[AgentPrintNumber] [int] NULL,
	[ClientPrintNumber] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[OrderPlaced]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[OrderState]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[OrderType]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[OutCome]    Script Date: 3/3/2021 8:53:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutCome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyId] [int] NOT NULL,
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
/****** Object:  Table [dbo].[OutComeType]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Privilege]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Region]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[UserGroup]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[UserPhone]    Script Date: 3/3/2021 8:53:12 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 3/3/2021 8:53:12 PM ******/
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
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[clientPhones] ON 

INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (1, 1010, N'897897987')
SET IDENTITY_INSERT [dbo].[clientPhones] OFF
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id], [Name], [RegionId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId]) VALUES (1010, N'شركة القوقز 2', 3028, N'adresssssss', CAST(N'2014-05-15' AS Date), N'ملاحظات', N'quqaz', N'46f94c8de14fb36680850768ff1b7f2a', 1)
SET IDENTITY_INSERT [dbo].[Clients] OFF
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost]) VALUES (1011, N'بغداد', CAST(2000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[Group] ON 

INSERT [dbo].[Group] ([Id], [Name]) VALUES (1, N'مجموعة المدراء')
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
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (1, N'خارج الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (2, N'لدى المندوب')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (3, N'دخال الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (4, N'تم تسليمها')
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [AgentPrintNumber], [ClientPrintNumber]) VALUES (6, N'1', 1010, 1011, CAST(2000.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), NULL, NULL, N'111', NULL, NULL, NULL, NULL, 4, 4, NULL, NULL, NULL, 7, 1, 1, 0, 3, NULL, NULL)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [AgentPrintNumber], [ClientPrintNumber]) VALUES (7, N'2', 1010, 1011, CAST(2000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL, NULL, N'654654', NULL, NULL, NULL, NULL, 4, 4, NULL, NULL, NULL, 7, 1, 1, 0, 3, NULL, NULL)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [AgentPrintNumber], [ClientPrintNumber]) VALUES (8, N'3', 1010, 1011, CAST(2000.00 AS Decimal(18, 2)), CAST(60000.00 AS Decimal(18, 2)), NULL, NULL, N'565464', NULL, NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 7, 1, 1, 0, 1, NULL, NULL)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [AgentPrintNumber], [ClientPrintNumber]) VALUES (9, N'4', 1010, 1011, CAST(2000.00 AS Decimal(18, 2)), CAST(68879.00 AS Decimal(18, 2)), NULL, NULL, N'54654', NULL, NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 7, 1, 1, 0, 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Order] OFF
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (1, N'عند العميل')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (2, N'في المخزن')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (3, N'في الطريق')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (4, N'تم التسليم')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (5, N'مرتجع كلي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (6, N'مرتجع جزئي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (7, N'مرفوض')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (8, N'مؤجل')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (1, N'قيد المعالجة')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (2, N'يحب اخذ النقود من العميل')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (3, N'منتهية')
SET IDENTITY_INSERT [dbo].[OrderType] ON 

INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (13, N'نوع 1')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (14, N'نوع 2')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (15, N'نوع 3')
INSERT [dbo].[OrderType] ([Id], [Name]) VALUES (16, N'new order type xx')
SET IDENTITY_INSERT [dbo].[OrderType] OFF
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (1, N'عرض المجموعات', N'ShowGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (2, N'إضافة مجموعات', N'AddGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (3, N'التعديل على المجموعات', N'EditGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (4, N'حذف مجموعات', N'DeleteGroup')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (5, N'عرض الموظفين', N'ShowUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (6, N'إضافة موظفين', N'AddUser')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (7, N'تعديل المظفين', N'EditUser')
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
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (23, N'تعديل المعلاء', N'UpdateClient')
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (24, N'حذف عملاء', N'DeleteClient')
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
SET IDENTITY_INSERT [dbo].[Region] ON 

INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (3026, N'منقطة1', 1011)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (3028, N'منطقة 2', 1011)
SET IDENTITY_INSERT [dbo].[Region] OFF
INSERT [dbo].[UserGroup] ([UserId], [GroupId]) VALUES (1, 1)
SET IDENTITY_INSERT [dbo].[UserPhone] ON 

INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1, 1, N'12345')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (9, 1, N'54321')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (10, 7, N'4564654')
SET IDENTITY_INSERT [dbo].[UserPhone] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [CountryId], [Salary], [UserName], [Password]) VALUES (1, N'مدير 1', N'', N'', CAST(N'2020-12-26' AS Date), N'', 0, NULL, CAST(0.00 AS Decimal(18, 2)), N'admin', N'21232f297a57a5a743894a0e4a801fc3')
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [CountryId], [Salary], [UserName], [Password]) VALUES (7, N'مندوب 1', NULL, NULL, CAST(N'0001-01-01' AS Date), N'ملاحظات', 1, 1011, CAST(1500.00 AS Decimal(18, 2)), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[clientPhones]  WITH CHECK ADD  CONSTRAINT [FK_clientPhones_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[clientPhones] CHECK CONSTRAINT [FK_clientPhones_Clients]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Reginos] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Reginos]
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
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [FK_Income_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [FK_Income_Currency]
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
ALTER TABLE [dbo].[OutCome]  WITH CHECK ADD  CONSTRAINT [FK_OutCome_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[OutCome] CHECK CONSTRAINT [FK_OutCome_Currency]
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
