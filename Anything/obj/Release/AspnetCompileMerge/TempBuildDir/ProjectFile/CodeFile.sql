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
(N'���H��',N'Beds',N'Doubled Beds',N'���H�ɫ�','True',N'�ɫ�')
,(N'��H��',N'Beds',N'Single Beds',N'��H�ɫ�','True',N'�ɫ�')
,(N'��H��',N'Beds',N'Single Beds',N'��H�ɫ�','True',N'�ɫ�')
,(N'��H���H�U�@��',N'Beds',N'Single adn Double Beds',N'��H�M���H��','True',N'�ɫ�')
,(N'�M���q�E',N'Beds',N'large Beds',N'�M���q�Q','True',N'�ɫ�')
,(N'��L',N'Beds',N'lager Beds',N'���H��','True',N'�ɫ�') 
,(N'��H��',N'Rooms',N'One Room',N'��H��','True',N'�Ы�')
,(N'���H��',N'Rooms',N'Two Room',N'���H��','True',N'�Ы�')
,(N'�T�H��',N'Rooms',N'five Room',N'�T�H��','True',N'�Ы�')
,(N'3+1��',N'Rooms',N'Four Room',N'3+1��','True',N'�Ы�')
,(N'���H�H�W�Ы�',N'Rooms',N'Four Room',N'���H�H�W�Ы�','True',N'�Ы�')
,(N'�����j�T�y�ʶ]���O',N'AdPosition',N'HomeHeade',N'�����ݪO','True',N'�s�i')
,(N'�k���s�i',N'AdPosition',N'RightAd',N'�k���s�i��','True',N'�s�i')
,(N'�����s�i',N'AdPosition',N'LeftAd',N'�����s�i��','True',N'�s�i')
,(N'�����p�]���O',N'AdPosition',N'smallAd',N'����W�s�i','True',N'�s�i')