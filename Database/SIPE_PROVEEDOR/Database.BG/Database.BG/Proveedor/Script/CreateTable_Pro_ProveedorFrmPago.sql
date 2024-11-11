USE [SIPE_PROVEEDOR]
GO

if exists(select 1
	from sysobjects 
	where name = 'Pro_ProveedorFrmPago'
	and xtype = 'U'
)
begin

	drop table Proveedor.Pro_ProveedorFrmPago
end

GO

CREATE TABLE [Proveedor].[Pro_ProveedorFrmPago](
	[IdProveedorPago] [bigint] IDENTITY(1,1) NOT NULL,
	[CodProveedor] [varchar](13) NOT NULL,
	[FormaPago] [varchar](10) NULL, --Detalle
	[Extrangera] [bit] NULL,
	[CodSapBanco] [varchar](15) NULL,
	[Pais] [varchar](10) NULL,
	[TipoCuenta] [varchar](3) NULL, --Tipo
	[NumeroCuenta] [varchar](18) NULL, --Numero
	[TitularCuenta] [varchar](60) NULL,
	[ReprCuenta] [varchar](200) NULL,
	[CodSwift] [varchar](10) NULL,
	[CodBENINT] [varchar](10) NULL,
	[CodABA] [varchar](10) NULL,
	[Principal] [bit] NULL,
	[Provincia] [varchar](10) NULL,
	[DirBancoExtranjero] [varchar](250) NULL,
	[Estado] [bit] NULL,
	[BancoExtranjero] [varchar](250) NULL,
	
 CONSTRAINT [PK_PRO_PRVBANCO] PRIMARY KEY CLUSTERED 
(
	[IdProveedorPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO





