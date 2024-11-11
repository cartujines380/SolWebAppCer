use SIPE_PROVEEDOR
go

IF EXISTS (select 1 from sysobjects where name = 'Pro_ContactoProveedor' and type='U')
begin
    drop table  Proveedor.Pro_ContactoProveedor
    if exists (select 1 from sysobjects where name = 'Pro_ContactoProveedor' and type = 'U')
      PRINT '<<< DROP TABLE Pro_ContactoProveedor -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_ContactoProveedor *OK* =='
end
GO

CREATE TABLE Proveedor.Pro_ContactoProveedor(
	IdContacto  int NOT NULL Identity,
	CodProveedor  int, 
	TipoIdentificacion varchar(2), 
	Identificacion varchar(20),  
	Nombre1 varchar(60),  
	Nombre2 varchar(60),  
	Apellido1 varchar(60),  
	Apellido2 varchar(60),  
	Prefijo varchar(12),
	Estado char(1),
	TelfFijo varchar(10),
	TelfFijoEXT  varchar(5),
	TelfMovil  varchar(10),
	email varchar(60),
	NotElectronica	smallint ,
	NotTransBancaria smallint ,
	RecActas smallint,
	RepLegal smallint,
	FechaRegistro datetime,
	FechaActualizacion datetime
)
GO

if exists(select 1 from sysobjects where name='Pro_ContactoProveedor' and type = 'U')
  PRINT '== CREATE TABLE Pro_ContactoProveedor *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_ContactoProveedor -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_ContactoProveedor ADD PRIMARY KEY (IdContacto)
CREATE INDEX ix_Pro_ContactoProveedor_01 ON Proveedor.Pro_ContactoProveedor (CodProveedor)
CREATE INDEX ix_Pro_ContactoProveedor_02 ON Proveedor.Pro_ContactoProveedor (Identificacion)
CREATE INDEX ix_Pro_ContactoProveedor_03 ON Proveedor.Pro_ContactoProveedor (Apellido1,Apellido2)
CREATE INDEX ix_Pro_ContactoProveedor_04 ON Proveedor.Pro_ContactoProveedor (Nombre1,Nombre2)

GO

UPDATE STATISTICS Proveedor.Pro_ContactoProveedor  
GO

