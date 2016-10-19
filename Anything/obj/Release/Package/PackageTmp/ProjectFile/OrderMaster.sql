USE [Anything]
GO

/****** Object:  Table [dbo].[OrderMaster]    Script Date: 10/10/2016 13:31:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductName] [nvarchar](300) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
	[Amount] [int] NOT NULL,
	[CheckIn] [datetime] NOT NULL,
	[CheckOut] [datetime] NOT NULL,
	[Status] [nvarchar](2) NOT NULL,
	[Tel] [nvarchar](20) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](150) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Remark] [nvarchar](500) NULL,
	[Modified] [datetime] NOT NULL,
	[Modify] [int] NOT NULL,
	[PayVendor] [nvarchar](100) NOT NULL,
	[ShareBonus] [decimal](10, 2) NOT NULL,
	[BoughtBonus] [decimal](10, 2) NOT NULL,
	[SystemBonus] [decimal](10, 2) NOT NULL,
	[MerchantTradeNo] [nvarchar](100) NOT NULL,
	[TradeNo] [nvarchar](100) NULL,
	[ProductType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

