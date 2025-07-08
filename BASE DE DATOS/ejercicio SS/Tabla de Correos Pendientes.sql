USE [SistemaSeguridad]
GO

/****** Object:  Table [dbo].[CorreosPendientes]    Script Date: 8/07/2025 13:15:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CorreosPendientes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Destinatario] [nvarchar](255) NULL,
	[CC] [nvarchar](255) NULL,
	[Asunto] [nvarchar](255) NULL,
	[Cuerpo] [nvarchar](max) NULL,
	[RutaAdjunto] [nvarchar](500) NULL,
	[Estado] [nvarchar](50) NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CorreosPendientes] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO


