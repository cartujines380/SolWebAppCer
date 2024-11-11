USE [SIPE_PROVEEDOR]
GO
/****** Object:  Table [Seguridad].[Seg_Usuario]    Script Date: 1/4/2020 18:16:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Seguridad].[Seg_Usuario](
	[IdEmpresa] [int] NOT NULL,
	[Ruc] [varchar](13) NOT NULL,
	[Usuario] [varchar](20) NOT NULL,
	[IdParticipante] [int] NOT NULL,
	[CodProveedor] [varchar](10) NULL,
	[CorreoE] [varchar](50) NOT NULL,
	[Telefono] [varchar](50) NULL,
	[Celular] [varchar](50) NULL,
	[EsAdmin] [bit] NOT NULL,
	[Estado] [varchar](1) NOT NULL,
	[UsrAutorizador] [varchar](20) NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[FechaModificacion] [datetime] NULL,
	[CodLegacy] [varchar](10) NULL,
	[UsrLegacy] [varchar](20) NULL,
	[UsrCargo] [varchar](5) NULL,
	[UsrFuncion] [varchar](5) NULL,
	[UsrSubido] [int] NULL,
	[Sociedad] [varchar](5) NULL,
 CONSTRAINT [PK_Seg_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC,
	[Ruc] ASC,
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Proveedor]
) ON [Proveedor]

GO
SET ANSI_PADDING OFF
GO