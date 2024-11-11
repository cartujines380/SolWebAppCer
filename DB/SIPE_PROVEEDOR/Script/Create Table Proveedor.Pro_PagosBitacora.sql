use SIPE_PROVEEDOR
go

if exists(select 1
	from sysobjects 
	where name = 'Pro_PagosBitacora'
	and xtype = 'U'
)
begin
	drop table Proveedor.Pro_PagosBitacora
end
go

create table Proveedor.Pro_PagosBitacora(
	Id int identity(1,1),
	Proceso varchar(25),
	FechaRegistro datetime,
	Servicio varchar(25),
	Detalle varchar(5000),
	Accion varchar(5),
	Estado varchar(1),
	Primary Key (Id)
)

go

create index idx_fecha_registro on Proveedor.Pro_PagosBitacora(FechaRegistro)
create index idx_detalle on Proveedor.Pro_PagosBitacora(Detalle)
go

