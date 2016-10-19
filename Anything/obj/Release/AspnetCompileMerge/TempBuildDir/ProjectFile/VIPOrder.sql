USE [Anything]
GO

/****** Object:  Table [dbo].[VIPOrder]    Script Date: 10/02/2016 20:04:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VIPOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Vip] [int] NOT NULL,
	[Pay] [decimal](10, 2) NOT NULL,
	[PayMethod] [nvarchar](100) NULL,
	[OrderStatus] [nvarchar](100) NULL,
	[PayType] [nvarchar](100) NULL,
	[Creator] [int] NULL,
	[Created] [datetime] NULL,
	[Address] [nvarchar](500) NOT NULL,
	[Tel] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_VIPOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[VIPOrder]  WITH CHECK ADD  CONSTRAINT [FK_VIPOrder_VIPs] FOREIGN KEY([ID])
REFERENCES [dbo].[VIPs] ([ID])
GO

ALTER TABLE [dbo].[VIPOrder] CHECK CONSTRAINT [FK_VIPOrder_VIPs]
GO

