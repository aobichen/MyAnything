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
           (N'�n�W',N'���B�n�W�F�y','true',N'�̪F��','16')
		   , (N'�Z�q�󤽶�',N'���B�Z�q�󤽶�O��','true',N'�̪F��','16')
		   , (N'�����',N'���B�����,����','true',N'�̪F��','16')
		   , (N'�F�j���]��',N'�Ὤ�F�j���]������','true',N'�Ὤ��','18')
		    , (N'����������',N'����������','true',N'�Ὤ��','18')
			, (N'���X�]��',N'���X�]��','true',N'������','15')
			, (N'��ھ���',N'������ھ���','true',N'������','15')
			, (N'���ש]��',N'���ש]��','true',N'������','15')
			, (N'85�j��',N'�F�Ҥh85�j��','true',N'������','15')
			, (N'���B���R�q��',N'���B���R�q��','true',N'������','15')
			, (N'��l�W',N'��l�W','true',N'������','15')
			, (N'�ؤs�ʪ���',N'�ؤs�ʪ���','true',N'������','15')
GO
