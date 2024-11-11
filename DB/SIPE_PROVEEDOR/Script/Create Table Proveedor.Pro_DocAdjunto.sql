USE [SIPE_PROVEEDOR]
GO

IF EXISTS (select 1 from sysobjects where name = 'Pro_DocAdjunto' and type='U')
begin
    drop table  Proveedor.Pro_DocAdjunto
    if exists (select 1 from sysobjects where name = 'Pro_DocAdjunto' and type = 'U')
      PRINT '<<< DROP TABLE Pro_DocAdjunto -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_DocAdjunto *OK* =='
end
GO

CREATE TABLE [Proveedor].[Pro_DocAdjunto](
	[CodProveedor] [varchar](10) NOT NULL,
	[IdDocAdjunto] [bigint] IDENTITY(1,1) NOT NULL,
	[CodDocumento] [varchar](10) NULL,
	[NomArchivo] [varchar](255) NULL,
	[Archivo] [varchar](255) NULL,
	[FechaCarga] [datetime] NULL,
	[Estado] [bit] NULL)

GO

if exists(select 1 from sysobjects where name='Pro_DocAdjunto' and type = 'U')
  PRINT '== CREATE TABLE Pro_DocAdjunto *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_DocAdjunto -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_DocAdjunto ADD PRIMARY KEY (IdDocAdjunto)
CREATE INDEX ix_Pro_IdDocAdjunto_01 ON Proveedor.Pro_DocAdjunto (IdDocAdjunto)
GO

ALTER TABLE [Proveedor].[Pro_DocAdjunto]  WITH CHECK ADD  CONSTRAINT [FK_PRO_DOC_REFERENCE_PRO_P] FOREIGN KEY([CodProveedor])
REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
GO

ALTER TABLE [Proveedor].[Pro_DocAdjunto] CHECK CONSTRAINT [FK_PRO_DOC_REFERENCE_PRO_P]
GO


