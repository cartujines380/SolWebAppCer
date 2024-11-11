use SIPE_PROVEEDOR
go

IF EXISTS (select 1 from sysobjects where name = 'Pro_ContactoDepartamento' and type='U')
begin
    drop table  Proveedor.Pro_ContactoDepartamento
    if exists (select 1 from sysobjects where name = 'Pro_ContactoDepartamento' and type = 'U')
      PRINT '<<< DROP TABLE Pro_ContactoDepartamento -- ERROR -- >>>'
    else
      PRINT '== DROP TABLE Pro_ContactoDepartamento *OK* =='
end
GO

CREATE TABLE Proveedor.Pro_ContactoDepartamento(
	IdContDep  int NOT NULL Identity,
	IdContacto  int,
	Identificacion varchar(20),
	CodDepartamento varchar(5),
	CodFuncion varchar(5),
	Estado int
)
GO

if exists(select 1 from sysobjects where name='Pro_ContactoDepartamento' and type = 'U')
  PRINT '== CREATE TABLE Pro_ContactoDepartamento *OK* =='
 else
  PRINT '<<< CREATE TABLE Pro_ContactoDepartamento -- ERROR -- >>>'
GO

ALTER TABLE Proveedor.Pro_ContactoDepartamento ADD PRIMARY KEY (IdContDep)
ALTER TABLE Proveedor.Pro_ContactoDepartamento ADD FOREIGN KEY (IdContacto) REFERENCES Proveedor.Pro_ContactoProveedor(IdContacto);
CREATE INDEX ix_Pro_ContactoDepartamento_01 ON Proveedor.Pro_ContactoDepartamento (Identificacion)
GO

UPDATE STATISTICS Proveedor.Pro_ContactoDepartamento  
GO

