USE [Anything]
GO

/****** Object:  Table [dbo].[City_TW]    Script Date: 09/19/2016 20:15:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[City_TW](
	[ID] [int] NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Location] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



Insert into City_TW(ID,City,Location)Values(0,N'請選擇縣市',0),
(1,N'基隆市',1),(2,N'臺北市',1),(3,N'新北市',1),(4,N'桃園市',1),
(5,N'新竹市',1),(6,N'新竹縣',1),(7,N'苗栗縣',2),(8,N'臺中市',2),
(9,N'彰化縣',2),(10,N'南投縣',2),(11,N'雲林縣',2),(12,N'嘉義市',3),
(13,N'嘉義縣',3),(14,N'臺南市',3),(15,N'高雄市',3),(16,N'屏東縣',3),
(17,N'臺東縣',4),(18,N'花蓮縣',4),(19,N'宜蘭縣',1),(20,N'澎湖縣',3),
(21,N'金門縣',5),(22,N'連江縣',5);