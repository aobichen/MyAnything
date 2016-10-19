USE [Anything]
GO

/****** Object:  Table [dbo].[RoomImage]    Script Date: 09/27/2016 17:34:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoomImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoomId] [int] NOT NULL,
	[Image] [image] NOT NULL,
	[Path] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Sort] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_RoomImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoomImage]  WITH CHECK ADD  CONSTRAINT [FK_RoomImage_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([ID])
GO

ALTER TABLE [dbo].[RoomImage] CHECK CONSTRAINT [FK_RoomImage_Room]
GO

