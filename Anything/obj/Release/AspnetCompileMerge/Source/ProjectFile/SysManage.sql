
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
           (N'BoughtBonus',N'������\���Q',N'5',N'%','True','0','2016-10-02',N'���O���Q')
           ,(N'UplineLevel',N'���Q�W�u���t�̦h�X�h',N'6',N'int','True','0','2016-10-02',N'�̦h�W�u')
           ,(N'UplineBonus',N'�W�u�`���Q������X��',N'2.5',N'%','True','0','2016-10-02',N'�W�u���Q')
           ,(N'SystemFee',N'���x��Q������X��',N'2.5',N'%','True','0','2016-10-02',N'���x����O')
           ,(N'BankFee',N'���y����O',N'3.5',N'%','True','0','2016-10-02',N'���y����O')
           ,(N'BonusRecorded',N'���Q����鬰�C��X��',N'25',N'DayOfMonth','True','0','2016-10-02',N'�����')
           ,(N'BonusCheckout',N'���Q�J�b�鬰�C��X��',N'30',N'DayOfMonth','True','0','2016-10-02',N'�J�b��')
           ,(N'SweetDays',N'���O�i����Q�e���',N'180',N'Days','True','0','2016-10-02',N'�e���')
           ,(N'BonusLimit',N'��o���Q,���ݭn�����O�̧C���B',N'1000',N'int','True','0','2016-10-02',N'���Q�C��')
		   ,(N'VIP',N'����VIP�������N�|�`�n�󭺭���,�j�j�����n���v',N'20',N'int','True','0','2016-10-02',N'VIP���')
		   ,(N'VIPPrice',N'VIP����',N'2000',N'int','True','0','2016-10-02',N'VIP����')
		   ,(N'VIPDays',N'VIP�Ѽ�',N'30',N'int','True','0','2016-10-02',N'VIP�Ѽ�')
GO



