use SIPE_PROVEEDOR
go

IF EXISTS (select 1 from sysobjects where name = 'Pro_ContactoAlmacen' and type='U')
begin
    drop table  Proveedor.Pro_ContactoAlmacen
    if exists (select 1 from sysobjects where name = 'Pro_ContactoAlmacen' and type = 'U')
      PRINT '<<< DROP TABLE Pro_ContactoAlmacen -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_ContactoAlmacen *OK* =='
end
GO

CREATE TABLE Proveedor.Pro_ContactoAlmacen(
	IdContAlm  int NOT NULL Identity,
	IdContacto  int, 
	CodAlmacen varchar(3),
	CodPais varchar(3),
	CodCiudad varchar(20),
	CodRegion varchar(3),
	Estado char(1),
	FechaRegistro datetime
)
GO

if exists(select 1 from sysobjects where name='Pro_ContactoAlmacen' and type = 'U')
  PRINT '== CREATE TABLE Pro_ContactoAlmacen *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_ContactoAlmacen -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_ContactoAlmacen ADD PRIMARY KEY (IdContAlm)
ALTER TABLE Proveedor.Pro_ContactoAlmacen ADD FOREIGN KEY (IdContacto) REFERENCES Proveedor.Pro_ContactoProveedor(IdContacto);
CREATE INDEX ix_Pro_ContactoAlmacen_02 ON Proveedor.Pro_ContactoAlmacen (FechaRegistro)

GO

UPDATE STATISTICS Proveedor.Pro_ContactoAlmacen
GO
