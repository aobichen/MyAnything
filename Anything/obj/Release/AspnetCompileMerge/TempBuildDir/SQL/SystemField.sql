USE [MyAnything]
GO

/****** Object:  Table [dbo].[SystemField]    Script Date: 2016/12/1 �U�� 04:30:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemField](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [nvarchar](10) NOT NULL,
	[ItemValue] [nvarchar](50) NOT NULL,
	[ItemDescription] [nvarchar](500) NOT NULL,
	[ItemUnit] [nvarchar](10) NOT NULL,
	[ItemType] [nvarchar](20) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Creator] [int] NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[Modify] [int] NULL,
 CONSTRAINT [PK_SystemField] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [MyAnything]
GO

INSERT INTO [dbo].[SystemField]
           ([ItemCode]
           ,[ItemValue]
           ,[ItemDescription]
           ,[ItemUnit]
           ,[ItemType]
           ,[Enabled]
           ,[Creator]
           ,[Created]
           ,[Modified]
           ,[Modify])
     VALUES
           ('BS','3.5','���y����O','float','CarshFlow','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','���x���@�O(%)','float','Platform','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','���J���ˤH���Q(%)','float','HotelPromo','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','���O���Q(%)','float','BuyFeedback','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','���ˤH�������Q(%)�̦h���h','float','UpperUser','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','1000','�ϥά��Q���̧C���O���B','int','AmtMinLimit','true',Null,GETDATE(),GETDATE(),Null)
GO

