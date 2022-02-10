USE [master]
GO
/****** Object:  Database [Kokaz]    Script Date: 2/10/2022 3:41:59 PM ******/
CREATE DATABASE [Kokaz]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Kokaz', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kokaz.mdf' , SIZE = 12480KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Kokaz_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kokaz_log.ldf' , SIZE = 27200KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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
ALTER DATABASE [Kokaz] SET  ENABLE_BROKER 
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
/****** Object:  Table [dbo].[AgentCountr]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentCountr](
	[AgentId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_AgentCountr] PRIMARY KEY CLUSTERED 
(
	[AgentId] ASC,
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgnetPrint]    Script Date: 2/10/2022 3:42:00 PM ******/
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
	[Region] [nvarchar](50) NULL,
 CONSTRAINT [PK_AgnetPrint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApproveAgentEditOrderRequest]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveAgentEditOrderRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[OrderPlacedId] [int] NOT NULL,
	[NewAmount] [decimal](18, 2) NOT NULL,
	[AgentId] [int] NOT NULL,
	[IsApprove] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clientPhones]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[ClientPrint]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrintId] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[LastTotal] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[DeliveCost] [decimal](18, 2) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[MoneyPlacedId] [int] NULL,
	[OrderPlacedId] [int] NULL,
	[Date] [date] NULL,
	[Note] [nvarchar](max) NULL,
	[PayForClient] [money] NULL,
 CONSTRAINT [PK_ClientPrint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 2/10/2022 3:42:00 PM ******/
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
	[Mail] [nvarchar](max) NULL,
	[Points] [int] NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
	[mediatorId] [int] NULL,
	[IsMain] [bit] NOT NULL,
	[Points] [int] NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[DisAcceptOrder]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisAcceptOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[ClientId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[RecipientName] [nvarchar](50) NULL,
	[RecipientPhones] [nvarchar](max) NOT NULL,
	[RegionId] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[ClientNote] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[Date] [date] NULL,
	[IsDollar] [bit] NOT NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discount]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Points] [int] NOT NULL,
	[Money] [money] NOT NULL,
	[PrintedId] [int] NOT NULL,
 CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EditRequest]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EditRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[OldName] [nvarchar](50) NULL,
	[NewName] [nvarchar](50) NULL,
	[OldUserName] [nvarchar](50) NULL,
	[NewUserName] [nvarchar](50) NULL,
	[UserId] [int] NULL,
	[Accept] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[GroupPrivilege]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[Income]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[IncomeType]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[Market]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Market](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[MarketUrl] [nvarchar](max) NULL,
	[LogoPath] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ClientId] [int] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoenyPlaced]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[Notfication]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notfication](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[OrderCount] [int] NULL,
	[OrderPlacedId] [int] NULL,
	[MoneyPlacedId] [int] NULL,
	[IsSeen] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 2/10/2022 3:42:00 PM ******/
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
	[Date] [datetime] NULL,
	[DiliveryDate] [date] NULL,
	[Note] [nvarchar](max) NULL,
	[AgentId] [int] NULL,
	[seen] [bit] NULL,
	[IsClientDiliverdMoney] [bit] NOT NULL,
	[IsSync] [bit] NOT NULL,
	[OrderStateId] [int] NOT NULL,
	[IsDollar] [bit] NOT NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [date] NULL,
	[SystemNote] [nvarchar](max) NULL,
	[OldDeliveryCost] [money] NULL,
	[IsSend] [bit] NULL,
	[ClientPaied] [money] NULL,
	[CurrentCountry] [int] NULL,
	[PrintedTimes] [int] NOT NULL,
	[AgentRequestStatus] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OrderLog]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLog](
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
	[UpdatedBy] [nvarchar](max) NULL,
	[OrderId] [int] NOT NULL,
	[IsDollar] [bit] NULL,
	[UpdatedDate] [date] NULL,
	[SystemNote] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrderLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderPlaced]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OrderPrint]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OrderState]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OrderType]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OutCome]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[OutComeType]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[PaymentRequest]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[PaymentWayId] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Accept] [bit] NULL,
	[CreateDate] [date] NOT NULL,
 CONSTRAINT [PK__PaymentR__3214EC07D76BAD63] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentWay]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentWay](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointsSetting]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointsSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Points] [int] NOT NULL,
	[Money] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Printed]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Printed](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrintNmber] [int] NOT NULL,
	[PrinterName] [nvarchar](50) NOT NULL,
	[Date] [datetime] NULL,
	[DestinationName] [nvarchar](50) NOT NULL,
	[DestinationPhone] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Printed] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Privilege]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[Receipt]    Script Date: 2/10/2022 3:42:00 PM ******/
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
	[PrintId] [int] NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[UserGroup]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[UserPhone]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 2/10/2022 3:42:00 PM ******/
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
/****** Object:  View [dbo].[v_OrderClientPrnitRepeate]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[v_OrderClientPrnitRepeate]
as 
select o.ClientId,o.Code,COUNT(*) OCount from [Order] o
join
OrderPrint op
on
op.OrderId = o.Id
join
Printed p
on 
p.Id =op.PrintId
where p.[Type] = 'Client'
group by o.ClientId, o.Code

GO
/****** Object:  View [dbo].[v_OrderclientPrintReportWithOrderDeitals]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[v_OrderclientPrintReportWithOrderDeitals] 
as 
select o.Id,o.Code,o.ClientId,o.MoenyPlacedId,o.OrderplacedId,o.Cost,o.DeliveryCost,o.OldCost,o.OldDeliveryCost,o.OrderStateId,p.Id 'PrintId',vocpr.OCount From [Order] o 
join
v_OrderClientPrnitRepeate  vocpr
on
vocpr.ClientId = o.ClientId
and
o.Code = vocpr.Code 
 join 
OrderPrint op
on
op.OrderId = o.Id
join
Printed p
on
p.Id = op.PrintId
where 
p.[Type]='Client'

GO
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (66, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1066, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1066, 2)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 2)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 3)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 6)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1068, 1)
SET IDENTITY_INSERT [dbo].[AgnetPrint] ON 

INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (11806, 2929, N'1', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'23123123123', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (11807, 2929, N'2', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة 1', N'21312312312', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (11808, 2929, N'3', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة 1', N'23123123123', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12806, 3930, N'4', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'23231231212', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12807, 3930, N'5', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'22302893231', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12808, 3932, N'10', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'28203129739', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12809, 3932, N'11', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'13901287391', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12810, 3932, N'12', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'29123172389', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12811, 3935, N'100', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة 1', N'31298838792', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12812, 3936, N'200', CAST(10000.00 AS Decimal(18, 2)), N'مدينة 1', N'02394092349', N'عميل 1', N'', NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12813, 3936, N'201', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'09238490238', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12814, 3936, N'202', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'12931273901', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12815, 3937, N'300', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'12312398127', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12816, 3937, N'301', CAST(190000.00 AS Decimal(18, 2)), N'مدينة 1', N'23098498724', N'عميل 1', NULL, NULL)
INSERT [dbo].[AgnetPrint] ([Id], [PrintId], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region]) VALUES (12817, 3937, N'303', CAST(100000.00 AS Decimal(18, 2)), N'مدينة 1', N'21908309182', N'عميل 1', NULL, NULL)
SET IDENTITY_INSERT [dbo].[AgnetPrint] OFF
SET IDENTITY_INSERT [dbo].[ApproveAgentEditOrderRequest] ON 

INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (18, 12632, 4, CAST(1000000.00 AS Decimal(18, 2)), 66, 1)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (19, 12633, 4, CAST(10000.00 AS Decimal(18, 2)), 66, 1)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (20, 12635, 4, CAST(100000.00 AS Decimal(18, 2)), 66, 1)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (21, 12634, 4, CAST(100000.00 AS Decimal(18, 2)), 66, 1)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (22, 12636, 4, CAST(100000.00 AS Decimal(18, 2)), 66, 1)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (23, 12637, 4, CAST(190000.00 AS Decimal(18, 2)), 66, NULL)
INSERT [dbo].[ApproveAgentEditOrderRequest] ([Id], [OrderId], [OrderPlacedId], [NewAmount], [AgentId], [IsApprove]) VALUES (24, 12638, 4, CAST(100000.00 AS Decimal(18, 2)), 66, NULL)
SET IDENTITY_INSERT [dbo].[ApproveAgentEditOrderRequest] OFF
SET IDENTITY_INSERT [dbo].[clientPhones] ON 

INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (1301, 1294, N'23312321323')
SET IDENTITY_INSERT [dbo].[clientPhones] OFF
SET IDENTITY_INSERT [dbo].[ClientPrint] ON 

INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10061, 3929, N'1', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), N'مدينة 1', N'23123123123', 4, 4, CAST(N'2022-01-06' AS Date), NULL, 90000.0000)
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10062, 3929, N'3', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), N'مدينة 1', N'23123123123', 4, 4, CAST(N'2022-01-06' AS Date), NULL, 990000.0000)
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10063, 3931, N'4', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), N'مدينة 1', N'23231231212', 4, 4, CAST(N'2022-01-10' AS Date), NULL, 90000.0000)
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10064, 3931, N'5', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), N'مدينة 1', N'22302893231', 4, 4, CAST(N'2022-01-10' AS Date), NULL, 90000.0000)
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10065, 3933, N'2', CAST(1000000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'مدينة 1', N'21312312312', 4, 5, CAST(N'2022-01-06' AS Date), NULL, 0.0000)
INSERT [dbo].[ClientPrint] ([Id], [PrintId], [Code], [LastTotal], [Total], [DeliveCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10066, 3933, N'11', CAST(100000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'مدينة 1', N'13901287391', 4, 5, CAST(N'2022-01-11' AS Date), NULL, 0.0000)
SET IDENTITY_INSERT [dbo].[ClientPrint] OFF
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Mail], [Points]) VALUES (1294, N'عميل 1', 1, N'sasd', CAST(N'2022-01-06' AS Date), N'asjdasd', N'client1', N'202cb962ac59075b964b07152d234b70', 65, N'laj@asd.com', 40)
SET IDENTITY_INSERT [dbo].[Clients] OFF
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (1, N'مدينة 1', CAST(10000.00 AS Decimal(18, 2)), NULL, 1, 10)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (2, N'مدينة2', CAST(2000.00 AS Decimal(18, 2)), NULL, 0, 10)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (3, N'country1', CAST(1000.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (6, N'country2', CAST(900.00 AS Decimal(18, 2)), NULL, 0, 90)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (7, N'tt', CAST(10.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (8, N'tt4', CAST(10.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (9, N'newcountry', CAST(1000.00 AS Decimal(18, 2)), 1, 0, 20)
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
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (1, N'خارج الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (2, N'مندوب')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (3, N'داخل الشركة')
INSERT [dbo].[MoenyPlaced] ([Id], [Name]) VALUES (4, N'تم تسليمها')
SET IDENTITY_INSERT [dbo].[Notfication] ON 

INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (101, 1294, N'الطلب 3 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (102, 1294, N'الطلب 2 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (103, 1294, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (104, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (105, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (106, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (107, 1294, N'تم تسديدك برقم 1', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (108, 1294, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (109, 1294, N'الطلب 4 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (110, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (111, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (112, 1294, N'تم تسديدك برقم 2', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (113, 1294, N'الطلب 10 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (114, 1294, N'الطلب 12 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (115, 1294, N'الطلب 11 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (116, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (117, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (118, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (119, 1294, N'تم تسديدك برقم 3', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (120, 1294, N'الطلب 100 اصبح تم التسليم و موقع المبلغ  مندوب', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (121, 1294, NULL, 1, 4, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (122, 1294, N'الطلب 100 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (123, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (124, 1294, N'الطلب 200 اصبح تم التسليم و موقع المبلغ  مندوب', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (125, 1294, N'الطلب 202 اصبح تم التسليم و موقع المبلغ  مندوب', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (126, 1294, N'الطلب 201 اصبح تم التسليم و موقع المبلغ  مندوب', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (127, 1294, NULL, 3, 4, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (128, 1294, N'الطلب 202 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (129, 1294, N'الطلب 201 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (130, 1294, N'الطلب 200 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (131, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (132, 1294, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (133, 1294, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (134, 1294, N'الطلب 200 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (135, 1294, N'الطلب 201 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (136, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (137, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (138, 1294, N'الطلب 300 اصبح تم التسليم و موقع المبلغ  مندوب', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (139, 1294, NULL, 1, 4, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (140, 1294, N'الطلب 300 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (141, 1294, NULL, 1, 3, 3, 0)
SET IDENTITY_INSERT [dbo].[Notfication] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (11627, N'1', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23123123123', NULL, NULL, NULL, N'admin', 4, 4, CAST(N'2022-01-06T22:55:03.530' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, 90000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (11628, N'2', 1294, 1, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'21312312312', NULL, NULL, NULL, N'admin', 4, 5, CAST(N'2022-01-06T22:55:03.530' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', 10000.0000, NULL, 0.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (11629, N'3', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23123123123', NULL, NULL, NULL, N'admin', 4, 4, CAST(N'2022-01-06T22:55:03.530' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, 990000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12627, N'4', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23231231212', NULL, NULL, NULL, N'admin', 4, 4, CAST(N'2022-01-10T20:37:39.147' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, 90000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12628, N'5', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'22302893231', NULL, NULL, NULL, N'admin', 4, 4, CAST(N'2022-01-10T20:37:39.147' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, 90000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12629, N'10', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'28203129739', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-11T23:22:33.407' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12630, N'11', 1294, 1, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'13901287391', NULL, NULL, NULL, N'admin', 4, 5, CAST(N'2022-01-11T23:22:33.407' AS DateTime), NULL, NULL, 66, 1, 1, 0, 3, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', 10000.0000, NULL, 0.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12631, N'12', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'29123172389', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-11T23:22:33.407' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12632, N'100', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'31298838792', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-14T23:56:56.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 2)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12633, N'200', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'02394092349', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-16T18:57:27.000' AS DateTime), NULL, N'', 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 2)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12634, N'201', 1294, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'09238490238', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-16T18:57:27.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 2)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12635, N'202', 1294, 1, CAST(100.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12931273901', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-16T18:57:27.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 2)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12636, N'300', 1294, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12312398127', NULL, NULL, NULL, N'admin', 3, 4, CAST(N'2022-01-16T19:08:19.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', NULL, NULL, NULL, 1, 0, 2)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12637, N'301', 1294, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(190000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23098498724', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-01-16T19:08:19.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, 1)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12638, N'303', 1294, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'21908309182', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-01-16T19:08:19.000' AS DateTime), NULL, NULL, 66, 1, 0, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, 1)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12639, N'21312', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1313123.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), N'sdjaskj', N'12345678910', NULL, N'wsdkjasdkj', N'jkdfjkshdfkjasdfkjshdf~', N'عميل 1', 1, 1, CAST(N'2022-01-31T03:11:42.687' AS DateTime), NULL, NULL, NULL, NULL, 0, 0, 1, 0, NULL, NULL, NULL, NULL, 1, NULL, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12640, N'123123', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), N'sdkjfdslkj', N'12312312312', NULL, N'sdkjh', N'sdashdkjhasdkjh', N'عميل 1', 1, 1, CAST(N'2022-01-31T03:15:47.967' AS DateTime), NULL, NULL, NULL, NULL, 0, 0, 1, 0, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12641, N'400', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), N'dsj', N'23091283901', NULL, N'jkfsd', N'dfhakdhfkjashdfkjhdfkjhd', N'عميل 1', 1, 1, CAST(N'2022-01-31T03:31:30.977' AS DateTime), NULL, NULL, NULL, NULL, 0, 0, 1, 0, NULL, NULL, NULL, NULL, 1, NULL, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (12642, N'9911', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), N'string', N'12738912739081', NULL, N'string', N'string', N'عميل 1', 1, 1, CAST(N'2022-01-31T03:48:29.843' AS DateTime), NULL, NULL, NULL, NULL, 0, 0, 1, 0, NULL, NULL, NULL, NULL, 1, NULL, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[Order] OFF
SET IDENTITY_INSERT [dbo].[OrderLog] ON 

INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8948, N'3', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23123123123', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 11629, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8949, N'2', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'21312312312', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 11628, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8950, N'1', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23123123123', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 11627, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8951, N'5', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'22302893231', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12628, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8952, N'4', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'23231231212', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12627, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8953, N'10', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'28203129739', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12629, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8954, N'12', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'29123172389', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12631, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8955, N'11', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'13901287391', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12630, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8956, N'100', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'31298838792', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12632, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8957, N'100', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'31298838792', NULL, NULL, NULL, 2, 4, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12632, NULL, NULL, N'OrderRequestEditStateCount')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8958, N'200', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'02394092349', NULL, NULL, NULL, 1, 3, NULL, NULL, N'', 66, 1, 0, 0, 1, N'admin', 12633, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8959, N'202', 1294, 1, CAST(100.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12931273901', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12635, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8960, N'201', 1294, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'09238490238', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12634, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8961, N'202', 1294, 1, CAST(100.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12931273901', NULL, NULL, NULL, 2, 4, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12635, NULL, NULL, N'OrderRequestEditStateCount')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8962, N'201', 1294, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'09238490238', NULL, NULL, NULL, 2, 4, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12634, NULL, NULL, N'OrderRequestEditStateCount')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8963, N'200', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'02394092349', NULL, NULL, NULL, 2, 4, NULL, NULL, N'', 66, 1, 0, 0, 1, N'admin', 12633, NULL, NULL, N'OrderRequestEditStateCount')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8964, N'200', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'02394092349', NULL, NULL, NULL, 2, 4, NULL, NULL, N'', 66, 1, 0, 0, 1, N'admin', 12633, NULL, NULL, N'UpdateOrdersStatusFromAgent')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8965, N'201', 1294, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'09238490238', NULL, NULL, NULL, 2, 4, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12634, NULL, NULL, N'UpdateOrdersStatusFromAgent')
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8966, N'300', 1294, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12312398127', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12636, NULL, NULL, NULL)
INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (8967, N'300', 1294, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), NULL, N'12312398127', NULL, NULL, NULL, 2, 4, NULL, NULL, NULL, 66, 1, 0, 0, 1, N'admin', 12636, NULL, NULL, N'OrderRequestEditStateCount')
SET IDENTITY_INSERT [dbo].[OrderLog] OFF
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (1, N'عند العميل')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (2, N'في المخزن')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (3, N'في الطريق')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (4, N'تم التسليم')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (5, N'مرتجع كلي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (6, N'مرتجع جزئي')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (7, N'مرفوض')
INSERT [dbo].[OrderPlaced] ([Id], [Name]) VALUES (8, N'مؤجل')
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11627, 2929)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11627, 3929)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11628, 2929)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11628, 3933)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11629, 2929)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (11629, 3929)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12627, 3930)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12627, 3931)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12628, 3930)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12628, 3931)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12629, 3932)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12630, 3932)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12630, 3933)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12631, 3932)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12632, 3935)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12633, 3936)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12634, 3936)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12635, 3936)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12636, 3937)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12637, 3937)
INSERT [dbo].[OrderPrint] ([OrderId], [PrintId]) VALUES (12638, 3937)
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (1, N'قيد المعالجة')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (2, N'يحب اخذ النقود من العميل')
INSERT [dbo].[OrderState] ([Id], [State]) VALUES (3, N'منتهية')
SET IDENTITY_INSERT [dbo].[PaymentRequest] ON 

INSERT [dbo].[PaymentRequest] ([Id], [ClientId], [PaymentWayId], [Note], [Accept], [CreateDate]) VALUES (5, 1294, 6, N'czxc', NULL, CAST(N'2022-01-31' AS Date))
SET IDENTITY_INSERT [dbo].[PaymentRequest] OFF
SET IDENTITY_INSERT [dbo].[PaymentWay] ON 

INSERT [dbo].[PaymentWay] ([Id], [Name]) VALUES (6, N'PayPal')
SET IDENTITY_INSERT [dbo].[PaymentWay] OFF
SET IDENTITY_INSERT [dbo].[Printed] ON 

INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (2929, 1, N'admin', CAST(N'2022-01-06T22:55:25.683' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3929, 1, N'admin', CAST(N'2022-01-10T20:33:18.680' AS DateTime), N'عميل 1', N'23312321323', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3930, 2, N'admin', CAST(N'2022-01-10T20:37:58.450' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3931, 2, N'admin', CAST(N'2022-01-10T20:39:00.153' AS DateTime), N'عميل 1', N'23312321323', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3932, 3, N'admin', CAST(N'2022-01-11T23:51:54.380' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3933, 3, N'admin', CAST(N'2022-01-12T00:19:18.310' AS DateTime), N'عميل 1', N'23312321323', N'Client')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3935, 4, N'admin', CAST(N'2022-01-14T23:57:08.000' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3936, 5, N'admin', CAST(N'2022-01-16T18:57:43.000' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
INSERT [dbo].[Printed] ([Id], [PrintNmber], [PrinterName], [Date], [DestinationName], [DestinationPhone], [Type]) VALUES (3937, 6, N'admin', CAST(N'2022-01-16T19:08:29.000' AS DateTime), N'مندوب 1', N'15161561651', N'Agent')
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

INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (7, N'r3', 3)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (9, N'r4', 3)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (12, N'c2r2', 6)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (15, N'asdasd', 7)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (16, N'asdas', 7)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (17, N'asdasd', 8)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (18, N't_2', 8)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (19, N'sfhsdfh', 8)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (20, N'region2', 9)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (21, N'Region1', 1)
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (22, N'تجريب', 3)
SET IDENTITY_INSERT [dbo].[Region] OFF
INSERT [dbo].[UserGroup] ([UserId], [GroupId]) VALUES (65, 1)
SET IDENTITY_INSERT [dbo].[UserPhone] ON 

INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (58, 66, N'15161561651')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1058, 1066, N'12312312379')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1059, 1067, N'12937012987')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1060, 1068, N'32493827490')
SET IDENTITY_INSERT [dbo].[UserPhone] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (65, N'admin', NULL, NULL, CAST(N'2022-01-01' AS Date), NULL, 0, NULL, N'admin', N'21232f297a57a5a743894a0e4a801fc3', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (66, N'مندوب 1', NULL, NULL, CAST(N'2022-01-06' AS Date), N'kjdsd', 1, CAST(0.00 AS Decimal(18, 2)), N'agent1', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1066, N'مندوب ', NULL, NULL, CAST(N'2022-02-10' AS Date), N'asjdfkjfksjf', 1, CAST(100000.00 AS Decimal(18, 2)), N'aa', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1067, N'bb', NULL, NULL, CAST(N'2022-02-10' AS Date), N'jsahdksjafs', 1, CAST(1000.00 AS Decimal(18, 2)), N'bb', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1068, N'zz', NULL, NULL, CAST(N'2022-02-10' AS Date), N'skldjkldasjfklasdjfldaksh', 1, CAST(31313.00 AS Decimal(18, 2)), N'zz', N'202cb962ac59075b964b07152d234b70', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [UQ__PointsSe__DA8267865B952CBA]    Script Date: 2/10/2022 3:42:00 PM ******/
ALTER TABLE [dbo].[PointsSetting] ADD UNIQUE NONCLUSTERED 
(
	[Points] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__PointsSe__FA951B4625038075]    Script Date: 2/10/2022 3:42:00 PM ******/
ALTER TABLE [dbo].[PointsSetting] ADD UNIQUE NONCLUSTERED 
(
	[Money] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AgnetPrint] ADD  DEFAULT (NULL) FOR [Region]
GO
ALTER TABLE [dbo].[ClientPrint] ADD  DEFAULT (NULL) FOR [Date]
GO
ALTER TABLE [dbo].[ClientPrint] ADD  DEFAULT (NULL) FOR [Note]
GO
ALTER TABLE [dbo].[Clients] ADD  DEFAULT (NULL) FOR [Mail]
GO
ALTER TABLE [dbo].[Clients] ADD  DEFAULT ((0)) FOR [Points]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF__Country__mediato__73852659]  DEFAULT (NULL) FOR [mediatorId]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF__Country__IsMain__1A9EF37A]  DEFAULT ((0)) FOR [IsMain]
GO
ALTER TABLE [dbo].[Country] ADD  DEFAULT ((0)) FOR [Points]
GO
ALTER TABLE [dbo].[DisAcceptOrder] ADD  DEFAULT (NULL) FOR [UpdatedBy]
GO
ALTER TABLE [dbo].[DisAcceptOrder] ADD  DEFAULT (NULL) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Market] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Notfication] ADD  DEFAULT ((0)) FOR [IsSeen]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__UpdatedBy__2E1BDC42]  DEFAULT (NULL) FOR [UpdatedBy]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__UpdatedDa__2F10007B]  DEFAULT (NULL) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__SystemNot__300424B4]  DEFAULT (NULL) FOR [SystemNote]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__IsSend__30F848ED]  DEFAULT (NULL) FOR [IsSend]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__CurrentCo__0697FACD]  DEFAULT (NULL) FOR [CurrentCountry]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [PrintedTimes]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [AgentRequestStatus]
GO
ALTER TABLE [dbo].[OrderLog] ADD  DEFAULT (NULL) FOR [SystemNote]
GO
ALTER TABLE [dbo].[AgentCountr]  WITH CHECK ADD  CONSTRAINT [FK_AgentCountr_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentCountr] CHECK CONSTRAINT [FK_AgentCountr_Country]
GO
ALTER TABLE [dbo].[AgentCountr]  WITH CHECK ADD  CONSTRAINT [FK_AgentCountr_Users] FOREIGN KEY([AgentId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgentCountr] CHECK CONSTRAINT [FK_AgentCountr_Users]
GO
ALTER TABLE [dbo].[AgnetPrint]  WITH CHECK ADD  CONSTRAINT [FK_AgnetPrint_Printed] FOREIGN KEY([PrintId])
REFERENCES [dbo].[Printed] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AgnetPrint] CHECK CONSTRAINT [FK_AgnetPrint_Printed]
GO
ALTER TABLE [dbo].[ApproveAgentEditOrderRequest]  WITH CHECK ADD FOREIGN KEY([AgentId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ApproveAgentEditOrderRequest]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[ApproveAgentEditOrderRequest]  WITH CHECK ADD FOREIGN KEY([OrderPlacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
GO
ALTER TABLE [dbo].[clientPhones]  WITH CHECK ADD  CONSTRAINT [FK_clientPhones_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[clientPhones] CHECK CONSTRAINT [FK_clientPhones_Clients]
GO
ALTER TABLE [dbo].[ClientPrint]  WITH CHECK ADD FOREIGN KEY([MoneyPlacedId])
REFERENCES [dbo].[MoenyPlaced] ([Id])
GO
ALTER TABLE [dbo].[ClientPrint]  WITH CHECK ADD FOREIGN KEY([OrderPlacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
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
ALTER TABLE [dbo].[Country]  WITH CHECK ADD  CONSTRAINT [FK__Country__mediato__74794A92] FOREIGN KEY([mediatorId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Country] CHECK CONSTRAINT [FK__Country__mediato__74794A92]
GO
ALTER TABLE [dbo].[DisAcceptOrder]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[DisAcceptOrder]  WITH CHECK ADD  CONSTRAINT [FK__DisAccept__Count__0D7A0286] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[DisAcceptOrder] CHECK CONSTRAINT [FK__DisAccept__Count__0D7A0286]
GO
ALTER TABLE [dbo].[DisAcceptOrder]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Discount]  WITH CHECK ADD  CONSTRAINT [FK_Discount_Printed] FOREIGN KEY([PrintedId])
REFERENCES [dbo].[Printed] ([Id])
GO
ALTER TABLE [dbo].[Discount] CHECK CONSTRAINT [FK_Discount_Printed]
GO
ALTER TABLE [dbo].[EditRequest]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[EditRequest]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
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
ALTER TABLE [dbo].[Market]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Notfication]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Notfication]  WITH CHECK ADD FOREIGN KEY([MoneyPlacedId])
REFERENCES [dbo].[MoenyPlaced] ([Id])
GO
ALTER TABLE [dbo].[Notfication]  WITH CHECK ADD FOREIGN KEY([OrderPlacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK__Order__CurrentCo__078C1F06] FOREIGN KEY([CurrentCountry])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK__Order__CurrentCo__078C1F06]
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
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_Clients]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_Country]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_MoenyPlaced] FOREIGN KEY([MoenyPlacedId])
REFERENCES [dbo].[MoenyPlaced] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_MoenyPlaced]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_Order]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_OrderPlaced] FOREIGN KEY([OrderplacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_OrderPlaced]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_Region]
GO
ALTER TABLE [dbo].[OrderLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderLog_Users] FOREIGN KEY([AgentId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[OrderLog] CHECK CONSTRAINT [FK_OrderLog_Users]
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
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK__PaymentRe__Clien__5AB9788F] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK__PaymentRe__Clien__5AB9788F]
GO
ALTER TABLE [dbo].[PaymentRequest]  WITH CHECK ADD  CONSTRAINT [FK__PaymentRe__Payme__5BAD9CC8] FOREIGN KEY([PaymentWayId])
REFERENCES [dbo].[PaymentWay] ([Id])
GO
ALTER TABLE [dbo].[PaymentRequest] CHECK CONSTRAINT [FK__PaymentRe__Payme__5BAD9CC8]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Clients]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Printed] FOREIGN KEY([PrintId])
REFERENCES [dbo].[Printed] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Printed]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Reginos_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
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
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserPhone] CHECK CONSTRAINT [FK_UserPhone_Users]
GO
/****** Object:  StoredProcedure [dbo].[OrderWithClientPrint]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[OrderWithClientPrint] 
@OrderId int
as 
begin

select o.Id 'OrderId',o.Code 'OrderCode' ,
o.ClientId 'OrderClientId',o.CountryId 'OrderCountryId ',
o.DeliveryCost 'OrderDeliveryCost',o.Cost 'OrderCost',
o.OldCost 'OrderOldCost',o.OldDeliveryCost ' OrderOldDeliveryCost',
o.CreatedBy 'OrderCreatedBy',o.MoenyPlacedId 'OrderMoenyPlacedId',
o.OrderplacedId  'OrderOrderplacedId',o.[Date] 'OrderDate',
o.DiliveryDate 'OrderDiliveryDate',o.Note 'OrderNote',o.IsClientDiliverdMoney,
o.ClientPaied 'OrderClientPaied',
P.Id 'PrintId',p.[Date] 'PrintDate',
p.PrintNmber 'PrintPtintNumber',p.PrinterName,p.DestinationName,
cp.Id 'ClientPrintId',cp.LastTotal 'ClientPrintLastTotal',
cp.Total 'ClientPtintTotal',cp.DeliveCost 'ClientPrintDeliveryCost',
cp.MoneyPlacedId 'ClientPrintMoneyPlacedId', cp.OrderPlacedId 'ClientPrintOrderPlaced',
cp.[Date] 'ClientPrintDate',cp.PayForClient 'ClientPrintPayForClient'
 from [OrderPrint] op
join
Printed p 
on p.Id = op.PrintId
Join
[Order] o
on
o.Id = op.OrderId
join
 ClientPrint cp 
 on 
 cp.PrintId = p.Id and cp.Code = o.Code
 where 
 op.OrderId = 11463
 and 
 p.Type = 'Client'
end
GO
/****** Object:  StoredProcedure [dbo].[sp_empty]    Script Date: 2/10/2022 3:42:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_empty]
as
begin
delete from AgentCountr;
delete from AgnetPrint;
delete from ApproveAgentEditOrderRequest;
delete from clientPhones;
delete from ClientPrint;
delete from Discount;
delete from Receipt;
delete from Printed;
delete from Country;
delete from Currency;
delete from DisAcceptOrder;
delete from EditRequest;
delete from Income;
delete from OutCome;
delete from Market;
delete from Notfication;
delete from [Order];
delete from OrderItem;
delete from OrderLog;
delete from OrderPrint;
delete from OrderType;
delete from OutCome;
delete from OutComeType;
delete from PaymentRequest;
delete from PaymentWay;
delete from PointsSetting;
delete from Region;

end;
GO
USE [master]
GO
ALTER DATABASE [Kokaz] SET  READ_WRITE 
GO
