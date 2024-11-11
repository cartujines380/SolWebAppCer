USE [SIPE_PROVEEDOR]
GO

IF EXISTS (select 1 from sysobjects where name = 'Pro_MedioContacto' and type='U')
begin
    drop table  Proveedor.Pro_MedioContacto
    if exists (select 1 from sysobjects where name = 'Pro_MedioContacto' and type = 'U')
      PRINT '<<< DROP TABLE Pro_MedioContacto -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_MedioContacto *OK* =='
end

GO

CREATE TABLE [Proveedor].[Pro_MedioContacto](
	[IdContacto] [bigint] NULL,
	[IdMedioContacto] [bigint] IDENTITY(1,1) NOT NULL,
	[CodProveedor] varchar(10) NULL,
	[TipMedioContacto] [varchar](10) NULL,
	[ValorMedioContacto] [varchar](100) NULL,
	[Estado] [bit] NULL)
GO

if exists(select 1 from sysobjects where name='Pro_MedioContacto' and type = 'U')
  PRINT '== CREATE TABLE Pro_MedioContacto *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_MedioContacto -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_MedioContacto ADD PRIMARY KEY (IdMedioContacto)
CREATE INDEX ix_Pro_IdMedioContacto_01 ON Proveedor.Pro_MedioContacto (IdMedioContacto)

GO

ALTER TABLE [Proveedor].[Pro_MedioContacto]  WITH CHECK ADD  CONSTRAINT [FK_PRO_M_REFERENCE_PRO_C] FOREIGN KEY([IdContacto])
REFERENCES [Proveedor].[Pro_Contacto] ([Id])
GO

ALTER TABLE [Proveedor].[Pro_MedioContacto] CHECK CONSTRAINT [FK_PRO_M_REFERENCE_PRO_C]
GO

ALTER TABLE [Proveedor].[Pro_MedioContacto]  WITH CHECK ADD  CONSTRAINT [FK_PRO_M_REFERENCE_PRO_P] FOREIGN KEY([CodProveedor])
REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
GO

ALTER TABLE [Proveedor].[Pro_MedioContacto] CHECK CONSTRAINT [FK_PRO_M_REFERENCE_PRO_P]
GO

UPDATE STATISTICS Proveedor.Pro_MedioContacto  
GO

