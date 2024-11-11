use SIPE_PROVEEDOR
go

if exists(select 1
	from sysobjects 
	where name = 'Pro_ConPagos'
	and xtype = 'U'
)
begin
	drop table Proveedor.Pro_ConPagos
end
go

create table Proveedor.Pro_ConPagos
(
	Id int identity(1,1),
	TipoIdentificacion varchar(1) not null,
	Identificacion varchar(13) not null,
	CodProveedorAx varchar(40) not null,
	Factura varchar(200) not null,
	FormaPago varchar(5) not null,  --Catalogo
	FechaPago datetime not null,
	Valor numeric(13,2) not null,
	Detalle varchar(100) not null,
	FechaCreacion datetime not null,
	Primary key(Id)
)
go

create index idx1 on Proveedor.Pro_ConPagos(Identificacion)
create index idx2 on Proveedor.Pro_ConPagos(CodProveedorAx)
create index idx3 on Proveedor.Pro_ConPagos(Factura)
create index idx4 on Proveedor.Pro_ConPagos(FechaPago)
go

