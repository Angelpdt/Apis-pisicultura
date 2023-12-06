USE [PROYECTO_psi]
GO

/****** Object:  Table [dbo].[produccion]    Script Date: 5/12/2023 7:11:21 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[produccion](
	[id_lote] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [int] NULL,
	[fecha_inicio] [varchar](50) NULL,
	[fecha_finalizacion] [varchar](50) NULL,
	[vendido] [char](2) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_lote] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

