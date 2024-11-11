USE SIPE_PROVEEDOR
GO

--CATALOGO tbl_ActividadEconomica
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_ActividadEconomica')
Begin
	insert into Proveedor.Pro_Tabla (Tabla, TablaNombre, Estado) values (9898, 'tbl_ActividadEconomica', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9898 and Codigo = 'COD1')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9898, 'COD1', 'Act Econ 1', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9898 and Codigo = 'COD2')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9898, 'COD2', 'Act Econ 2', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9898 and Codigo = 'SFI')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9898, 'SFI', 'Servicios Financieros', 'A')
End



--CATALOGO tbl_TipoContrato
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_TipoContrato')
Begin
	insert into Proveedor.Pro_Tabla (Tabla, TablaNombre, Estado) values (9899, 'tbl_TipoContrato', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9899 and Codigo = 'CONTIPBIEN')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado, DescAlterno) values (9899, 'CONTIPBIEN', 'Bienes', 'A', '01')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9899 and Codigo = 'CONTIPSERV')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado, DescAlterno) values (9899, 'CONTIPSERV', 'Servicios', 'A', '02')
End

--CATALOGO tbl_TipoServicio
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_TipoServicio')
Begin
	insert into Proveedor.Pro_Tabla (Tabla, TablaNombre, Estado) values (9900, 'tbl_TipoServicio', 'A')
End

delete C
from SIPE_PROVEEDOR.Proveedor.Pro_Catalogo C
inner join SIPE_PROVEEDOR.Proveedor.Pro_Tabla T 
on C.Tabla = T.Tabla
where TablaNombre = 'tbl_TipoServicio'

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9900 and Codigo = 'CODTIP1')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9900, 'CODTIP1', 'Bienes', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9900 and Codigo = 'CODTIP2')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9900, 'CODTIP2', 'Servicios', 'A')
End

--CATALOGO tbl_PlazoSuscripcion
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_PlazoSuscripcion')
Begin
	insert into Proveedor.Pro_Tabla (Tabla, TablaNombre, Estado) values (9896, 'tbl_PlazoSuscripcion', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9896 and Codigo = 'CODPLAZO1')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9896, 'CODPLAZO1', '24', 'A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = 9896 and Codigo = 'CODPLAZO2')
Begin
	insert into Proveedor.Pro_Catalogo (Tabla, Codigo, Detalle, Estado) values (9896, 'CODPLAZO2', '48', 'A')
End

--CATALOGO tbl_FPagoContrato
declare @maxid int
select @maxid = MAX(Tabla) +1 from Proveedor.Pro_Tabla

delete from Proveedor.Pro_Catalogo where Tabla = (select tabla from Proveedor.Pro_Tabla where TablaNombre = 'tbl_FPagoContrato')
delete from Proveedor.Pro_Tabla where TablaNombre = 'tbl_FPagoContrato';
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_FPagoContrato')
Begin
	insert into Proveedor.Pro_Tabla(Tabla,TablaNombre,Estado) values (@maxid,'tbl_FPagoContrato','A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '1')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'1','CREDITO A CUENTA','A','TRANSFERENCIA A LA CUENTA')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '2')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'2','TARJETA DE CREDITO','A','ACREDITACION EN LA TJ')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '3')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'3','CHEQUE','A','PAGO CON CHEQUE')
End



--CONFIGURACION ACCIONES
if not exists (select top 1 1 from [Licitacion].[Lic_Accion] where IdAccion = 1)
Begin
	insert into [Licitacion].[Lic_Accion] (IdAccion, Nombre) values (1, 'GUARDAR')
End

if not exists (select top 1 1 from [Licitacion].[Lic_Accion] where IdAccion = 2)
Begin
	insert into [Licitacion].[Lic_Accion] (IdAccion, Nombre) values (2, 'GENERAR')
End

if not exists (select top 1 1 from [Licitacion].[Lic_Accion] where IdAccion = 3)
Begin
	insert into [Licitacion].[Lic_Accion] (IdAccion, Nombre) values (3, 'FIRMAR')
End

if not exists (select top 1 1 from [Licitacion].[Lic_Accion] where IdAccion = 4)
Begin
	insert into [Licitacion].[Lic_Accion] (IdAccion, Nombre) values (4, 'RECHAZAR')
End

--CONFIGURACION ESTADOS
if not exists (select top 1 1 from [Licitacion].[Lic_EstadosContrato] where IdEstado = 1)
Begin
	insert into [Licitacion].[Lic_EstadosContrato] (IdEstado, Nombre) values (1, 'PENDIENTE')
End

if not exists (select top 1 1 from [Licitacion].[Lic_EstadosContrato] where IdEstado = 2)
Begin
	insert into [Licitacion].[Lic_EstadosContrato] (IdEstado, Nombre) values (2, 'GENERADO')
End

if not exists (select top 1 1 from [Licitacion].[Lic_EstadosContrato] where IdEstado = 3)
Begin
	insert into [Licitacion].[Lic_EstadosContrato] (IdEstado, Nombre) values (3, 'FIRMADO PROVEEDOR')
End

if not exists (select top 1 1 from [Licitacion].[Lic_EstadosContrato] where IdEstado = 4)
Begin
	insert into [Licitacion].[Lic_EstadosContrato] (IdEstado, Nombre) values (4, 'FIRMADO GERENTE COMPRAS')
End

if not exists (select top 1 1 from [Licitacion].[Lic_EstadosContrato] where IdEstado = 5)
Begin
	insert into [Licitacion].[Lic_EstadosContrato] (IdEstado, Nombre) values (5, 'FIRMADO APODERADO')
End

--CONFIGURACION ROLES
if not exists (select top 1 1 from [Licitacion].[Lic_RolesWorkflow] where IdRol = 1)
Begin
	insert into [Licitacion].[Lic_RolesWorkflow] (IdRol, Nombre) values (1, 'COMPRAS')
End

if not exists (select top 1 1 from [Licitacion].[Lic_RolesWorkflow] where IdRol = 2)
Begin
	insert into [Licitacion].[Lic_RolesWorkflow] (IdRol, Nombre) values (2, 'GTECOMPRAS')
End

if not exists (select top 1 1 from [Licitacion].[Lic_RolesWorkflow] where IdRol = 3)
Begin
	insert into [Licitacion].[Lic_RolesWorkflow] (IdRol, Nombre) values (3, 'APODERADO1')
End

if not exists (select top 1 1 from [Licitacion].[Lic_RolesWorkflow] where IdRol = 4)
Begin
	insert into [Licitacion].[Lic_RolesWorkflow] (IdRol, Nombre) values (4, 'APODERADO1')
End


--CATALOGO tbl_TipoCuenta

select @maxid = MAX(Tabla) +1 from Proveedor.Pro_Tabla

delete from Proveedor.Pro_Catalogo where Tabla = (select tabla from Proveedor.Pro_Tabla where TablaNombre = 'tbl_TipoCuenta')
delete from Proveedor.Pro_Tabla where TablaNombre = 'tbl_TipoCuenta';
if not exists (select top 1 1 from Proveedor.Pro_Tabla where TablaNombre = 'tbl_TipoCuenta')
Begin
	insert into Proveedor.Pro_Tabla(Tabla,TablaNombre,Estado) values (@maxid,'tbl_TipoCuenta','A')
End

if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '01')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'01','Ahorros','A','1')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '02')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'02','Corriente','A','1')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '03')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'03','Visa','A','2')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '04')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'04','Mastercard','A','2')
End
if not exists (select top 1 1 from Proveedor.Pro_Catalogo where Tabla = @maxid and Codigo = '05')
Begin
	insert into Proveedor.Pro_Catalogo(Tabla,Codigo,Detalle,Estado,DescAlterno)
	values(@maxid,'05','Cheque','A','3')
End