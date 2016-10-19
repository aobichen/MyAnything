USE [Anything]
GO

/****** Object:  Table [dbo].[HotelImage]    Script Date: 09/27/2016 17:33:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HotelImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HotelId] [int] NULL,
	[Image] [image] NULL,
	[Deleted] [bit] NOT NULL,
	[Sort] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Path] [nvarchar](500) NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_HotelImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[HotelImage]  WITH CHECK ADD  CONSTRAINT [FK_HotelImage_Hotel] FOREIGN KEY([HotelId])
REFERENCES [dbo].[Hotel] ([ID])
GO

ALTER TABLE [dbo].[HotelImage] CHECK CONSTRAINT [FK_HotelImage_Hotel]
GO

