USE [Anything]
GO

/****** Object:  Table [dbo].[Hotel]    Script Date: 09/27/2016 17:33:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Hotel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](10) NOT NULL,
	[City] [int] NOT NULL,
	[Area] [int] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[WebSite] [nvarchar](200) NOT NULL,
	[Introduce] [nvarchar](500) NULL,
	[Feature] [nvarchar](500) NOT NULL,
	[Information] [nvarchar](1000) NOT NULL,
	[ServiceOptions] [nvarchar](100) NULL,
	[Scenics] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[SaleOff] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NULL,
	[Tel] [nvarchar](20) NULL,
 CONSTRAINT [PK_Hotel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

