USE [FaciDB]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_senddatatable_entereddatatable]') AND parent_object_id = OBJECT_ID(N'[dbo].[senddatatable]'))
ALTER TABLE [dbo].[senddatatable] DROP CONSTRAINT [FK_senddatatable_entereddatatable]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_senddatatable_clientdevicetable]') AND parent_object_id = OBJECT_ID(N'[dbo].[senddatatable]'))
ALTER TABLE [dbo].[senddatatable] DROP CONSTRAINT [FK_senddatatable_clientdevicetable]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_entereddatatable_actualstatustable]') AND parent_object_id = OBJECT_ID(N'[dbo].[entereddatatable]'))
ALTER TABLE [dbo].[entereddatatable] DROP CONSTRAINT [FK_entereddatatable_actualstatustable]
GO
/****** Object:  Table [dbo].[senddatatable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[senddatatable]') AND type in (N'U'))
DROP TABLE [dbo].[senddatatable]
GO
/****** Object:  Table [dbo].[roomstatustable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[roomstatustable]') AND type in (N'U'))
DROP TABLE [dbo].[roomstatustable]
GO
/****** Object:  Table [dbo].[loggingtable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[loggingtable]') AND type in (N'U'))
DROP TABLE [dbo].[loggingtable]
GO
/****** Object:  Table [dbo].[entereddatatable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[entereddatatable]') AND type in (N'U'))
DROP TABLE [dbo].[entereddatatable]
GO
/****** Object:  Table [dbo].[clientdevicetable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[clientdevicetable]') AND type in (N'U'))
DROP TABLE [dbo].[clientdevicetable]
GO
/****** Object:  Table [dbo].[alarmtable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[alarmtable]') AND type in (N'U'))
DROP TABLE [dbo].[alarmtable]
GO
/****** Object:  Table [dbo].[alarmofdevicestable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[alarmofdevicestable]') AND type in (N'U'))
DROP TABLE [dbo].[alarmofdevicestable]
GO
/****** Object:  Table [dbo].[actualstatustable]    Script Date: 3/30/2019 8:07:45 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[actualstatustable]') AND type in (N'U'))
DROP TABLE [dbo].[actualstatustable]
GO
/****** Object:  Table [dbo].[actualstatustable]    Script Date: 3/30/2019 8:07:45 PM ******/
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
/****** Object:  Table [dbo].[alarmofdevicestable]    Script Date: 3/30/2019 8:07:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[alarmofdevicestable](
	[DeviceID] [int] NOT NULL,
	[AlarmID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[alarmtable]    Script Date: 3/30/2019 8:07:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[alarmtable](
	[AlarmID] [int] NOT NULL,
	[Color] [nvarchar](50) NULL,
	[Sound] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clientdevicetable]    Script Date: 3/30/2019 8:07:45 PM ******/
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
	[IsInTheSystem] [bit] NULL,
 CONSTRAINT [PK_clientdevicetable] PRIMARY KEY CLUSTERED 
(
	[ClientDeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[entereddatatable]    Script Date: 3/30/2019 8:07:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[entereddatatable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
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
	[ActualStatusID] [int] NOT NULL,
 CONSTRAINT [PK_entereddatatable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[loggingtable]    Script Date: 3/30/2019 8:07:45 PM ******/
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
/****** Object:  Table [dbo].[roomstatustable]    Script Date: 3/30/2019 8:07:45 PM ******/
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
/****** Object:  Table [dbo].[senddatatable]    Script Date: 3/30/2019 8:07:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[senddatatable](
	[ClientDeviceID] [int] NOT NULL,
	[EnteredDataID] [int] NOT NULL
) ON [PRIMARY]
GO
INSERT [dbo].[actualstatustable] ([StatusID], [StatusName]) VALUES (1, N'Active')
INSERT [dbo].[actualstatustable] ([StatusID], [StatusName]) VALUES (2, N'Processed')
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
ALTER TABLE [dbo].[senddatatable]  WITH CHECK ADD  CONSTRAINT [FK_senddatatable_clientdevicetable] FOREIGN KEY([ClientDeviceID])
REFERENCES [dbo].[clientdevicetable] ([ClientDeviceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[senddatatable] CHECK CONSTRAINT [FK_senddatatable_clientdevicetable]
GO
ALTER TABLE [dbo].[senddatatable]  WITH CHECK ADD  CONSTRAINT [FK_senddatatable_entereddatatable] FOREIGN KEY([EnteredDataID])
REFERENCES [dbo].[entereddatatable] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[senddatatable] CHECK CONSTRAINT [FK_senddatatable_entereddatatable]
GO
