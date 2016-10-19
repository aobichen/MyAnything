USE [Anything]
GO

/****** Object:  Table [dbo].[ServiceOption]    Script Date: 09/27/2016 17:18:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceOption](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Created] [datetime] NULL,
	[Show] [bit] NULL,
 CONSTRAINT [PK_ServiceOption] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT INTO [Anything].[dbo].[ServiceOption]
           ([Text]
           ,[Description]
           ,[Enabled]
           ,[Created]
           ,[Show])
     VALUES
           (N'Wifi免費網路',N'wifi無線網路','True','2016-09-18 00:00:00','True')
           ,(N'停車位(場)',N'停車位(場)','True','2016-09-18 00:00:00','True')
           ,(N'提供早餐',N'提供早餐','True','2016-09-18 00:00:00','True')
           ,(N'可帶寵物入住',N'可帶寵物入住','True','2016-09-18 00:00:00','True')
           ,(N'有線電視頻道',N'有線電視頻道','True','2016-09-18 00:00:00','True')
           ,(N'平面電視',N'平面電視','True','2016-09-18 00:00:00','True')
GO



