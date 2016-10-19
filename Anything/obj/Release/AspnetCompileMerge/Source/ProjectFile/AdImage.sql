USE [Anything]
GO

/****** Object:  Table [dbo].[AdImage]    Script Date: 09/27/2016 17:31:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AdImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AdId] [int] NOT NULL,
	[Image] [image] NOT NULL,
	[Path] [nvarchar](250) NULL,
	[Name] [nvarchar](50) NULL,
	[Enabled] [bit] NULL,
	[ImageText] [nvarchar](150) NULL,
 CONSTRAINT [PK_AdImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AdImage]  WITH CHECK ADD  CONSTRAINT [FK_AdImage_AdOrder] FOREIGN KEY([AdId])
REFERENCES [dbo].[AdOrder] ([ID])
GO

ALTER TABLE [dbo].[AdImage] CHECK CONSTRAINT [FK_AdImage_AdOrder]
GO

