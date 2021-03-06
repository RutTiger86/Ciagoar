USE [master]
GO
/****** Object:  Database [Ciagoar]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  User [ciagoarmaster]    Script Date: 2022-04-17 오후 1:29:58 ******/
CREATE USER [ciagoarmaster] FOR LOGIN [ciagoarmaster] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ciagoarmaster]
GO
/****** Object:  Table [dbo].[auth_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[delivery_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[delivery_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[relative_co_id] [int] NOT NULL,
	[apply_user_id] [int] NOT NULL,
	[order_date] [date] NOT NULL,
	[memo] [nvarchar](400) NULL,
	[oder_step] [smallint] NOT NULL,
	[mgr_user_id] [int] NULL,
	[delivery_datetime] [datetime] NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_DELIVERY_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[delivery_inventory]    Script Date: 2022-04-17 오후 1:29:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[delivery_inventory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[delivery_item_id] [int] NOT NULL,
	[inveontory_storage_id] [int] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
 CONSTRAINT [PK_DELIVERY_INVENTORY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[delivery_item]    Script Date: 2022-04-17 오후 1:29:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[delivery_item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[delivery_info_id] [int] NOT NULL,
	[item_info_id] [int] NOT NULL,
	[order_price] [nvarchar](150) NOT NULL,
	[price_unit_type] [int] NOT NULL,
	[order_qty] [date] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_DELIVERY_ITEM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[inventory_storage]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[item_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
	[base_price] [int] NULL,
	[price_unit_type] [int] NULL,
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
/****** Object:  Table [dbo].[order_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[order_item]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[relative_co]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[relative_co_staff]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[return_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[return_info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[apply_user_id] [int] NOT NULL,
	[order_date] [date] NOT NULL,
	[delivery_inventory_id] [int] NOT NULL,
	[retrun_type] [smallint] NOT NULL,
	[memo] [nvarchar](400) NULL,
	[return_step] [smallint] NOT NULL,
	[mgr_user_id] [int] NULL,
	[return_datetime] [datetime] NULL,
	[isuse] [bit] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[createtime] [datetime] NOT NULL,
	[updatetime] [datetime] NULL,
 CONSTRAINT [PK_RETURN_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[storage_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[user_auth]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
/****** Object:  Table [dbo].[user_info]    Script Date: 2022-04-17 오후 1:29:58 ******/
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
ALTER TABLE [dbo].[delivery_info] ADD  CONSTRAINT [DF__delivery___oder___27C3E46E]  DEFAULT ((0)) FOR [oder_step]
GO
ALTER TABLE [dbo].[delivery_info] ADD  CONSTRAINT [DF__delivery___isuse__28B808A7]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[delivery_info] ADD  CONSTRAINT [DF__delivery___isdel__29AC2CE0]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[delivery_info] ADD  CONSTRAINT [DF__delivery___creat__2AA05119]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[delivery_inventory] ADD  CONSTRAINT [DF__delivery___isdel__2F650636]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[delivery_inventory] ADD  CONSTRAINT [DF__delivery___creat__30592A6F]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[delivery_item] ADD  CONSTRAINT [DF__delivery___isdel__2C88998B]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[delivery_item] ADD  CONSTRAINT [DF__delivery___creat__2D7CBDC4]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__isuse__14B10FFA]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__isdel__15A53433]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[inventory_info] ADD  CONSTRAINT [DF__inventory__creat__1699586C]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__isuse__1C5231C2]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__isdel__1D4655FB]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[inventory_storage] ADD  CONSTRAINT [DF__inventory__creat__1E3A7A34]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__isuse__038683F8]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__isdel__047AA831]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[item_info] ADD  CONSTRAINT [DF__item_info__creat__056ECC6A]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__oder___2022C2A6]  DEFAULT ((0)) FOR [oder_step]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__isuse__2116E6DF]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__isdel__220B0B18]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[order_info] ADD  CONSTRAINT [DF__order_inf__creat__22FF2F51]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[order_item] ADD  CONSTRAINT [DF__order_ite__isdel__24E777C3]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[order_item] ADD  CONSTRAINT [DF__order_ite__creat__25DB9BFC]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___isuse__0D0FEE32]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___isdel__0E04126B]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[relative_co] ADD  CONSTRAINT [DF__relative___creat__0EF836A4]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___isuse__10E07F16]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___isdel__11D4A34F]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[relative_co_staff] ADD  CONSTRAINT [DF__relative___creat__12C8C788]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[return_info] ADD  CONSTRAINT [DF__return_in__retru__324172E1]  DEFAULT ((0)) FOR [retrun_type]
GO
ALTER TABLE [dbo].[return_info] ADD  CONSTRAINT [DF__return_in__retur__3335971A]  DEFAULT ((0)) FOR [return_step]
GO
ALTER TABLE [dbo].[return_info] ADD  CONSTRAINT [DF__return_in__isuse__3429BB53]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[return_info] ADD  CONSTRAINT [DF__return_in__isdel__351DDF8C]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[return_info] ADD  CONSTRAINT [DF__return_in__creat__361203C5]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__isuse__1881A0DE]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__isdel__1975C517]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[storage_info] ADD  CONSTRAINT [DF__storage_i__creat__1A69E950]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__type___075714DC]  DEFAULT ((0)) FOR [type_code]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__auth___084B3915]  DEFAULT ((0)) FOR [auth_step]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__isuse__093F5D4E]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__isdel__0A338187]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[user_auth] ADD  CONSTRAINT [DF__user_auth__creat__0B27A5C0]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__type___7EC1CEDB]  DEFAULT ((0)) FOR [type_code]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__isuse__7FB5F314]  DEFAULT ((1)) FOR [isuse]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__isdel__00AA174D]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[user_info] ADD  CONSTRAINT [DF__user_info__creat__019E3B86]  DEFAULT (getdate()) FOR [createtime]
GO
ALTER TABLE [dbo].[delivery_info]  WITH CHECK ADD  CONSTRAINT [FK_relative_co_TO_delivery_info_1] FOREIGN KEY([relative_co_id])
REFERENCES [dbo].[relative_co] ([ID])
GO
ALTER TABLE [dbo].[delivery_info] CHECK CONSTRAINT [FK_relative_co_TO_delivery_info_1]
GO
ALTER TABLE [dbo].[delivery_info]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_delivery_info_1] FOREIGN KEY([apply_user_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[delivery_info] CHECK CONSTRAINT [FK_user_info_TO_delivery_info_1]
GO
ALTER TABLE [dbo].[delivery_info]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_delivery_info_2] FOREIGN KEY([mgr_user_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[delivery_info] CHECK CONSTRAINT [FK_user_info_TO_delivery_info_2]
GO
ALTER TABLE [dbo].[delivery_inventory]  WITH CHECK ADD  CONSTRAINT [FK_delivery_item_TO_delivery_inventory_1] FOREIGN KEY([delivery_item_id])
REFERENCES [dbo].[delivery_item] ([ID])
GO
ALTER TABLE [dbo].[delivery_inventory] CHECK CONSTRAINT [FK_delivery_item_TO_delivery_inventory_1]
GO
ALTER TABLE [dbo].[delivery_inventory]  WITH CHECK ADD  CONSTRAINT [FK_inventory_storage_TO_delivery_inventory_1] FOREIGN KEY([inveontory_storage_id])
REFERENCES [dbo].[inventory_storage] ([ID])
GO
ALTER TABLE [dbo].[delivery_inventory] CHECK CONSTRAINT [FK_inventory_storage_TO_delivery_inventory_1]
GO
ALTER TABLE [dbo].[delivery_item]  WITH CHECK ADD  CONSTRAINT [FK_delivery_info_TO_delivery_item_1] FOREIGN KEY([delivery_info_id])
REFERENCES [dbo].[delivery_info] ([ID])
GO
ALTER TABLE [dbo].[delivery_item] CHECK CONSTRAINT [FK_delivery_info_TO_delivery_item_1]
GO
ALTER TABLE [dbo].[delivery_item]  WITH CHECK ADD  CONSTRAINT [FK_item_info_TO_delivery_item_1] FOREIGN KEY([item_info_id])
REFERENCES [dbo].[item_info] ([ID])
GO
ALTER TABLE [dbo].[delivery_item] CHECK CONSTRAINT [FK_item_info_TO_delivery_item_1]
GO
ALTER TABLE [dbo].[inventory_info]  WITH CHECK ADD  CONSTRAINT [FK_item_info_TO_inventory_info_1] FOREIGN KEY([item_info_id])
REFERENCES [dbo].[item_info] ([ID])
GO
ALTER TABLE [dbo].[inventory_info] CHECK CONSTRAINT [FK_item_info_TO_inventory_info_1]
GO
ALTER TABLE [dbo].[inventory_info]  WITH CHECK ADD  CONSTRAINT [FK_order_info_TO_inventory_info_1] FOREIGN KEY([order_info_id])
REFERENCES [dbo].[order_info] ([ID])
GO
ALTER TABLE [dbo].[inventory_info] CHECK CONSTRAINT [FK_order_info_TO_inventory_info_1]
GO
ALTER TABLE [dbo].[inventory_storage]  WITH CHECK ADD  CONSTRAINT [FK_inventory_info_TO_inventory_storage_1] FOREIGN KEY([inventory_info_id])
REFERENCES [dbo].[inventory_info] ([ID])
GO
ALTER TABLE [dbo].[inventory_storage] CHECK CONSTRAINT [FK_inventory_info_TO_inventory_storage_1]
GO
ALTER TABLE [dbo].[inventory_storage]  WITH CHECK ADD  CONSTRAINT [FK_storage_info_TO_inventory_storage_1] FOREIGN KEY([storage_info_id])
REFERENCES [dbo].[storage_info] ([ID])
GO
ALTER TABLE [dbo].[inventory_storage] CHECK CONSTRAINT [FK_storage_info_TO_inventory_storage_1]
GO
ALTER TABLE [dbo].[item_info]  WITH CHECK ADD  CONSTRAINT [FK_relative_co_TO_item_info_1] FOREIGN KEY([relative_co_id])
REFERENCES [dbo].[relative_co] ([ID])
GO
ALTER TABLE [dbo].[item_info] CHECK CONSTRAINT [FK_relative_co_TO_item_info_1]
GO
ALTER TABLE [dbo].[order_info]  WITH CHECK ADD  CONSTRAINT [FK_relative_co_TO_order_info_1] FOREIGN KEY([relative_co_id])
REFERENCES [dbo].[relative_co] ([ID])
GO
ALTER TABLE [dbo].[order_info] CHECK CONSTRAINT [FK_relative_co_TO_order_info_1]
GO
ALTER TABLE [dbo].[order_info]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_order_info_1] FOREIGN KEY([user_info_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[order_info] CHECK CONSTRAINT [FK_user_info_TO_order_info_1]
GO
ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FK_item_info_TO_order_item_1] FOREIGN KEY([item_info_id])
REFERENCES [dbo].[item_info] ([ID])
GO
ALTER TABLE [dbo].[order_item] CHECK CONSTRAINT [FK_item_info_TO_order_item_1]
GO
ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FK_order_info_TO_order_item_1] FOREIGN KEY([order_info_ID])
REFERENCES [dbo].[order_info] ([ID])
GO
ALTER TABLE [dbo].[order_item] CHECK CONSTRAINT [FK_order_info_TO_order_item_1]
GO
ALTER TABLE [dbo].[relative_co_staff]  WITH CHECK ADD  CONSTRAINT [FK_relative_co_TO_relative_co_staff_1] FOREIGN KEY([relative_co_id])
REFERENCES [dbo].[relative_co] ([ID])
GO
ALTER TABLE [dbo].[relative_co_staff] CHECK CONSTRAINT [FK_relative_co_TO_relative_co_staff_1]
GO
ALTER TABLE [dbo].[return_info]  WITH CHECK ADD  CONSTRAINT [FK_delivery_inventory_TO_return_info_1] FOREIGN KEY([delivery_inventory_id])
REFERENCES [dbo].[delivery_inventory] ([ID])
GO
ALTER TABLE [dbo].[return_info] CHECK CONSTRAINT [FK_delivery_inventory_TO_return_info_1]
GO
ALTER TABLE [dbo].[return_info]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_return_info_1] FOREIGN KEY([apply_user_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[return_info] CHECK CONSTRAINT [FK_user_info_TO_return_info_1]
GO
ALTER TABLE [dbo].[return_info]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_return_info_2] FOREIGN KEY([mgr_user_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[return_info] CHECK CONSTRAINT [FK_user_info_TO_return_info_2]
GO
ALTER TABLE [dbo].[user_auth]  WITH CHECK ADD  CONSTRAINT [FK_user_info_TO_user_auth_1] FOREIGN KEY([user_info_id])
REFERENCES [dbo].[user_info] ([ID])
GO
ALTER TABLE [dbo].[user_auth] CHECK CONSTRAINT [FK_user_info_TO_user_auth_1]
GO
USE [master]
GO
ALTER DATABASE [Ciagoar] SET  READ_WRITE 
GO
