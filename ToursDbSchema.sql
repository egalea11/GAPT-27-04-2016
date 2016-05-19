USE [master]
GO

/****** Object:  Database [ToursDb]    Script Date: 19/05/2016 14:39:02 ******/
CREATE DATABASE [ToursDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToursDb', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\ToursDb.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ToursDb_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\ToursDb_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [ToursDb] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToursDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ToursDb] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ToursDb] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ToursDb] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ToursDb] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ToursDb] SET ARITHABORT OFF 
GO

ALTER DATABASE [ToursDb] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ToursDb] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [ToursDb] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ToursDb] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ToursDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ToursDb] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ToursDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ToursDb] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ToursDb] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ToursDb] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ToursDb] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ToursDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ToursDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ToursDb] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ToursDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ToursDb] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ToursDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ToursDb] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ToursDb] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ToursDb] SET  MULTI_USER 
GO

ALTER DATABASE [ToursDb] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ToursDb] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ToursDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [ToursDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [ToursDb] SET  READ_WRITE 
GO


USE [ToursDb]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Surname] [nvarchar](max) NULL,
	[IsAdmin] [bit] NOT NULL,
	[Country] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AttractionType]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttractionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL DEFAULT (NULL),
 CONSTRAINT [PK_AttractionType] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Image]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[Link] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Island]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Island](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Island] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TownId] [int] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationAttraction]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationAttraction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NOT NULL,
	[AttractionTypeId] [int] NOT NULL,
 CONSTRAINT [PK_LocationAttraction] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TourDateTimeId] [int] NOT NULL,
	[AdultQuantity] [int] NOT NULL,
	[ChildQuantity] [int] NOT NULL,
	[TotalPrice] [decimal](10, 2) NULL,
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rating]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumOfStars] [int] NOT NULL,
	[Description] [nvarchar](50) NULL DEFAULT (NULL),
 CONSTRAINT [PK_Rating] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Review]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TourId] [int] NOT NULL,
	[RatingId] [int] NULL DEFAULT (NULL),
	[Comment] [nvarchar](max) NULL DEFAULT (NULL),
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TempOrder]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TourDateTimeId] [int] NOT NULL,
	[AdultQuantity] [int] NOT NULL,
	[ChildQuantity] [int] NOT NULL,
	[TotalPrice] [decimal](10, 2) NULL,
	[DateTimeCreated] [datetime] NOT NULL,
	[Expired] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_TempOrder] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tour]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tour](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortDescription] [nvarchar](500) NULL DEFAULT (NULL),
	[LongDescription] [nvarchar](4000) NULL DEFAULT (NULL),
	[AdultPrice] [decimal](10, 2) NULL,
	[ChildPrice] [decimal](10, 2) NULL,
	[MaxGroupSize] [int] NOT NULL,
	[AverageRatingId] [int] NOT NULL DEFAULT (NULL),
	[CategoryId] [int] NOT NULL,
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Tour] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TourAttendees]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourAttendees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[IsAdult] [bit] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](7) NULL DEFAULT (NULL),
	[DateOfBirth] [datetime] NULL DEFAULT (NULL),
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_TourAttendees] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TourDate]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[DateOfTour] [date] NOT NULL,
 CONSTRAINT [PK_TourDate] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TourDateTime]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDateTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TourDateId] [int] NOT NULL,
	[TourTimeId] [int] NOT NULL,
 CONSTRAINT [PK_TourDateTime] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TourTime]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourTime](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[StartTime] [nvarchar](6) NOT NULL,
	[EndTime] [nvarchar](6) NOT NULL,
 CONSTRAINT [PK_TourTime] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TourTimeTable]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourTimeTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NOT NULL,
	[StartTime] [nvarchar](6) NOT NULL,
	[EndTime] [nvarchar](6) NOT NULL,
	[TourTimeId] [int] NOT NULL,
 CONSTRAINT [PK_TourTimeTable] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Town]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Town](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IslandId] [int] NOT NULL,
 CONSTRAINT [PK_Town] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WishList]    Script Date: 19/05/2016 14:53:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WishList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TourId] [int] NOT NULL,
	[Expired] [bit] NOT NULL DEFAULT ((0)),
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_WishList] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([Id])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_TourId]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_TownId] FOREIGN KEY([TownId])
REFERENCES [dbo].[Town] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_TownId]
GO
ALTER TABLE [dbo].[LocationAttraction]  WITH CHECK ADD  CONSTRAINT [FK_LocationAttraction_AttractionTypeId] FOREIGN KEY([AttractionTypeId])
REFERENCES [dbo].[AttractionType] ([Id])
GO
ALTER TABLE [dbo].[LocationAttraction] CHECK CONSTRAINT [FK_LocationAttraction_AttractionTypeId]
GO
ALTER TABLE [dbo].[LocationAttraction]  WITH CHECK ADD  CONSTRAINT [FK_LocationAttraction_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[LocationAttraction] CHECK CONSTRAINT [FK_LocationAttraction_LocationId]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_TourDateTimeId] FOREIGN KEY([TourDateTimeId])
REFERENCES [dbo].[TourDateTime] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_TourDateTimeId]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_RatingId] FOREIGN KEY([RatingId])
REFERENCES [dbo].[Rating] ([Id])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_RatingId]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([Id])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_TourId]
GO
ALTER TABLE [dbo].[TempOrder]  WITH CHECK ADD  CONSTRAINT [FK_TempOrder_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TempOrder] CHECK CONSTRAINT [FK_TempOrder_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[TempOrder]  WITH CHECK ADD  CONSTRAINT [FK_TempOrder_TourDateTimeId] FOREIGN KEY([TourDateTimeId])
REFERENCES [dbo].[TourDateTime] ([Id])
GO
ALTER TABLE [dbo].[TempOrder] CHECK CONSTRAINT [FK_TempOrder_TourDateTimeId]
GO
ALTER TABLE [dbo].[Tour]  WITH CHECK ADD  CONSTRAINT [FK_Tour_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Tour] CHECK CONSTRAINT [FK_Tour_CategoryId]
GO
ALTER TABLE [dbo].[Tour]  WITH CHECK ADD  CONSTRAINT [FK_Tour_RatingId] FOREIGN KEY([AverageRatingId])
REFERENCES [dbo].[Rating] ([Id])
GO
ALTER TABLE [dbo].[Tour] CHECK CONSTRAINT [FK_Tour_RatingId]
GO
ALTER TABLE [dbo].[TourAttendees]  WITH CHECK ADD  CONSTRAINT [FK_TourAttendees_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[TourAttendees] CHECK CONSTRAINT [FK_TourAttendees_OrderId]
GO
ALTER TABLE [dbo].[TourDate]  WITH CHECK ADD  CONSTRAINT [FK_TourDate_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([Id])
GO
ALTER TABLE [dbo].[TourDate] CHECK CONSTRAINT [FK_TourDate_TourId]
GO
ALTER TABLE [dbo].[TourDateTime]  WITH CHECK ADD  CONSTRAINT [FK_TourDateTime_TourDateId] FOREIGN KEY([TourDateId])
REFERENCES [dbo].[TourDate] ([Id])
GO
ALTER TABLE [dbo].[TourDateTime] CHECK CONSTRAINT [FK_TourDateTime_TourDateId]
GO
ALTER TABLE [dbo].[TourDateTime]  WITH CHECK ADD  CONSTRAINT [FK_TourDateTime_TourTimeId] FOREIGN KEY([TourTimeId])
REFERENCES [dbo].[TourTime] ([Id])
GO
ALTER TABLE [dbo].[TourDateTime] CHECK CONSTRAINT [FK_TourDateTime_TourTimeId]
GO
ALTER TABLE [dbo].[TourTime]  WITH CHECK ADD  CONSTRAINT [FK_TourTime_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([Id])
GO
ALTER TABLE [dbo].[TourTime] CHECK CONSTRAINT [FK_TourTime_TourId]
GO
ALTER TABLE [dbo].[TourTimeTable]  WITH CHECK ADD  CONSTRAINT [FK_TourTimeTable_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[TourTimeTable] CHECK CONSTRAINT [FK_TourTimeTable_LocationId]
GO
ALTER TABLE [dbo].[TourTimeTable]  WITH CHECK ADD  CONSTRAINT [FK_TourTimeTable_TourTimeId] FOREIGN KEY([TourTimeId])
REFERENCES [dbo].[TourTime] ([Id])
GO
ALTER TABLE [dbo].[TourTimeTable] CHECK CONSTRAINT [FK_TourTimeTable_TourTimeId]
GO
ALTER TABLE [dbo].[Town]  WITH CHECK ADD  CONSTRAINT [FK_Town_IslandId] FOREIGN KEY([IslandId])
REFERENCES [dbo].[Island] ([Id])
GO
ALTER TABLE [dbo].[Town] CHECK CONSTRAINT [FK_Town_IslandId]
GO
ALTER TABLE [dbo].[WishList]  WITH CHECK ADD  CONSTRAINT [FK_WishList_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WishList] CHECK CONSTRAINT [FK_WishList_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[WishList]  WITH CHECK ADD  CONSTRAINT [FK_WishList_TourId] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tour] ([Id])
GO
ALTER TABLE [dbo].[WishList] CHECK CONSTRAINT [FK_WishList_TourId]
GO
