USE [Anything]
GO

/****** Object:  Table [dbo].[AdManage]    Script Date: 09/27/2016 17:30:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AdManage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Position] [nvarchar](250) NOT NULL,
	[PositionId] [int] NOT NULL,
	[Image] [image] NULL,
	[AllowImage] [bit] NOT NULL,
	[AllowText] [bit] NOT NULL,
	[ImageLimit] [int] NULL,
	[TextLimit] [int] NULL,
	[ImageWidth] [float] NULL,
	[ImageHeight] [float] NULL,
	[MaxCount] [int] NULL,
	[Enabled] [bit] NOT NULL,
	[Creator] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Modify] [int] NOT NULL,
	[AllowImageType] [nvarchar](150) NULL,
	[SaleOff] [bit] NOT NULL,
	[SalePrice] [decimal](18, 0) NOT NULL,
	[DiscountPrice] [decimal](18, 0) NULL,
	[Days] [int] NOT NULL,
 CONSTRAINT [PK_AdManage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

