USE [master]
GO
/****** Object:  Database [Kokaz]    Script Date: 5/3/2022 7:08:34 AM ******/
CREATE DATABASE [Kokaz]
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
ALTER DATABASE [Kokaz] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Kokaz] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Kokaz', N'ON'
GO
ALTER DATABASE [Kokaz] SET QUERY_STORE = OFF
GO
USE [Kokaz]
GO
ALTER DATABASE SCOPED CONFIGURATION SET ACCELERATED_PLAN_FORCING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_ADAPTIVE_JOINS = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_MEMORY_GRANT_FEEDBACK = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_ON_ROWSTORE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DEFERRED_COMPILATION_TV = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_ONLINE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_RESUMABLE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET GLOBAL_TEMPORARY_TABLE_AUTO_DROP = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET INTERLEAVED_EXECUTION_TVF = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ISOLATE_SECURITY_POLICY_CARDINALITY = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LAST_QUERY_PLAN_STATS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LIGHTWEIGHT_QUERY_PROFILING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET OPTIMIZE_FOR_AD_HOC_WORKLOADS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ROW_MODE_MEMORY_GRANT_FEEDBACK = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET TSQL_SCALAR_UDF_INLINING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET VERBOSE_TRUNCATION_WARNINGS = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_PROCEDURE_EXECUTION_STATISTICS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_QUERY_EXECUTION_STATISTICS = OFF;
GO
USE [Kokaz]
GO
/****** Object:  Table [dbo].[AgentCountr]    Script Date: 5/3/2022 7:08:34 AM ******/
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
/****** Object:  Table [dbo].[AgentOrderPrint]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentOrderPrint](
	[AgentPrintId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[AgentPrintId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentPrint]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentPrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrinterName] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[DestinationName] [nvarchar](50) NOT NULL,
	[DestinationPhone] [varchar](11) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentPrintDetails]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentPrintDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](15) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Country] [nvarchar](30) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[ClientName] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Region] [nvarchar](50) NULL,
	[AgentPrintId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApproveAgentEditOrderRequest]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[CashMovment]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CashMovment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TreasuryId] [int] NOT NULL,
	[CreatedOnUtc] [date] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientPayment]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPayment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrinterName] [nvarchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[DestinationName] [nvarchar](50) NOT NULL,
	[DestinationPhone] [varchar](11) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientPaymentDetails]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPaymentDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientPaymentId] [int] NULL,
	[Code] [nvarchar](50) NOT NULL,
	[LastTotal] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[MoneyPlacedId] [int] NULL,
	[OrderPlacedId] [int] NULL,
	[Date] [date] NULL,
	[Note] [nvarchar](max) NULL,
	[PayForClient] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clientPhones]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Clients]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Country]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[DisAcceptOrder]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Discount]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Points] [int] NOT NULL,
	[Money] [money] NOT NULL,
	[ClientPaymentId] [int] NULL,
 CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EditRequest]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Group]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[GroupPrivilege]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Income]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[IncomeType]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Market]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[MoenyPlaced]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Notfication]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Order]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OrderClientPaymnet]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderClientPaymnet](
	[OrderId] [int] NOT NULL,
	[ClientPaymentId] [int] NOT NULL,
 CONSTRAINT [Pk_OrderClientPaymnet] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ClientPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderFromExcel]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderFromExcel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[RecipientName] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Phone] [nvarchar](15) NULL,
	[Note] [nvarchar](max) NULL,
	[ClientId] [int] NOT NULL,
	[CreateDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OrderLog]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OrderPlaced]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OrderState]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OrderType]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OutCome]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[OutComeType]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[PaymentRequest]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[PaymentWay]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[PointsSetting]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Privilege]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Receipt]    Script Date: 5/3/2022 7:08:35 AM ******/
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
	[ClientPaymentId] [int] NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Treasury]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Treasury](
	[Id] [int] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[CreateOnUtc] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TreasuryHistory]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TreasuryHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TreasuryId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CreatedOnUtc] [date] NOT NULL,
	[ClientPaymentId] [int] NULL,
	[CashMovmentId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[UserPhone]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 5/3/2022 7:08:35 AM ******/
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
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (66, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 2)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 3)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1067, 6)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1068, 1)
INSERT [dbo].[AgentCountr] ([AgentId], [CountryId]) VALUES (1069, 34)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (1, 13745)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (2, 13745)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (1, 13746)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (2, 13747)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (2, 13748)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (2, 13749)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13750)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13751)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13752)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13753)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13754)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13755)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13756)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13757)
INSERT [dbo].[AgentOrderPrint] ([AgentPrintId], [OrderId]) VALUES (3, 13758)
SET IDENTITY_INSERT [dbo].[AgentPrint] ON 

INSERT [dbo].[AgentPrint] ([Id], [PrinterName], [Date], [DestinationName], [DestinationPhone]) VALUES (1, N'admin', CAST(N'2022-04-16' AS Date), N'مندوب نقل البيانات', N'99999999999')
INSERT [dbo].[AgentPrint] ([Id], [PrinterName], [Date], [DestinationName], [DestinationPhone]) VALUES (2, N'admin', CAST(N'2022-04-16' AS Date), N'مندوب نقل البيانات', N'99999999999')
INSERT [dbo].[AgentPrint] ([Id], [PrinterName], [Date], [DestinationName], [DestinationPhone]) VALUES (3, N'admin', CAST(N'2022-04-16' AS Date), N'مندوب نقل البيانات', N'99999999999')
SET IDENTITY_INSERT [dbo].[AgentPrint] OFF
SET IDENTITY_INSERT [dbo].[AgentPrintDetails] ON 

INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (1, N'1', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'15165165165', N'عميل نقل البيانات', NULL, NULL, 1)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (2, N'2', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'21321321321', N'عميل نقل البيانات', NULL, NULL, 1)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (3, N'1', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'15165165165', N'عميل نقل البيانات', NULL, NULL, 2)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (4, N'3', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'13213213213', N'عميل نقل البيانات', NULL, NULL, 2)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (5, N'4', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32132132132', N'عميل نقل البيانات', NULL, NULL, 2)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (6, N'5', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'23132132132', N'عميل نقل البيانات', NULL, NULL, 2)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (7, N'12', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32132132132', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (8, N'11', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32213213213', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (9, N'10', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32132132132', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (10, N'13', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32132132132', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (11, N'9', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'32132132132', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (12, N'14', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'12321321321', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (13, N'7', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'21321321321', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (14, N'6', CAST(100000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'21321323213', N'عميل نقل البيانات', NULL, NULL, 3)
INSERT [dbo].[AgentPrintDetails] ([Id], [Code], [Total], [Country], [Phone], [ClientName], [Note], [Region], [AgentPrintId]) VALUES (15, N'8', CAST(1000000.00 AS Decimal(18, 2)), N'مدينة تجريب نقل البيانات', N'01321321321', N'عميل نقل البيانات', NULL, NULL, 3)
SET IDENTITY_INSERT [dbo].[AgentPrintDetails] OFF
SET IDENTITY_INSERT [dbo].[ClientPayment] ON 

INSERT [dbo].[ClientPayment] ([Id], [PrinterName], [Date], [DestinationName], [DestinationPhone]) VALUES (1, N'admin', CAST(N'2022-04-21' AS Date), N'عميل نقل البيانات', N'99999999999')
SET IDENTITY_INSERT [dbo].[ClientPayment] OFF
SET IDENTITY_INSERT [dbo].[ClientPaymentDetails] ON 

INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (1, 1, N'1', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'1', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (2, 1, N'2', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'2', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (3, 1, N'3', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'1', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (4, 1, N'4', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (5, 1, N'5', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'2', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (6, 1, N'6', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'2', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (7, 1, N'7', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'2', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (8, 1, N'8', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'0', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (9, 1, N'9', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (10, 1, N'10', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (11, 1, N'11', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (12, 1, N'12', NULL, CAST(100000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 95000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (13, 1, N'13', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'3', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
INSERT [dbo].[ClientPaymentDetails] ([Id], [ClientPaymentId], [Code], [LastTotal], [Total], [DeliveryCost], [Country], [Phone], [MoneyPlacedId], [OrderPlacedId], [Date], [Note], [PayForClient]) VALUES (14, 1, N'14', NULL, CAST(1000000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'م', N'1', 1, 3, CAST(N'2022-04-16' AS Date), NULL, 995000.0000)
SET IDENTITY_INSERT [dbo].[ClientPaymentDetails] OFF
SET IDENTITY_INSERT [dbo].[clientPhones] ON 

INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (1301, 1294, N'23312321323')
INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (2301, 2294, N'21831289739')
INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (2302, 2295, N'54565654564')
INSERT [dbo].[clientPhones] ([Id], [ClientId], [Phone]) VALUES (2303, 2296, N'99999999999')
SET IDENTITY_INSERT [dbo].[clientPhones] OFF
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Mail], [Points]) VALUES (1294, N'kkkknnnk', 2, N'ccc', CAST(N'2022-02-10' AS Date), N'nvnvn', N'zzz', N'9336ebf25087d91c818ee6e9ec29f8c1', 65, N'asd@asd.com', 40)
INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Mail], [Points]) VALUES (2294, N'عميل1', 2, N'sadjasld', CAST(N'2022-03-04' AS Date), NULL, N'client1', N'202cb962ac59075b964b07152d234b70', 65, NULL, 100)
INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Mail], [Points]) VALUES (2295, N'عميل تسديد شركات ', NULL, N' ', CAST(N'2022-03-06' AS Date), NULL, N'client3', N'202cb962ac59075b964b07152d234b70', 65, NULL, 0)
INSERT [dbo].[Clients] ([Id], [Name], [CountryId], [Address], [FirstDate], [Note], [UserName], [Password], [UserId], [Mail], [Points]) VALUES (2296, N'عميل نقل البيانات', 34, N'شسمينماشسم', CAST(N'2022-04-01' AS Date), N'asdasd', N'clientTransfer', N'202cb962ac59075b964b07152d234b70', 65, N'transfer@gmail.com', 220)
SET IDENTITY_INSERT [dbo].[Clients] OFF
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (1, N'TTTCCCTTTCCC', CAST(10000.00 AS Decimal(18, 2)), NULL, 1, 10)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (2, N'مدينة2', CAST(2000.00 AS Decimal(18, 2)), NULL, 0, 10)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (3, N'country1', CAST(1000.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (6, N'country2', CAST(900.00 AS Decimal(18, 2)), NULL, 0, 90)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (7, N'tt', CAST(10.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (8, N'tt4', CAST(10.00 AS Decimal(18, 2)), NULL, 0, 100)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (9, N'newcountry', CAST(1000.00 AS Decimal(18, 2)), 1, 0, 20)
INSERT [dbo].[Country] ([Id], [Name], [DeliveryCost], [mediatorId], [IsMain], [Points]) VALUES (34, N'مدينة تجريب نقل البيانات', CAST(5000.00 AS Decimal(18, 2)), NULL, 0, 10)
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[DisAcceptOrder] ON 

INSERT [dbo].[DisAcceptOrder] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [Date], [IsDollar], [UpdatedBy], [UpdatedDate]) VALUES (1, N'21312', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(1313123.00 AS Decimal(18, 2)), N'sdjaskj', N'12345678910', NULL, N'wsdkjasdkj', N'jkdfjkshdfkjasdfkjshdf~', N'عميل 1', CAST(N'2022-01-31' AS Date), 0, N'admin', CAST(N'2022-03-15' AS Date))
INSERT [dbo].[DisAcceptOrder] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [Date], [IsDollar], [UpdatedBy], [UpdatedDate]) VALUES (2, N'400', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), N'dsj', N'23091283901', NULL, N'jkfsd', N'dfhakdhfkjashdfkjhdfkjhd', N'عميل 1', CAST(N'2022-01-31' AS Date), 0, N'admin', CAST(N'2022-03-15' AS Date))
INSERT [dbo].[DisAcceptOrder] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [Date], [IsDollar], [UpdatedBy], [UpdatedDate]) VALUES (3, N'9911', 1294, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), N'string', N'12738912739081', NULL, N'string', N'string', N'عميل 1', CAST(N'2022-01-31' AS Date), 0, N'admin', CAST(N'2022-03-15' AS Date))
SET IDENTITY_INSERT [dbo].[DisAcceptOrder] OFF
SET IDENTITY_INSERT [dbo].[Discount] ON 

INSERT [dbo].[Discount] ([Id], [Points], [Money], [ClientPaymentId]) VALUES (1, 20, 1000.0000, 1)
SET IDENTITY_INSERT [dbo].[Discount] OFF
SET IDENTITY_INSERT [dbo].[Group] ON 

INSERT [dbo].[Group] ([Id], [Name]) VALUES (1, N'مجموعة المدراء')
INSERT [dbo].[Group] ([Id], [Name]) VALUES (9, N'مجموعة تجريب')
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
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (1, 56)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (9, 1)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (9, 2)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (9, 3)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (9, 4)
INSERT [dbo].[GroupPrivilege] ([GroupId], [PrivilegId]) VALUES (9, 5)
SET IDENTITY_INSERT [dbo].[IncomeType] ON 

INSERT [dbo].[IncomeType] ([Id], [Name]) VALUES (4, N'type 4')
INSERT [dbo].[IncomeType] ([Id], [Name]) VALUES (5, N'typee 3')
INSERT [dbo].[IncomeType] ([Id], [Name]) VALUES (1002, N'Type 3')
SET IDENTITY_INSERT [dbo].[IncomeType] OFF
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
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (142, 1294, N'الطلب 500 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (143, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (144, 1294, N'تم تسديدك برقم 4', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (145, 1294, N'الطلب 500 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (146, 1294, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (147, 1294, N'الطلب 600 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (148, 1294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (149, 1294, N'تم تسديدك برقم 5', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (150, 2294, N'تم تسديدك برقم 6', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (151, 2294, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (152, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (153, 2294, N'تم تسديدك برقم 7', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (154, 2295, N'الطلب 4 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (155, 2295, N'الطلب 3 اصبح مرفوض و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (156, 2295, N'الطلب 2 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (157, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (158, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (159, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (160, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (161, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (162, 2295, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (163, 2295, NULL, 1, 4, 4, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (164, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (165, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (166, 2295, N'الطلب 2 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (167, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (168, 2295, N'الطلب 3 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (169, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (170, 2295, N'الطلب 4 اصبح مرفوض و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (171, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (172, 2295, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (173, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (174, 2295, N'الطلب 6 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (175, 2295, NULL, 1, 4, 4, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (176, 2295, N'الطلب 2 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (177, 2295, N'الطلب 3 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (178, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (179, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (180, 2295, N'الطلب 2 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (181, 2295, N'الطلب 6 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (182, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (183, 2295, N'الطلب 5 اصبح مرفوض و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (184, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (185, 2295, N'الطلب 4 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (186, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (187, 2295, N'الطلب 3 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (188, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (189, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (190, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (191, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (192, 2295, N'الطلب 2 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (193, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (194, 2295, N'الطلب 2 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (195, 2295, N'الطلب 3 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (196, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (197, 2295, N'الطلب 4 اصبح مرفوض و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (198, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (199, 2294, N'الطلب 4 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
GO
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (200, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (201, 2294, N'الطلب 3 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (202, 2294, N'الطلب 2 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (203, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (204, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (205, 2295, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (206, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (207, 2295, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (208, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (209, 2295, N'الطلب 6 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (210, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (211, 2295, N'الطلب 6 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (212, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (213, 2295, N'الطلب 7 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (214, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (215, 2295, N'الطلب 7 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (216, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (217, 2295, N'الطلب 7 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (218, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (219, 2295, N'الطلب 8 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (220, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (221, 2295, N'الطلب 9 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (222, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (223, 2294, N'تم تسديدك برقم 33', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (224, 2294, N'الطلب 100 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (225, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (226, 2294, N'الطلب 101 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (227, 2294, NULL, 1, 1, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (228, 2294, N'تم تسديدك برقم 34', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (229, 2294, N'الطلب 101 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (230, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (231, 2294, N'تم تسديدك برقم 35', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (232, 2294, N'الطلب 200 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (233, 2294, NULL, 1, 1, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (234, 2294, N'تم تسديدك برقم 36', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (235, 2294, N'الطلب 200 اصبح مرتجع جزئي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (236, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (237, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (238, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (239, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (240, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (241, 2295, N'الطلب 2 اصبح مرتجع جزئي و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (242, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (243, 2295, N'الطلب 3 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (244, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (245, 2295, N'الطلب 3 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (246, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (247, 2295, N'الطلب 4 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (248, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (249, 2295, N'الطلب 4 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (250, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (251, 2295, N'الطلب 5 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (252, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (253, 2294, N'تم تسديدك برقم 45', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (254, 2294, N'الطلب 1001 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (255, 2294, NULL, 1, 1, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (256, 2294, N'تم تسديدك برقم 46', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (257, 2294, N'تم تسديدك برقم 47', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (258, 2294, N'الطلب 2001 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (259, 2294, NULL, 1, 1, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (260, 2294, N'الطلب 2000 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (261, 2294, NULL, 1, 1, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (262, 2294, N'تم تسديدك برقم 48', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (263, 2294, N'الطلب 2000 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (264, 2294, NULL, 1, 3, 3, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (265, 2294, N'تم تسديدك برقم 49', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (266, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (267, 2295, NULL, 1, 1, 2, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (268, 2295, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (269, 2295, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (270, 2294, N'الطلب 150 اصبح مرتجع كلي و موقع المبلغ  مندوب', NULL, NULL, NULL, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (271, 2294, NULL, 1, 5, 2, 1)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (272, 2294, N'تم تسديدك برقم 53', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (273, 2294, N'الطلب 250 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (274, 2294, N'تم تسديدك برقم 54', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (275, 2294, N'الطلب 350 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (276, 2294, N'الطلب 351 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (277, 2294, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (278, 2294, N'تم تسديدك برقم 55', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (279, 2294, N'تم تسديدك برقم 56', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (280, 2294, N'الطلب 352 اصبح تم التسليم و موقع المبلغ  خارج الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (281, 2294, N'الطلب 352 اصبح تم التسليم و موقع المبلغ  تم تسليمها', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1270, 2296, N'الطلب 1 اصبح تم التسليم و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1271, 2296, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1272, 2296, N'تم تسديدك برقم 57', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1273, 2296, N'الطلب 2 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1274, 2296, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1275, 2296, N'تم تسديدك برقم 58', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1276, 2296, N'تم تسديدك برقم 59', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1277, 2296, N'تم تسديدك برقم 60', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1278, 2296, N'الطلب 1 اصبح مرتجع كلي و موقع المبلغ  داخل الشركة', NULL, NULL, NULL, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1279, 2296, NULL, 1, 3, 3, 0)
INSERT [dbo].[Notfication] ([Id], [ClientId], [Note], [OrderCount], [OrderPlacedId], [MoneyPlacedId], [IsSeen]) VALUES (1280, 2296, N'تم تسديدك برقم 1', NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Notfication] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13745, N'1', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'15165165165', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, N'UpdateOrdersStatusFromAgent', 5000.0000, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13746, N'2', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'21321321321', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13747, N'3', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'13213213213', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13748, N'4', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13749, N'5', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'23132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13750, N'6', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'21321323213', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13751, N'7', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'21321321321', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13752, N'8', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'01321321321', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13753, N'9', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13754, N'10', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13755, N'11', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32213213213', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13756, N'12', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 95000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13757, N'13', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'32132132132', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
INSERT [dbo].[Order] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [CreatedBy], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [IsDollar], [UpdatedBy], [UpdatedDate], [SystemNote], [OldDeliveryCost], [IsSend], [ClientPaied], [CurrentCountry], [PrintedTimes], [AgentRequestStatus]) VALUES (13758, N'14', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'12321321321', NULL, NULL, NULL, N'admin', 1, 3, CAST(N'2022-04-16T04:35:56.000' AS DateTime), NULL, NULL, 1069, 1, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, 995000.0000, 1, 0, 0)
SET IDENTITY_INSERT [dbo].[Order] OFF
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13745, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13746, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13747, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13748, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13749, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13750, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13751, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13752, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13753, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13754, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13755, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13756, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13757, 1)
INSERT [dbo].[OrderClientPaymnet] ([OrderId], [ClientPaymentId]) VALUES (13758, 1)
SET IDENTITY_INSERT [dbo].[OrderFromExcel] ON 

INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (33, N'222757', N'علي قحطان', N'بابل', CAST(20000.00 AS Decimal(18, 2)), N'بابل الحلة شارع 80 حي عسكري', N'7824200822', N'مشد القدم', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (34, N'222756', N'ام احمد', N'بغداد ', CAST(21000.00 AS Decimal(18, 2)), N'بغداد الشعب', N'7717053095', N'مشد القدم', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (35, N'222755', N'شيرين', N'بغداد ', CAST(21000.00 AS Decimal(18, 2)), N'بغداد الغدير ساحة ميسلون', N'7714219835', N'مشد القدم', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (36, N'222754', N'ابو علي', N'النجف', CAST(40000.00 AS Decimal(18, 2)), N'النجف طريق كربلاء عموري 174', N'7819437294', N'طارد الحشرات -كفوف الجلي', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (37, N'222753', N'علي', N'النجف', CAST(20000.00 AS Decimal(18, 2)), N'النجف طريق النجف وكربلاء عمود 605', N'7500859057', N'طارد الحشرات', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (38, N'222752', N'ابو ملاك', N'بصره', CAST(25000.00 AS Decimal(18, 2)), N'البصرة القرنة قرب كراج الموحد', N'7707023733', N'طارد الحشرات', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (39, N'222751', N'رجل', N'بغداد ', CAST(20000.00 AS Decimal(18, 2)), N'بغداد الزعفرانية', N'7716050228', N'طارد الحشرات', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (40, N'222750', N'محمد فوزي', N'بصره', CAST(87000.00 AS Decimal(18, 2)), N'بصرة قضاء المدينة الشروق', N'7725404210', N'حزام فقررات الظهر', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (41, N'222749', N'معن سالم', N'موصل ', CAST(95000.00 AS Decimal(18, 2)), N'الموصل حي الحدباء', N'7508874439', N'حزام فقرات الظهر - وسادة الرقبة الهوائية', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (42, N'222748', N'رحيم', N'بغداد ', CAST(100000.00 AS Decimal(18, 2)), N'بغداد الكاظمية', N'7710053072', N'حزام فقرات الظهر- كعب زيادة الطول', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (43, N'222747', N'نوال حازم', N'موصل', CAST(87000.00 AS Decimal(18, 2)), N'موصل \ الموصل الجديدة فرع البو سالم', N'7740870584', N'حزام فقرات الظهر', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (44, N'222746', N'محمد علي', N'بغداد ', CAST(25000.00 AS Decimal(18, 2)), N'بغداد الكرخ ابو غريب', N'7816800158', N'حبل رياضة المعدني', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (45, N'222763', N'عمر احمد', N'تكريت', CAST(25000.00 AS Decimal(18, 2)), N'تكريت قضاء العلم', N'7821731110', N'مشد سليم ليفت قياس m', 2294, CAST(N'2022-04-13' AS Date))
INSERT [dbo].[OrderFromExcel] ([Id], [Code], [RecipientName], [Country], [Cost], [Address], [Phone], [Note], [ClientId], [CreateDate]) VALUES (46, N'222764', N'علي', N'المثنى', CAST(47000.00 AS Decimal(18, 2)), N'المثنى السماوة شارع الجسر مقابل مدرسة الرشيد', N'7710033339', N'ميزان الكتروني', 2294, CAST(N'2022-04-13' AS Date))
SET IDENTITY_INSERT [dbo].[OrderFromExcel] OFF
SET IDENTITY_INSERT [dbo].[OrderLog] ON 

INSERT [dbo].[OrderLog] ([Id], [Code], [ClientId], [CountryId], [DeliveryCost], [Cost], [OldCost], [AgentCost], [RecipientName], [RecipientPhones], [RegionId], [Address], [ClientNote], [MoenyPlacedId], [OrderplacedId], [Date], [DiliveryDate], [Note], [AgentId], [seen], [IsClientDiliverdMoney], [IsSync], [OrderStateId], [UpdatedBy], [OrderId], [IsDollar], [UpdatedDate], [SystemNote]) VALUES (10029, N'1', 2296, 34, CAST(5000.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), NULL, CAST(3000.00 AS Decimal(18, 2)), NULL, N'15165165165', NULL, NULL, NULL, 1, 3, NULL, NULL, NULL, 1069, 1, 0, 0, 1, N'admin', 13745, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OrderLog] OFF
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
SET IDENTITY_INSERT [dbo].[OutComeType] ON 

INSERT [dbo].[OutComeType] ([Id], [Name]) VALUES (1, N'نوع 1 شسيش')
INSERT [dbo].[OutComeType] ([Id], [Name]) VALUES (2, N'نوع 2')
SET IDENTITY_INSERT [dbo].[OutComeType] OFF
SET IDENTITY_INSERT [dbo].[PaymentRequest] ON 

INSERT [dbo].[PaymentRequest] ([Id], [ClientId], [PaymentWayId], [Note], [Accept], [CreateDate]) VALUES (5, 1294, 6, N'czxc', NULL, CAST(N'2022-01-31' AS Date))
SET IDENTITY_INSERT [dbo].[PaymentRequest] OFF
SET IDENTITY_INSERT [dbo].[PaymentWay] ON 

INSERT [dbo].[PaymentWay] ([Id], [Name]) VALUES (6, N'PayPal')
SET IDENTITY_INSERT [dbo].[PaymentWay] OFF
SET IDENTITY_INSERT [dbo].[PointsSetting] ON 

INSERT [dbo].[PointsSetting] ([Id], [Points], [Money]) VALUES (1, 20, 1000.0000)
SET IDENTITY_INSERT [dbo].[PointsSetting] OFF
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
INSERT [dbo].[Privilege] ([Id], [Name], [SysName]) VALUES (56, N'تسديد', N'Pay')
SET IDENTITY_INSERT [dbo].[Receipt] ON 

INSERT [dbo].[Receipt] ([Id], [ClientId], [Amount], [Date], [Note], [CreatedBy], [About], [Manager], [IsPay], [ClientPaymentId]) VALUES (1, 2296, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2022-04-21' AS Date), N'ملاحظات', N'admin', N'ذلك عن اي شي ', N'اناا', 0, 1)
SET IDENTITY_INSERT [dbo].[Receipt] OFF
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
INSERT [dbo].[Region] ([Id], [Name], [CountryId]) VALUES (23, N'C1Region2', 1)
SET IDENTITY_INSERT [dbo].[Region] OFF
INSERT [dbo].[UserGroup] ([UserId], [GroupId]) VALUES (65, 1)
INSERT [dbo].[UserGroup] ([UserId], [GroupId]) VALUES (1070, 9)
SET IDENTITY_INSERT [dbo].[UserPhone] ON 

INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (58, 66, N'15161561651')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1058, 1066, N'12312312379')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1059, 1067, N'12937012987')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1060, 1068, N'32493827490')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1061, 1069, N'99999999999')
INSERT [dbo].[UserPhone] ([Id], [UserId], [Phone]) VALUES (1062, 1070, N'23132132132')
SET IDENTITY_INSERT [dbo].[UserPhone] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (65, N'admin', NULL, NULL, CAST(N'2022-01-01' AS Date), NULL, 0, NULL, N'admin', N'21232f297a57a5a743894a0e4a801fc3', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (66, N'مندوب 1', NULL, NULL, CAST(N'2022-01-06' AS Date), N'kjdsd', 1, CAST(0.00 AS Decimal(18, 2)), N'agent1', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1066, N'مندوب ', NULL, NULL, CAST(N'2022-02-10' AS Date), N'asjdfkjfksjf', 1, CAST(100000.00 AS Decimal(18, 2)), N'aa', N'', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1067, N'bb', NULL, NULL, CAST(N'2022-02-10' AS Date), N'jsahdksjafs', 1, CAST(1000.00 AS Decimal(18, 2)), N'bb', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1068, N'zz', NULL, NULL, CAST(N'2022-02-10' AS Date), N'skldjkldasjfklasdjfldaksh', 1, CAST(31313.00 AS Decimal(18, 2)), N'zz', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1069, N'مندوب نقل البيانات', NULL, NULL, CAST(N'2022-04-01' AS Date), N'يصبمتاشيسبشس', 1, CAST(3000.00 AS Decimal(18, 2)), N'agnetTransfer', N'202cb962ac59075b964b07152d234b70', 1)
INSERT [dbo].[Users] ([Id], [Name], [Experince], [Adress], [HireDate], [Note], [CanWorkAsAgent], [Salary], [UserName], [Password], [IsActive]) VALUES (1070, N'موظف تجريب الصندوق', N'sdfsa', NULL, CAST(N'2022-05-02' AS Date), N'sdfsad', 0, NULL, N'user1', N'202cb962ac59075b964b07152d234b70', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [UQ__PointsSe__DA826786C9B4659A]    Script Date: 5/3/2022 7:08:35 AM ******/
ALTER TABLE [dbo].[PointsSetting] ADD UNIQUE NONCLUSTERED 
(
	[Points] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ__PointsSe__FA951B46C519FCD7]    Script Date: 5/3/2022 7:08:35 AM ******/
ALTER TABLE [dbo].[PointsSetting] ADD UNIQUE NONCLUSTERED 
(
	[Money] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [dbo].[AgentOrderPrint]  WITH CHECK ADD FOREIGN KEY([AgentPrintId])
REFERENCES [dbo].[AgentPrint] ([Id])
GO
ALTER TABLE [dbo].[AgentOrderPrint]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[AgentPrintDetails]  WITH CHECK ADD FOREIGN KEY([AgentPrintId])
REFERENCES [dbo].[AgentPrint] ([Id])
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
ALTER TABLE [dbo].[ClientPaymentDetails]  WITH CHECK ADD FOREIGN KEY([ClientPaymentId])
REFERENCES [dbo].[ClientPayment] ([Id])
GO
ALTER TABLE [dbo].[ClientPaymentDetails]  WITH CHECK ADD FOREIGN KEY([MoneyPlacedId])
REFERENCES [dbo].[MoenyPlaced] ([Id])
GO
ALTER TABLE [dbo].[ClientPaymentDetails]  WITH CHECK ADD FOREIGN KEY([OrderPlacedId])
REFERENCES [dbo].[OrderPlaced] ([Id])
GO
ALTER TABLE [dbo].[clientPhones]  WITH CHECK ADD  CONSTRAINT [FK_clientPhones_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[clientPhones] CHECK CONSTRAINT [FK_clientPhones_Clients]
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
ALTER TABLE [dbo].[Discount]  WITH CHECK ADD FOREIGN KEY([ClientPaymentId])
REFERENCES [dbo].[ClientPayment] ([Id])
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
ALTER TABLE [dbo].[OrderClientPaymnet]  WITH CHECK ADD FOREIGN KEY([ClientPaymentId])
REFERENCES [dbo].[ClientPayment] ([Id])
GO
ALTER TABLE [dbo].[OrderClientPaymnet]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderFromExcel]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
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
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD FOREIGN KEY([ClientPaymentId])
REFERENCES [dbo].[ClientPayment] ([Id])
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Clients]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Reginos_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Reginos_Country]
GO
ALTER TABLE [dbo].[Treasury]  WITH CHECK ADD FOREIGN KEY([Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TreasuryHistory]  WITH CHECK ADD FOREIGN KEY([CashMovmentId])
REFERENCES [dbo].[CashMovment] ([Id])
GO
ALTER TABLE [dbo].[TreasuryHistory]  WITH CHECK ADD FOREIGN KEY([ClientPaymentId])
REFERENCES [dbo].[ClientPayment] ([Id])
GO
ALTER TABLE [dbo].[TreasuryHistory]  WITH CHECK ADD FOREIGN KEY([TreasuryId])
REFERENCES [dbo].[Treasury] ([Id])
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
/****** Object:  StoredProcedure [dbo].[OrderWithClientPrint]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_changeAgnetPrintTables]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc  [dbo].[sp_changeAgnetPrintTables]  
as
declare @Id int; 
declare @printNumber int;
declare @PrinterName nvarchar(50);
declare @Date Date; 
declare @DesName nvarchar(50); 
declare @DesPhone nvarchar(11);
declare @code varchar(15);
declare @total  decimal(18,2);
declare @country nvarchar(50);
declare @phone varchar(11);
declare @clientName nvarchar(50);
declare @note nvarchar(max);
declare @region nvarchar(50); 
begin 
Set IDENTITY_INSERT dbo.AgentPrint ON			
declare printedC cursor  for 
Select Id,PrintNmber,PrinterName,[Date],DestinationName,DestinationPhone from Printed where [Type]='Agent' ;
open printedC 
FETCH NEXT FROM printedC INTO @Id,@printNumber,@PrinterName,@Date,@DesName,@DesPhone
while @@FETCH_STATUS =0 
begin

insert into AgentPrint (Id,PrinterName,[Date],DestinationName,DestinationPhone) values (@printNumber,@PrinterName,@Date,@DesName,@DesPhone);
declare  agentPrintCursor cursor  for select Code,Total,Country,Phone,ClientName,Note,Region  from AgnetPrint where PrintId = @Id order by Id;
open agentPrintCursor;
Fetch next from  agentPrintCursor into @code,@total,@country,@phone,@clientName,@note,@region 
while @@FETCH_STATUS =0 
begin
insert into AgentPrintDetails(Code,Total,Country,Phone,ClientName,Note,Region,AgentPrintId)
values (@code,@total,@country,@phone,@clientName,@note,@region,@printNumber);
Fetch next from  agentPrintCursor into @code,@total,@country,@phone,@clientName,@note,@region;
end 
close  agentPrintCursor;
DEALLOCATE  agentPrintCursor;
insert into AgentOrderPrint (OrderId,AgentPrintId) select  OrderId,@printNumber from OrderPrint where PrintId = @Id;
select OrderId from OrderPrint where PrintId = @Id;
FETCH NEXT FROM printedC INTO @Id,@printNumber,@PrinterName,@Date,@DesName,@DesPhone;
end 
close printedC ;
DEALLOCATE printedC; 
Set IDENTITY_INSERT dbo.AgentPrint OFF
end
GO
/****** Object:  StoredProcedure [dbo].[sp_empty]    Script Date: 5/3/2022 7:08:35 AM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_innerProc]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_innerProc]
as 
begin 
begin transaction ; 
insert into Country ([Name],DeliveryCost,mediatorId,IsMain,Points)
values ('aaaa',1000,null,0,10);
insert into AgentCountr(AgentId,CountryId) values (1068,9);
insert into Country ([Name],DeliveryCost,mediatorId,IsMain,Points)
values ('cccc',1000,null,0,10);
ROLLBACK;
end ;
GO
/****** Object:  StoredProcedure [dbo].[sp_outerProc]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[sp_outerProc] 
as 
begin 
begin transaction ;  
insert into [Group]([Name])values('zzzz');
exec sp_innerProc ;
ROLLBACK;
end ;
GO
/****** Object:  StoredProcedure [dbo].[sp_TransferClientPrintToClientPaymet]    Script Date: 5/3/2022 7:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_TransferClientPrintToClientPaymet]
as
declare @Id int; 
declare @printNumber int;
declare @PrinterName nvarchar(50);
declare @Date Date; 
declare @DesName nvarchar(50); 
declare @DesPhone nvarchar(11);
declare @code varchar(15);
declare @lastToatal decimal;
declare @Total decimal ; 
declare  @DeliveCost decimal ;
declare @Country nvarchar ;
declare @Phone nvarchar 
declare @MoneyPlacedId int;
declare @OrderPlacedId int;
declare @date2 Date ;
declare @Note nvarchar;
declare @PayForClient  money;
begin

Set Identity_insert dbo.ClientPayment on 
declare printedC cursor  for 
Select Id,PrintNmber,PrinterName,[Date],DestinationName,DestinationPhone from Printed where [Type]='Client' ;
open printedC 
FETCH NEXT from printedC INTO @Id,@printNumber,@PrinterName,@Date,@DesName,@DesPhone;
While @@FETCH_STATUS =0 
begin
insert into ClientPayment (Id,PrinterName,[Date],DestinationName,DestinationPhone) values (@printNumber,@PrinterName,@Date,@DesName,@DesPhone);
declare clientPaymentDetialsCursor cursor for Select Code,LastTotal,Total,DeliveCost,Country,Phone,MoneyPlacedId,OrderPlacedId,[Date],Note,PayForClient from ClientPrint where PrintId=@Id order by Id;
open clientPaymentDetialsCursor;
Fetch next from clientPaymentDetialsCursor into @code,@lastToatal,@Total,@DeliveCost,@Country,@Phone,@MoneyPlacedId,@OrderPlacedId,@date2,@Note,@PayForClient;
	while @@FETCH_STATUS =0
	begin
	insert into ClientPaymentDetails(ClientPaymentId,Code,LastTotal,Total,DeliveryCost,Country,Phone,MoneyPlacedId,OrderPlacedId,[Date],Note,PayForClient) values (@printNumber,@code,@lastToatal,@Total,@DeliveCost,@Country,@Phone,@MoneyPlacedId,@OrderPlacedId,@date2,@Note,@PayForClient);
	Fetch next from clientPaymentDetialsCursor into @code,@lastToatal,@Total,@DeliveCost,@Country,@Phone,@MoneyPlacedId,@OrderPlacedId,@date2,@Note,@PayForClient;
	end;
	close clientPaymentDetialsCursor;
	deallocate clientPaymentDetialsCursor;
	insert into OrderClientPaymnet (OrderId,ClientPaymentId) select OrderId,@printNumber from OrderPrint where PrintId = @Id;
	Update Discount 
	set ClientPaymentId = @printNumber 
	where PrintedId = @Id;
	update Receipt 
	set 
	ClientPaymentId = @printNumber
	where PrintId = @Id;

	FETCH NEXT from printedC INTO @Id,@printNumber,@PrinterName,@Date,@DesName,@DesPhone;
end 
close printedC
deallocate printedC;

Set Identity_insert dbo.ClientPayment on 
end
GO
USE [master]
GO
ALTER DATABASE [Kokaz] SET  READ_WRITE 
GO
