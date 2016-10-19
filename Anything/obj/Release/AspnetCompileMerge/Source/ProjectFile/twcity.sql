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



Insert into City_TW(ID,City,Location)Values(0,N'�п�ܿ���',0),
(1,N'�򶩥�',1),(2,N'�O�_��',1),(3,N'�s�_��',1),(4,N'��饫',1),
(5,N'�s�˥�',1),(6,N'�s�˿�',1),(7,N'�]�߿�',2),(8,N'�O����',2),
(9,N'���ƿ�',2),(10,N'�n�뿤',2),(11,N'���L��',2),(12,N'�Ÿq��',3),
(13,N'�Ÿq��',3),(14,N'�O�n��',3),(15,N'������',3),(16,N'�̪F��',3),
(17,N'�O�F��',4),(18,N'�Ὤ��',4),(19,N'�y����',1),(20,N'���',3),
(21,N'������',5),(22,N'�s����',5);