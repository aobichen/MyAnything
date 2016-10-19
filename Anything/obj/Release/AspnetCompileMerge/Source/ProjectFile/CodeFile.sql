USE [Anything]
GO

/****** Object:  Table [dbo].[CodeFile]    Script Date: 09/27/2016 17:31:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CodeFile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemDescription] [nvarchar](200) NOT NULL,
	[ItemType] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](300) NULL,
	[ItemCode] [nvarchar](50) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[TypeText] [nvarchar](50) NULL,
 CONSTRAINT [PK_CodeFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [Anything].[dbo].[CodeFile]
([ItemDescription],[ItemType],[Remark],[ItemCode],[Enabled],[TypeText])
VALUES
(N'雙人床',N'Beds',N'Doubled Beds',N'雙人床型','True',N'床型')
,(N'單人床',N'Beds',N'Single Beds',N'單人床型','True',N'床型')
,(N'單人床',N'Beds',N'Single Beds',N'單人床型','True',N'床型')
,(N'單人雙人各一床',N'Beds',N'Single adn Double Beds',N'單人和雙人床','True',N'床型')
,(N'和式通舖',N'Beds',N'large Beds',N'和式通鋪','True',N'床型')
,(N'其他',N'Beds',N'lager Beds',N'雙人房','True',N'床型') 
,(N'單人房',N'Rooms',N'One Room',N'單人房','True',N'房型')
,(N'雙人房',N'Rooms',N'Two Room',N'雙人房','True',N'房型')
,(N'三人房',N'Rooms',N'five Room',N'三人房','True',N'房型')
,(N'3+1房',N'Rooms',N'Four Room',N'3+1房','True',N'房型')
,(N'五人以上房型',N'Rooms',N'Four Room',N'五人以上房型','True',N'房型')
,(N'首頁大幅流動跑馬燈',N'AdPosition',N'HomeHeade',N'首頁看板','True',N'廣告')
,(N'右側廣告',N'AdPosition',N'RightAd',N'右側廣告欄','True',N'廣告')
,(N'左側廣告',N'AdPosition',N'LeftAd',N'左側廣告欄','True',N'廣告')
,(N'首頁小跑馬燈',N'AdPosition',N'smallAd',N'內文上廣告','True',N'廣告')