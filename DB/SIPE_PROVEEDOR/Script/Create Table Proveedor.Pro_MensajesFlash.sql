use SIPE_PROVEEDOR
go

if exists(select 1
	from sysobjects 
	where name = 'Pro_MensajesFlash'
	and xtype = 'U'
)
begin
	drop table Proveedor.Pro_MensajesFlash
end
go

create table Proveedor.Pro_MensajesFlash
(
	Id int identity(1,1),
	Identificacion varchar(13) not null,
	Titulo varchar(50) not null,
	Mensaje varchar(8000) not null,
	Estado varchar(1) not null,
	FechaCreacion datetime not null,
	Primary key(Id)
)
go

create index idx1 on Proveedor.Pro_MensajesFlash(Identificacion)
create index idx2 on Proveedor.Pro_MensajesFlash(FechaCreacion)
go

