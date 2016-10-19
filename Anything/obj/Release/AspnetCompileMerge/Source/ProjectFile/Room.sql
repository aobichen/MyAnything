USE [Anything]
GO

/****** Object:  Table [dbo].[Room]    Script Date: 09/27/2016 17:34:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Room](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HotelId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[SellPrice] [decimal](18, 0) NOT NULL,
	[DiscountPrice] [decimal](18, 0) NULL,
	[Bonus] [decimal](18, 0) NOT NULL,
	[Amount] [int] NOT NULL,
	[Information] [nvarchar](2000) NULL,
	[Enabled] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Creator] [int] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Person] [int] NOT NULL,
	[Beds] [int] NOT NULL,
	[BedType] [nvarchar](2) NOT NULL,
	[RoomBed] [nvarchar](6) NOT NULL,
	[BussinessBonus] [int] NULL,
	[PlatformBonus] [int] NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_Hotel] FOREIGN KEY([HotelId])
REFERENCES [dbo].[Hotel] ([ID])
GO

ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_Hotel]
GO

