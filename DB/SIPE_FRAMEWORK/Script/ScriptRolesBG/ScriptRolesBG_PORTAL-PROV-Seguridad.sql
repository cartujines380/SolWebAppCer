use SIPE_FRAMEWORK
go
--***************************************************
--* CREA Y HOMOLOGA ROL								*
--* 2022-07-22
--***************************************************

--PORTAL-PROV-Seguridad
select getdate() INICIO
go

--VARIABLES
Declare @v_idRol INT = 40
Declare @v_DescRol varchar(max) = 'PORTAL-PROV-Seguridad'


--HOMOLOGACION
if exists(
	select 1 from Seguridad.Seg_HomologacionRoles
	where IdRol = @v_idRol and CodAD = @v_DescRol and Estado = 'A'
)
begin
	delete from Seguridad.Seg_HomologacionRoles
	where IdRol = @v_idRol and CodAD = @v_DescRol and Estado = 'A'
end

if exists(
	select 1 from Seguridad.Seg_HomologacionRoles
	where IdRol = @v_idRol and CodAD = 'SUPERVISOR' and Estado = 'A'
)
begin
	delete from Seguridad.Seg_HomologacionRoles
	where IdRol = @v_idRol and CodAD = 'SUPERVISOR' and Estado = 'A'
end

--Se elimina para no generar duplicados y se pueda ejecutar n veces
delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = @v_idRol and IdOrganizacion = 39
and IdTransaccion in (2400, 2415, 2420, 2410, 215, 400, 401, 404, 406, 407, 408, 409, 410, 2200, 2250,
	2215, 2220, 2260, 2265, 2255, 107, 200, 201, 202, 203, 204, 205, 206, 207, 208, 
	209, 210, 211, 215, 218, 219, 300, 301, 302, 303, 304, 322, 323, 404, 405, 
	407, 408, 410, 415, 700, 708, 711, 713, 2600, 2610, 2, 3, 4, 5, 6,
	7, 4200, 4201, 4202, 4203, 1101, 1102, 1104, 414, 3100, 3105, 1104, 3200, 3201, 3202, 4400, 4401, 4402, 4403, 4301)

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = @v_idRol and IdOrganizacion = 1
and IdTransaccion in (78,147,300,301,902)

Delete from [Seguridad].[Seg_OpcionTransRol]
	where IdTransaccion in (4400,4401,4402,4403,4301)
	and IdRol != 40

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = @v_idRol and IdOrganizacion = 2
and IdTransaccion in (41)

delete from Seguridad.Seg_Rol 
where IdRol = @v_idRol
and IdEmpresa = 2 
and Status = 'ACTIVE'
go
--Fin de eliminado

Declare @v_idRol INT = 40
Declare @v_DescRol varchar(max) = 'PORTAL-PROV-Seguridad'



insert into sipe_framework.seguridad.Seg_Rol(IdRol,IdEmpresa,IdSucursal,Descripcion,Status,Nombre)
values(@v_idRol, 2  ,0,'Rol PORTAL-PROV-Seguridad','ACTIVE','PORTAL-PROV-Seguridad')

--CONTRATOS
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (@v_idRol,39,4400,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (@v_idRol,39,4401,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (@v_idRol,39,4402,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (@v_idRol,39,4403,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (@v_idRol,39,4301,1)


insert into Seguridad.Seg_HomologacionRoles(IdRol,CodAD,FechaCreacion,UsuarioCreacion,Estado)
values (@v_idRol, @v_DescRol ,getdate(),'USCRIPT','A')

insert into Seguridad.Seg_HomologacionRoles(IdRol,CodAD,FechaCreacion,UsuarioCreacion,Estado)
values (@v_idRol, 'SUPERVISOR' ,getdate(),'USCRIPT','A')
go


select getdate() FIN
go

update Seguridad.Seg_Transaccion
set Menu = 1
where IdTransaccion = 4400