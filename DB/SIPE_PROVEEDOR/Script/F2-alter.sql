USE SIPE_PROVEEDOR
GO

--MODIFICACION DE TABLAS

if exists(
	SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME = 'Pro_Sociedad' 
	AND COLUMN_NAME in ('Direccion','Locacion','Correo','Telefono')
)
begin
	select 'Ya existe el campo' as Salida
end
else
begin
	alter table Proveedor.Pro_Sociedad
	add CodActividadEconomica varchar(20),
		Direccion varchar(100),
		Locacion varchar(35),
		Correo varchar(241),
		Telefono varchar(40)
end 


if exists(
	SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME = 'Pro_Proveedor' 
	AND COLUMN_NAME in ('CodClaseContribuyente','Extension')
)
begin
	select 'Ya existe el campo' as Salida
end
else
begin
	alter table Proveedor.Pro_Proveedor
	add CodClaseContribuyente varchar(10),
		Extension varchar(4)
end 

if exists(
	SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME = 'Pro_ProveedorDetalle' 
	AND COLUMN_NAME = 'CodActividadEconomica'
)
begin
	select 'Ya existe el campo' as Salida
end
else
begin
	alter table Proveedor.Pro_ProveedorDetalle
	add CodActividadEconomica varchar(20)
end 


--Lic_ContratoLicitacion
if exists(select 1 from sysobjects 
	where name = 'DF__Lic_Contr__CodFo__7775B2CE')
begin
	ALTER TABLE Licitacion.Lic_ContratoLicitacion
	DROP CONSTRAINT DF__Lic_Contr__CodFo__7775B2CE
end

if exists(
	SELECT 1 FROM   INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME = 'Lic_ContratoLicitacion' 
	AND COLUMN_NAME = 'CodFormaPago'
)
begin
	update Licitacion.Lic_ContratoLicitacion
	set CodFormaPago = null

	alter table Licitacion.Lic_ContratoLicitacion 
	drop column CodFormaPago 
end
