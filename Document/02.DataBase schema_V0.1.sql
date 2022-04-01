USE [master]
GO
/****** Object:  Database [Ciagoar]    Script Date: 2022-04-01 오후 9:20:40 ******/
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
/****** Object:  User [ciagoarmaster]    Script Date: 2022-04-01 오후 9:20:40 ******/
CREATE USER [ciagoarmaster] FOR LOGIN [ciagoarmaster] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [ciagoarmaster]
GO
/****** Object:  Table [dbo].[OAUTH_INFO]    Script Date: 2022-04-01 오후 9:20:40 ******/
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
/****** Object:  Table [dbo].[USER_AUTHENTICATION]    Script Date: 2022-04-01 오후 9:20:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_AUTHENTICATION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserInfoID] [int] NOT NULL,
	[AuthenticationType] [smallint] NOT NULL,
	[AuthenticationKey] [nvarchar](512) NOT NULL,
	[AuthenticationStep] [smallint] NOT NULL,
	[IsUse] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_USER_AUTHENTICATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USER_INFO]    Script Date: 2022-04-01 오후 9:20:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USER_INFO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[AuthType] [smallint] NOT NULL,
	[Nickname] [nvarchar](100) NOT NULL,
	[IsUse] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
	[UpdateUserID] [int] NULL,
 CONSTRAINT [PK_USER_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OAUTH_INFO] ADD  CONSTRAINT [DF_OAUTH_INFO_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[USER_AUTHENTICATION] ADD  CONSTRAINT [DF_USER_AUTHENTICATION_AuthenticationType]  DEFAULT ((0)) FOR [AuthenticationType]
GO
ALTER TABLE [dbo].[USER_AUTHENTICATION] ADD  CONSTRAINT [DF_USER_AUTHENTICATION_AuthenticationStep]  DEFAULT ((0)) FOR [AuthenticationStep]
GO
ALTER TABLE [dbo].[USER_AUTHENTICATION] ADD  CONSTRAINT [DF_USER_AUTHENTICATION_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[USER_INFO] ADD  CONSTRAINT [DF_USER_INFO_UserType]  DEFAULT ((0)) FOR [AuthType]
GO
ALTER TABLE [dbo].[USER_INFO] ADD  CONSTRAINT [DF_USER_INFO_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[USER_AUTHENTICATION]  WITH CHECK ADD  CONSTRAINT [FK_USER_AUTHENTICATION_USER_INFO] FOREIGN KEY([UserInfoID])
REFERENCES [dbo].[USER_INFO] ([ID])
GO
ALTER TABLE [dbo].[USER_AUTHENTICATION] CHECK CONSTRAINT [FK_USER_AUTHENTICATION_USER_INFO]
GO
USE [master]
GO
ALTER DATABASE [Ciagoar] SET  READ_WRITE 
GO
