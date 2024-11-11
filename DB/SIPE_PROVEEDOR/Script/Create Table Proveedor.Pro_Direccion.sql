USE [SIPE_PROVEEDOR]
GO

IF EXISTS (select 1 from sysobjects where name = 'Pro_Direccion' and type='U')
begin
    drop table  Proveedor.Pro_Direccion
    if exists (select 1 from sysobjects where name = 'Pro_Direccion' and type = 'U')
      PRINT '<<< DROP TABLE Pro_Direccion -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_Direccion *OK* =='
end

GO

CREATE TABLE [Proveedor].[Pro_Direccion](
	[IdDireccion] [bigint] IDENTITY(1,1) NOT NULL,
	[CodProveedor] [varchar](10) NOT NULL,
	[Pais] [varchar](10) NULL,
	[Provincia] [varchar](10) NULL,
	[Ciudad] [varchar](12) NULL,
	[CallePrincipal] [varchar](100) NULL,
	[CalleSecundaria] [varchar](100) NULL,
	[PisoEdificio] [varchar](10) NULL,
	[CodPostal] [varchar](30) NULL,
	[Solar] [varchar](30) NULL,
	[Estado] [bit] NULL)

GO

if exists(select 1 from sysobjects where name='Pro_Direccion' and type = 'U')
  PRINT '== CREATE TABLE Pro_Direccion *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_Direccion -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_Direccion ADD PRIMARY KEY (IdDireccion)
CREATE INDEX ix_Pro_IdDireccion_01 ON Proveedor.Pro_Direccion (IdDireccion)

GO

UPDATE STATISTICS Proveedor.Pro_Direccion
GO


