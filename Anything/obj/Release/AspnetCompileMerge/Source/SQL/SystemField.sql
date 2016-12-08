USE [MyAnything]
GO

/****** Object:  Table [dbo].[SystemField]    Script Date: 2016/12/1 下午 04:30:57 ******/
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
           ('BS','3.5','金流手續費','float','CarshFlow','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','平台維護費(%)','float','Platform','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','民宿推薦人紅利(%)','float','HotelPromo','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','消費紅利(%)','float','BuyFeedback','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','2.5','推薦人平均紅利(%)最多六層','float','UpperUser','true',Null,GETDATE(),GETDATE(),Null),
		   ('BS','1000','使用紅利當月最低消費金額','int','AmtMinLimit','true',Null,GETDATE(),GETDATE(),Null)
GO

