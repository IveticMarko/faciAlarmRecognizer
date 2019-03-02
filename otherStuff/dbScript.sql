USE [FaciDB]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_entereddatatable_actualstatustable]') AND parent_object_id = OBJECT_ID(N'[dbo].[entereddatatable]'))
ALTER TABLE [dbo].[entereddatatable] DROP CONSTRAINT [FK_entereddatatable_actualstatustable]
GO
/****** Object:  Table [dbo].[roomstatustable]    Script Date: 3/2/2019 8:03:55 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[roomstatustable]') AND type in (N'U'))
DROP TABLE [dbo].[roomstatustable]
GO
/****** Object:  Table [dbo].[loggingtable]    Script Date: 3/2/2019 8:03:55 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[loggingtable]') AND type in (N'U'))
DROP TABLE [dbo].[loggingtable]
GO
/****** Object:  Table [dbo].[entereddatatable]    Script Date: 3/2/2019 8:03:55 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[entereddatatable]') AND type in (N'U'))
DROP TABLE [dbo].[entereddatatable]
GO
/****** Object:  Table [dbo].[clientdevicetable]    Script Date: 3/2/2019 8:03:55 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[clientdevicetable]') AND type in (N'U'))
DROP TABLE [dbo].[clientdevicetable]
GO
/****** Object:  Table [dbo].[actualstatustable]    Script Date: 3/2/2019 8:03:55 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[actualstatustable]') AND type in (N'U'))
DROP TABLE [dbo].[actualstatustable]
GO
/****** Object:  Table [dbo].[actualstatustable]    Script Date: 3/2/2019 8:03:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[actualstatustable](
	[StatusID] [int] NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_actualstatustable] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clientdevicetable]    Script Date: 3/2/2019 8:03:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[clientdevicetable](
	[ClientDeviceID] [int] IDENTITY(1,1) NOT NULL,
	[ClientDeviceToken] [nvarchar](max) NULL,
	[Group] [int] NOT NULL,
	[RoomID] [int] NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_clientdevicetable] PRIMARY KEY CLUSTERED 
(
	[ClientDeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[entereddatatable]    Script Date: 3/2/2019 8:03:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[entereddatatable](
	[RoomID] [int] NOT NULL,
	[GroupController] [nvarchar](2) NOT NULL,
	[Group] [tinyint] NOT NULL,
	[RoomName] [nvarchar](4) NOT NULL,
	[StatusID] [tinyint] NOT NULL,
	[SuplementData] [nvarchar](2) NULL,
	[AdditionalData] [nvarchar](100) NULL,
	[RoomDescription] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[ActualStatusID] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[loggingtable]    Script Date: 3/2/2019 8:03:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loggingtable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Data] [nvarchar](500) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_loggingtable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roomstatustable]    Script Date: 3/2/2019 8:03:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roomstatustable](
	[StatusID] [int] NOT NULL,
	[StatusName] [nvarchar](100) NOT NULL,
	[StatusCode] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_roomstatustable] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[actualstatustable] ([StatusID], [StatusName]) VALUES (1, N'Active')
INSERT [dbo].[actualstatustable] ([StatusID], [StatusName]) VALUES (2, N'Processed')
SET IDENTITY_INSERT [dbo].[clientdevicetable] ON 

INSERT [dbo].[clientdevicetable] ([ClientDeviceID], [ClientDeviceToken], [Group], [RoomID], [Active]) VALUES (2, N'ct19Qyuc5PI:APA91bH8Otgu1I5kuLLnK0NIpO9ZWJ5bjpA5_iR4nEhWPLl779yx0n1Sm_5WkEXdGkP1oYmacdio97VqcF6L6PKED00b68YSgLd0XpN405IjahUTlrj8spwkHFuRLEV8ZYcrrjrsnfwa', 3, 21, 1)
INSERT [dbo].[clientdevicetable] ([ClientDeviceID], [ClientDeviceToken], [Group], [RoomID], [Active]) VALUES (5, NULL, 3, 20, 1)
SET IDENTITY_INSERT [dbo].[clientdevicetable] OFF
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (0, N'off', N'off')
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (1, N'sb', N'sb')
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (2, N'aw', N'aw')
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (3, N'aw2', N'aw2')
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (4, N'pat', N'pat')
INSERT [dbo].[roomstatustable] ([StatusID], [StatusName], [StatusCode]) VALUES (5, N'wc', N'wc')
ALTER TABLE [dbo].[entereddatatable]  WITH CHECK ADD  CONSTRAINT [FK_entereddatatable_actualstatustable] FOREIGN KEY([ActualStatusID])
REFERENCES [dbo].[actualstatustable] ([StatusID])
GO
ALTER TABLE [dbo].[entereddatatable] CHECK CONSTRAINT [FK_entereddatatable_actualstatustable]
GO
