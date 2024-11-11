use SIPE_FRAMEWORK
go

--GTECOMPRAS
select getdate() INICIO
go

--HOMOLOGACION
if exists(
	select 1 from Seguridad.Seg_HomologacionRoles
	where IdRol = 38 and CodAD = 'APODERADO1' and Estado = 'A'
)
begin
	delete from Seguridad.Seg_HomologacionRoles
	where IdRol = 38 and CodAD = 'APODERADO1' and Estado = 'A'
end

--Se elimina para no generar duplicados y se pueda ejecutar n veces
delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 38 and IdOrganizacion = 39
and IdTransaccion in (2400, 2415, 2420, 2410, 215, 400, 401, 404, 406, 407, 408, 409, 410, 2200, 2250,
	2215, 2220, 2260, 2265, 2255, 107, 200, 201, 202, 203, 204, 205, 206, 207, 208, 
	209, 210, 211, 215, 218, 219, 300, 301, 302, 303, 304, 322, 323, 404, 405, 
	407, 408, 410, 415, 700, 708, 711, 713, 2600, 2610, 2, 3, 4, 5, 6,
	7, 4200, 4201, 4202, 4203, 1101, 1102, 1104, 414, 3100, 3105, 1104, 3200, 3201, 3202)

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 38 and IdOrganizacion = 1
and IdTransaccion in (78,147,300,301,902)

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 38 and IdOrganizacion = 2
and IdTransaccion in (41)

delete from Seguridad.Seg_Rol 
where IdRol = 38 
and IdEmpresa = 2 
and Status = 'ACTIVE'
go
--Fin de eliminado

insert into sipe_framework.seguridad.Seg_Rol(IdRol,IdEmpresa,IdSucursal,Descripcion,Status,Nombre)
values(38,2,0,'Rol Apoderado 1','ACTIVE','Rol Apoderado 1')
go

--CONTRATOS
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (38,39,1104,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (38,39,3200,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (38,39,3201,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (38,39,3202,1)
go

insert into Seguridad.Seg_HomologacionRoles(IdRol,CodAD,FechaCreacion,UsuarioCreacion,Estado)
values (38,'APODERADO1',getdate(),'USCRIPT','A')
go


select getdate() FIN
go
