use SIPE_FRAMEWORK
go

--COMPRAS
select getdate() INICIO
go

--HOMOLOGACION
if exists(
	select 1 from Seguridad.Seg_HomologacionRoles
	where IdRol = 36 and CodAD = 'COMPRAS' and Estado = 'A'
)
begin
	delete from Seguridad.Seg_HomologacionRoles
	where IdRol = 36 and CodAD = 'COMPRAS' and Estado = 'A'
end
go


--Se elimina para no generar duplicados y se pueda ejecutar n veces
delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 36 and IdOrganizacion = 39
and IdTransaccion in (2400, 2415, 2420, 2410, 215, 400, 401, 404, 406, 407, 408, 409, 410, 2200, 2250,
	2215, 2220, 2260, 2265, 2255, 107, 200, 201, 202, 203, 204, 205, 206, 207, 208, 
	209, 210, 211, 215, 218, 219, 300, 301, 302, 303, 304, 322, 323, 404, 405, 
	407, 408, 410, 415, 700, 708, 711, 713, 2600, 2610, 2, 3, 4, 5, 6,
	7, 4200, 4201, 4202, 4203, 1101, 1102, 1104, 414, 3100, 3105, 1104, 3200, 3201, 3202)

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 36 and IdOrganizacion = 1
and IdTransaccion in (78,147,300,301,902)

delete from [Seguridad].[Seg_OpcionTransRol] where IdRol = 36 and IdOrganizacion = 2
and IdTransaccion in (41)

delete from Seguridad.Seg_Rol 
where IdRol = 36 
and IdEmpresa = 2 
and Status = 'ACTIVE'
go
--Fin de eliminado

insert into Seguridad.Seg_Rol(IdRol,IdEmpresa,IdSucursal,Descripcion,Status,Nombre)
values(36,2,0,'Rol Compras','ACTIVE','Rol Compras')
go

--GENERAL
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,1,78,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,1,147,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,1,300,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,1,301,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,1,902,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,2,41,1)
go

--NOTIFICACIONES
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2400,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2415,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2420,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2410,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,215,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,400,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,401,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,404,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,406,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,407,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,408,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,409,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,410,1)
go

--PROVEEDORES
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2200,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2250,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2215,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2220,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2260,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2265,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2255,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,107,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,200,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,201,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,202,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,203,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,204,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,205,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,206,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,207,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,208,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,209,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,210,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,211,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,218,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,219,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,300,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,301,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,302,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,303,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,304,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,322,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,323,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,405,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,415,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,700,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,708,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,711,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,713,1)
go

--ACCESO DE USUARIOS
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2600,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2610,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,2,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,4,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,5,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,6,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,7,1)
go

--LICITACION
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,4200,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,4201,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,4202,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,4203,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,1101,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,1102,1)
go

--PAGOS
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,414,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3100,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3105,1)
go

--CONTRATOS
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,1104,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3200,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3201,1)
INSERT [Seguridad].[Seg_OpcionTransRol] ([IdRol], [IdOrganizacion], [IdTransaccion], [IdOpcion]) VALUES (36,39,3202,1)
go


insert into Seguridad.Seg_HomologacionRoles(IdRol,CodAD,FechaCreacion,UsuarioCreacion,Estado)
values (36,'COMPRAS',getdate(),'USCRIPT','A')
go

select getdate() FIN
go