USE [SistemaSeguridad]
GO

/****** Object:  Table [dbo].[Tokens]    Script Date: 27/05/2025 14:51:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tokens]') AND type in (N'U'))
DROP TABLE [dbo].[Tokens]
GO


