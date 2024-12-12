USE [VillageRental]
GO
/****** Object:  Table [dbo].[category]    Script Date: 12/5/2024 3:07:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NOT NULL,
	[name] [varchar](25) NOT NULL,
	CONSTRAINT [PK_category1] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [UQ_category_category_id] UNIQUE ([category_id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rental_equipment]    Script Date: 12/5/2024 3:07:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rental_equipment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[equipment_id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](250) NULL,
	[daily_rate] [decimal](16, 2) NULL,
	[category_id] [int] NOT NULL,
	CONSTRAINT [FK_rental_eq_category_id] FOREIGN KEY ([category_id])
	REFERENCES [dbo].[category] ([category_id]) ON UPDATE CASCADE,
	CONSTRAINT [PK_rental_equipment] PRIMARY KEY CLUSTERED ([id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[category] ON 
GO
INSERT [dbo].[category] ([id], [category_id], [name]) VALUES (1, 10, N'Power Tools')
GO
INSERT [dbo].[category] ([id], [category_id], [name]) VALUES (2, 20, N'Yard Equipment')
GO
INSERT [dbo].[category] ([id], [category_id], [name]) VALUES (3, 30, N'Compressors')
GO
INSERT [dbo].[category] ([id], [category_id], [name]) VALUES (4, 40, N'Generators')
GO
INSERT [dbo].[category] ([id], [category_id], [name]) VALUES (5, 50, N'Air Tools')
GO
SET IDENTITY_INSERT [dbo].[category] OFF
GO
