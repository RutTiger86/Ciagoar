USE [master]
GO
/****** Object:  Database [Ciagoar]    Script Date: 2022-04-16 오후 2:08:56 ******/
CREATE DATABASE [Ciagoar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Ciagoar', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Ciagoar.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Ciagoar_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Ciagoar_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Ciagoar] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Ciagoar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Ciagoar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Ciagoar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Ciagoar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Ciagoar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Ciagoar] SET ARITHABORT OFF 
GO
ALTER DATABASE [Ciagoar] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Ciagoar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Ciagoar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Ciagoar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Ciagoar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Ciagoar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Ciagoar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Ciagoar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Ciagoar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Ciagoar] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Ciagoar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Ciagoar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Ciagoar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Ciagoar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Ciagoar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Ciagoar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Ciagoar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Ciagoar] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Ciagoar] SET  MULTI_USER 
GO
ALTER DATABASE [Ciagoar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Ciagoar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Ciagoar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Ciagoar] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Ciagoar] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Ciagoar] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Ciagoar] SET QUERY_STORE = OFF
GO
USE [Ciagoar]
GO
/****** Object:  User [ciagoarmaster]    Script Date: 2022-04-16 오후 2:08:56 ******/
CREATE USER [ciagoarmaster] FOR LOGIN [ciagoarmaster] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ciagoarmaster]
GO
/****** Object:  Table [dbo].[auth_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auth_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type_code] [smallint] NOT NULL,
	[field_name] [nvarchar](max) NOT NULL,
	[field_value] [nvarchar](max) NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_AUTH_INFO] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[item_info_id] [int] NOT NULL,
	[order_info_id] [int] NOT NULL,
	[receipt_date] [date] NOT NULL,
	[mfg_date] [date] NULL,
	[expiry_date] [date] NOT NULL,
	[memo] [nvarchar](400) NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_INVENTORY_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_storage]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_storage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[inventory_info_id] [int] NOT NULL,
	[storage_info_id] [int] NOT NULL,
	[serial_no] [nvarchar](150) NOT NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_INVENTORY_STORAGE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[item_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[item_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[item_name] [nvarchar](150) NOT NULL,
	[item_name_eng] [nvarchar](150) NULL,
	[item_code] [nvarchar](150) NOT NULL,
	[relative_co_id] [int] NOT NULL,
	[unit] [nvarchar](10) NOT NULL,
	[model_code] [nvarchar](100) NOT NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_ITEM_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OAUTH_INFO]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OAUTH_INFO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AuthenticationType] [smallint] NOT NULL,
	[ClientID] [nvarchar](max) NOT NULL,
	[ClientSecret] [nvarchar](max) NOT NULL,
	[AuthURI] [nvarchar](150) NOT NULL,
	[TokenURI] [nvarchar](150) NOT NULL,
	[IsUse] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_OAUTH_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[relative_co_id] [int] NOT NULL,
	[user_info_id] [int] NOT NULL,
	[order_date] [date] NOT NULL,
	[memo] [nvarchar](400) NULL,
	[oder_step] [smallint] NOT NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_ORDER_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_item]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[order_info_ID] [int] NOT NULL,
	[item_info_id] [int] NOT NULL,
	[order_price] [nvarchar](150) NOT NULL,
	[price_unit_type] [int] NOT NULL,
	[order_qty] [date] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
 CONSTRAINT [PK_ORDER_ITEM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[relative_co]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[relative_co](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[co_name] [nvarchar](150) NOT NULL,
	[co_address] [nvarchar](150) NOT NULL,
	[phone_number] [nvarchar](150) NOT NULL,
	[connect_url] [nvarchar](10) NULL,
	[memo] [nvarchar](400) NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_RELATIVE_CO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[relative_co_staff]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[relative_co_staff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[relative_co_id] [int] NOT NULL,
	[staff_name] [nvarchar](150) NOT NULL,
	[phone_number] [nvarchar](150) NOT NULL,
	[staff_rank] [nvarchar](10) NULL,
	[memo] [nvarchar](400) NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_RELATIVE_CO_STAFF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[storage_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[storage_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[storage_name] [nvarchar](150) NOT NULL,
	[storage_code] [nvarchar](150) NULL,
	[storage_location] [nvarchar](150) NOT NULL,
	[memo] [nvarchar](400) NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NOT NULL,
 CONSTRAINT [PK_STORAGE_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_auth]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_auth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[user_info_id] [int] NOT NULL,
	[type_code] [smallint] NOT NULL,
	[auth_key] [nvarchar](512) NOT NULL,
	[auth_step] [smallint] NOT NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_USER_AUTH] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_info]    Script Date: 2022-04-16 오후 2:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[type_code] [smallint] NOT NULL,
	[nickname] [nvarchar](100) NOT NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_USER_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[auth_info] ADD  CONSTRAINT [DF__auth_info__IsDel__123EB7A3]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[auth_info] ADD  CONSTRAINT [DF__auth_info__Creat__1332DBDC]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__isuse__2B0A656D]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__isdel__2BFE89A6]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__creat__2CF2ADDF]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__isuse__32AB8735]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__isdel__339FAB6E]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__creat__3493CFA7]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__isuse__19DFD96B]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__isdel__1AD3FDA4]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__creat__1BC821DD]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[OAUTH_INFO] ADD  CONSTRAINT [DF_OAUTH_INFO_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__oder___367C1819]  DEFAULT ((0)) FOR [oder_step]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__isuse__37703C52]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__isdel__3864608B]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__creat__395884C4]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[order_item] ADD  CONSTRAINT [DF__order_ite__isdel__3B40CD36]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[order_item] ADD  CONSTRAINT [DF__order_ite__creat__3C34F16F]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___isuse__236943A5]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___isdel__245D67DE]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___creat__25518C17]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___isuse__2739D489]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___isdel__282DF8C2]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___creat__29221CFB]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__isuse__2EDAF651]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__isdel__2FCF1A8A]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__creat__30C33EC3]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__type___1DB06A4F]  DEFAULT ((0)) FOR [type_code]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__auth___1EA48E88]  DEFAULT ((0)) FOR [auth_step]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__isuse__1F98B2C1]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__isdel__208CD6FA]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__creat__2180FB33]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__type___151B244E]  DEFAULT ((0)) FOR [type_code]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__isuse__160F4887]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__isdel__17036CC0]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__creat__17F790F9]  DEFAULT (getdate()) FOR [createtime]
GO
USE [master]
GO
ALTER DATABASE [Ciagoar] SET  READ_WRITE 
GO
