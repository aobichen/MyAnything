USE [Anything]
GO

/****** Object:  Table [dbo].[BonusSystem]    Script Date: 09/27/2016 17:26:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BonusSystem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserBonus] [float] NOT NULL,
	[ParentBonus] [float] NOT NULL,
	[Modifiter] [nvarchar](50) NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_BonusSystem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

