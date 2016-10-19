
USE [Anything]
GO

/****** Object:  Table [dbo].[SysManage]    Script Date: 10/02/2016 19:42:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SysManage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FieldCode] [nvarchar](100) NOT NULL,
	[FieldDescription] [nvarchar](2000) NOT NULL,
	[Value] [nvarchar](10) NOT NULL,
	[Unit] [nvarchar](10) NULL,
	[Enabled] [bit] NOT NULL,
	[Editor] [int] NOT NULL,
	[Edited] [datetime] NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SysManage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [Anything].[dbo].[SysManage]
           ([FieldCode]
           ,[FieldDescription]
           ,[Value]
           ,[Unit]
           ,[Enabled]
           ,[Editor]
           ,[Edited]
		   ,[FieldName])
     VALUES
           (N'BoughtBonus',N'交易成功紅利',N'5',N'%','True','0','2016-10-02',N'消費紅利')
           ,(N'UplineLevel',N'紅利上線分配最多幾層',N'6',N'int','True','0','2016-10-02',N'最多上線')
           ,(N'UplineBonus',N'上線總紅利為售價幾成',N'2.5',N'%','True','0','2016-10-02',N'上線紅利')
           ,(N'SystemFee',N'平台獲利佔售價幾成',N'2.5',N'%','True','0','2016-10-02',N'平台手續費')
           ,(N'BankFee',N'金流手續費',N'3.5',N'%','True','0','2016-10-02',N'金流手續費')
           ,(N'BonusRecorded',N'紅利結算日為每月幾號',N'25',N'DayOfMonth','True','0','2016-10-02',N'結算日')
           ,(N'BonusCheckout',N'紅利入帳日為每月幾號',N'30',N'DayOfMonth','True','0','2016-10-02',N'入帳日')
           ,(N'SweetDays',N'消費可獲紅利蜜月期',N'180',N'Days','True','0','2016-10-02',N'蜜月期')
           ,(N'BonusLimit',N'獲得紅利,當月需要的消費最低金額',N'1000',N'int','True','0','2016-10-02',N'紅利低消')
		   ,(N'VIP',N'成為VIP的飯店將會常駐於首頁中,大大提高曝光率',N'20',N'int','True','0','2016-10-02',N'VIP資格')
		   ,(N'VIPPrice',N'VIP價格',N'2000',N'int','True','0','2016-10-02',N'VIP價格')
		   ,(N'VIPDays',N'VIP天數',N'30',N'int','True','0','2016-10-02',N'VIP天數')
GO



