USE [Anything]
GO

/****** Object:  Table [dbo].[AdOrder]    Script Date: 09/27/2016 17:31:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AdOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Position] [nvarchar](250) NOT NULL,
	[bought] [decimal](18, 0) NOT NULL,
	[AllowImage] [bit] NOT NULL,
	[AllowText] [bit] NOT NULL,
	[ImageLimit] [int] NULL,
	[TextLimit] [int] NULL,
	[ImageWidth] [float] NULL,
	[ImageHeight] [float] NULL,
	[Creator] [int] NULL,
	[Created] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Href] [nvarchar](300) NULL,
	[Days] [int] NOT NULL,
	[Tel] [nvarchar](20) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](150) NULL,
	[AdId] [int] NOT NULL,
	[PositionId] [int] NOT NULL,
	[Text] [nvarchar](1500) NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AdOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

