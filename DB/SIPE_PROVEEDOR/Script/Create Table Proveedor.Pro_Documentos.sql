use SIPE_PROVEEDOR
go

if exists(
	select 1 
	from sysobjects 
	where name = 'Pro_Documentos' 
	and xtype = 'U'
)
begin
	drop table Proveedor.Pro_Documentos
end
go

create table Proveedor.Pro_Documentos
(
IdDocumentos		int identity(1,1),
CodTipoPersona		varchar(5) NOT NULL, --Catalogo 1032
Codigo				varchar(5) NOT NULL, --Codigo de dos caracteres de cada documento
Descripcion			varchar(200) NOT NULL,
EsObligatorio		varchar(1) NOT NULL,
FechaRegistro		datetime NOT NULL,
UsuarioCreacion		varchar(20) NULL,
FechaModificacion	datetime NULL,
UsuarioModificacion varchar(20) NULL,
Estado				varchar(5) NOT NULL,
PRIMARY KEY(IdDocumentos)
)
go

create index idx_Pro_Documentos_01 on Proveedor.Pro_Documentos(CodTipoPersona)
create index idx_Pro_Documentos_02 on Proveedor.Pro_Documentos(Codigo)
go

