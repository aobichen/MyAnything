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
           (N'Wifi�K�O����',N'wifi�L�u����','True','2016-09-18 00:00:00','True')
           ,(N'������(��)',N'������(��)','True','2016-09-18 00:00:00','True')
           ,(N'���Ѧ��\',N'���Ѧ��\','True','2016-09-18 00:00:00','True')
           ,(N'�i�a�d���J��',N'�i�a�d���J��','True','2016-09-18 00:00:00','True')
           ,(N'���u�q���W�D',N'���u�q���W�D','True','2016-09-18 00:00:00','True')
           ,(N'�����q��',N'�����q��','True','2016-09-18 00:00:00','True')
GO



