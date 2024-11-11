use SIPE_PROVEEDOR
go

IF EXISTS (select 1 from sysobjects where name = 'Pro_SolProveedorDetalle' and type='U')
begin
    drop table  Proveedor.Pro_SolProveedorDetalle
    if exists (select 1 from sysobjects where name = 'Pro_SolProveedorDetalle' and type = 'U')
      PRINT '<<< DROP TABLE Pro_SolProveedorDetalle -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_SolProveedorDetalle *OK* =='
end
GO

CREATE TABLE [Proveedor].[Pro_SolProveedorDetalle](
	[IdDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitud] [bigint] NOT NULL,
	[EsCritico] [char](1) NULL,
	[ProcesoBrindaSoporte] [varchar](850) NULL,
	[EsServiciosAux] [char](1) NULL,
	[IdArea] [varchar](5) NULL,
	[IdFormasPago] [varchar](5) NULL,
	[IdStatus] [varchar](5) NULL,
	[LlenadoDocVinculacion] [char](1) NULL,
	[VinculadoBG] [char](1) NULL,
	[Recurrente] [char](1) NULL,
	[FirmadoSegInfo] [char](1) NULL,
	[FirmadoPCI] [char](1) NULL,
	[CalificadoContNegocio] [varchar](12) NULL,
	[Sgs] [varchar](1000) NULL,
	[TipoCalificacion] [varchar](5) NULL,
	[Calificacion] [varchar](5) NULL,
	[FecTermCalificacion] [varchar](20) NULL,
	[UsuarioCreacion] [varchar](15) NULL,
	[FechaCreacion] [datetime] NULL,
	[CodActividadEconomica] [varchar](20) NULL,
	[TipoServicio] [varchar](10) NULL,
	[RelacionBanco] [bit] NULL,
	[RelacionIdentificacion] [varchar](20) NULL,
	[RelacionNombres] [varchar](100) NULL,
	[RelacionArea] [varchar](50) NULL,
	[PersonaExpuesta] [bit] NULL,
	[EleccionPopular] [bit] NULL,
)
GO

if exists(select 1 from sysobjects where name='Pro_SolProveedorDetalle' and type = 'U')
  PRINT '== CREATE TABLE Pro_SolProveedorDetalle *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_SolProveedorDetalle -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_SolProveedorDetalle ADD PRIMARY KEY (IdDetalle)
CREATE INDEX ix_Pro_SolProveedorDetalle_01 ON Proveedor.Pro_SolProveedorDetalle (IdSolicitud)

GO

ALTER TABLE [Proveedor].[Pro_SolProveedorDetalle]  WITH CHECK ADD  CONSTRAINT [FK_PRO_SOLD_REFERENCE_PRO_SOLP] FOREIGN KEY([IdSolicitud])
REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
GO

UPDATE STATISTICS Proveedor.Pro_SolProveedorDetalle  
GO