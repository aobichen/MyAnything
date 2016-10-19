USE [Anything]
GO

/****** Object:  Table [dbo].[Scenic]    Script Date: 09/27/2016 17:34:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Scenic](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[City] [nvarchar](10) NOT NULL,
	[CityId] [int] NOT NULL,
 CONSTRAINT [PK_Scenic] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [Anything].[dbo].[Scenic]
           ([Name]
           ,[Description]
           ,[Enabled]
           ,[City],
		   [CityId])
     VALUES
           (N'南灣',N'墾丁南灣沙灘','true',N'屏東縣','16')
		   , (N'鵝鑾鼻公園',N'墾丁鵝鑾鼻公園燈塔','true',N'屏東縣','16')
		   , (N'後壁湖',N'墾丁後壁湖,海產','true',N'屏東縣','16')
		   , (N'東大門夜市',N'花蓮東大門夜市美食','true',N'花蓮縣','18')
		    , (N'高雄市中心',N'高雄市中心','true',N'花蓮縣','18')
			, (N'六合夜市',N'六合夜市','true',N'高雄市','15')
			, (N'國際機場',N'高雄國際機場','true',N'高雄市','15')
			, (N'瑞豐夜市',N'瑞豐夜市','true',N'高雄市','15')
			, (N'85大樓',N'東帝士85大樓','true',N'高雄市','15')
			, (N'捷運美麗島站',N'捷運美麗島站','true',N'高雄市','15')
			, (N'西子灣',N'西子灣','true',N'高雄市','15')
			, (N'壽山動物園',N'壽山動物園','true',N'高雄市','15')
GO
