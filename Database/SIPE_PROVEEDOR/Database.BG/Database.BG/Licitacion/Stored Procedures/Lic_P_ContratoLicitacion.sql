USE [SIPE_PROVEEDOR]
GO
/****** Object:  StoredProcedure [Licitacion].[Lic_P_ContratoLicitacion]    Script Date: 2/6/2022 15:32:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER proc [Licitacion].[Lic_P_ContratoLicitacion] (
	@PI_ParamXML xml
)
AS
BEGIN

	BEGIN TRY
	
		DECLARE	@tipo					int,
				@idRol					int,
				@idContrato				int,
				@codTipoContrato		varchar(20),
				@codLineaNegocio		varchar(20),
				@codTipoServicio		varchar(20),
				@codPlazoSuscripcion	varchar(20),
				@administradorContrato	varchar(100),
				@idAdquisicion			bigint,
				@nombreLicitacion		varchar(40),
				@numContrato			varchar(8),
				@anioContrato			int,
				@mesContrato			int,
				@diaContrato			int,
				@tipoCont				int,
				@secuencialContrato		int,
				@fechaActual			datetime,
				@rutaFisicaArchivo		varchar(2000),
				@nombreArchivo			varchar(25),
				@usuario				varchar(20),
				@errorMessage			varchar(1000),
				@errorSeverity			int,
				@errorState				int,
				@idEstado               int,
				@accionRechazado        int

		SELECT  @tipo					= nref.value('@tipo','int'),
				@idContrato				= nref.value('@idContrato','int'),
				@codTipoContrato		= nref.value('@codTipoContrato','varchar(20)'),
				@codLineaNegocio		= nref.value('@codLineaNegocio','varchar(20)'),
				@codTipoServicio		= nref.value('@codTipoServicio','varchar(20)'),
				@codPlazoSuscripcion	= nref.value('@codPlazoSuscripcion','varchar(20)'),
				@administradorContrato	= nref.value('@administradorContrato','varchar(100)'),
				@idAdquisicion			= nref.value('@idAdquisicion','bigint'),
				@nombreLicitacion		= nref.value('@nombreLicitacion','varchar(40)'),
				@usuario				= nref.value('@usuario','varchar(20)'),
				@idEstado				= nref.value('@idEstado','int')
		FROM @PI_ParamXML.nodes('/Root') as item(nref)

		--TIPO 1 = Consulta bandeja
		If @tipo='1'
		Begin			
			--Se identifica el idRol segun el rol AD asignado
			select @idRol = RW.IdRol
			FROM @PI_ParamXML.nodes('/Root/Rol') as item(nref)
			inner join Licitacion.Lic_RolesWorkflow RW with (nolock) on RW.Nombre = nref.value('@nombre','varchar(50)')

			--Consulta General
			select @idRol = case when nref.value('@nombre','varchar(50)') = 'T' then 0 else @idRol end
			FROM @PI_ParamXML.nodes('/Root/Rol') as item(nref)

			if @idRol = 0 --CONSULTA DE CONTRATOS
			begin
				select	RA.idAdquisicion As IdAdquisicion,
						RA.nombrePub As NombreLic,
						RA.descripcionPub As DescripcionLic,
						PP.Ruc As Ruc,
						PP.NomComercial As RazonSocial,
						CL.NumeroContrato AS NumContrato,
						CL.IdEstado AS IdEstado,
						CASE WHEN CL.IdEstado IS NULL THEN 'PENDIENTE' ELSE 
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 0 THEN EC.Nombre + ' 1' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 0 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 2' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 1 y 2' 
						ELSE EC.Nombre END END END END AS DesEstado,
						CL.NombreArchivo
				from Pedidos.Red_Adquisicion RA with (nolock)
				inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
				inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
				left join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion and CL.IdEstado in (2,3,4,5)
				left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
				where RA.estadoparticipandp = 'A'

			end
			Else If  @idRol = 1 --LISTADO DE LICITACIONES PARA COMPRAS
			Begin

				select	RA.idAdquisicion As IdAdquisicion,
						RA.nombrePub As NombreLic,
						RA.descripcionPub As DescripcionLic,
						PP.Ruc As Ruc,
						PP.NomComercial As RazonSocial,
						CL.NumeroContrato AS NumContrato,
						CL.IdEstado AS IdEstado,
						CASE WHEN CL.IdEstado IS NULL THEN 'PENDIENTE' 
							 ELSE EC.Nombre END 
						AS DesEstado,
						CL.NombreArchivo
				from Pedidos.Red_Adquisicion RA with (nolock)
				inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
				inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
				left join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion
				left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
				where RA.estadoparticipandp = 'A'

			End
			Else If @idRol = 2 --LISTADO DE LICITACIONES PARA GERENTE COMPRAS
			Begin

				select	RA.idAdquisicion As IdAdquisicion,
						RA.nombrePub As NombreLic,
						RA.descripcionPub As DescripcionLic,
						PP.Ruc As Ruc,
						PP.NomComercial As RazonSocial,
						CL.NumeroContrato AS NumContrato,
						CL.IdEstado AS IdEstado,
						EC.Nombre AS DesEstado,
						CL.NombreArchivo
				from Pedidos.Red_Adquisicion RA with (nolock)
				inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
				inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
				inner join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion and CL.IdEstado in (3)
				left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
				where RA.estadoparticipandp = 'A'

			End
			Else If @idRol = 3 --LISTADO DE LICITACIONES PARA APODERADOS 1
			Begin

				select	RA.idAdquisicion As IdAdquisicion,
						RA.nombrePub As NombreLic,
						RA.descripcionPub As DescripcionLic,
						PP.Ruc As Ruc,
						PP.NomComercial As RazonSocial,
						CL.NumeroContrato AS NumContrato,
						CL.IdEstado AS IdEstado,
						CASE WHEN CL.IdEstado IS NULL THEN 'PENDIENTE' ELSE 
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 0 THEN EC.Nombre + ' 1' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 0 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 2' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 1 y 2' 
						ELSE EC.Nombre END END END END AS DesEstado,
						CL.NombreArchivo
				from Pedidos.Red_Adquisicion RA with (nolock)
				inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
				inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
				inner join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion and CL.IdEstado in (4,5) and CL.AprobacionApoderado1 = 0
				left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
				where RA.estadoparticipandp = 'A'

			End
			Else If @idRol = 4 --LISTADO DE LICITACIONES PARA APODERADOS 2
			Begin

				select	RA.idAdquisicion As IdAdquisicion,
						RA.nombrePub As NombreLic,
						RA.descripcionPub As DescripcionLic,
						PP.Ruc As Ruc,
						PP.NomComercial As RazonSocial,
						CL.NumeroContrato AS NumContrato,
						CL.IdEstado AS IdEstado,
						CASE WHEN CL.IdEstado IS NULL THEN 'PENDIENTE' ELSE 
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 0 THEN EC.Nombre + ' 1' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 0 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 2' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 1 y 2' 
						ELSE EC.Nombre END END END END AS DesEstado,
						CL.NombreArchivo
				from Pedidos.Red_Adquisicion RA with (nolock)
				inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
				inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
				inner join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion and CL.IdEstado in (4,5) and CL.AprobacionApoderado2 = 0
				left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
				where RA.estadoparticipandp = 'A'

			End

		End

		--TIPO 2 = Listado de licitaciones para proveedores
		If @tipo='2'
		Begin
			if (@usuario = 'T')
			begin
				set @usuario = null
			end

			select	RA.idAdquisicion As IdAdquisicion,
					RA.nombrePub As NombreLic,
					RA.descripcionPub As DescripcionLic,
					PP.Ruc As Ruc,
					PP.NomComercial As RazonSocial,
					CL.NumeroContrato AS NumContrato,
					CL.IdEstado AS IdEstado,
					CASE WHEN CL.IdEstado IS NULL THEN 'PENDIENTE' ELSE 
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 0 THEN EC.Nombre + ' 1' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 0 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 2' ELSE
						CASE WHEN CL.AprobacionApoderado1 = 1 and CL.AprobacionApoderado2 = 1 THEN EC.Nombre + ' 1 y 2' 
						ELSE EC.Nombre END END END END AS DesEstado,
					CL.NombreArchivo
			from Pedidos.Red_Adquisicion RA with (nolock)
			inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
			inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
			inner join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion and CL.IdEstado in (2,3,4,5)
			left join Licitacion.Lic_EstadosContrato EC with (nolock) on EC.IdEstado = CL.IdEstado
			where PP.Ruc = isnull(@usuario,PP.Ruc)
			and RA.estadoparticipandp = 'A'

		End

		--TIPO 3 = Consulta detalle licitacion
		If @tipo='3'
		Begin

			select	S.NombreSociedad,
					S.RucSociedad,
					S.RepresentanteLegal,
					C.Detalle As DesActividadEconomica,
					S.Direccion,
					S.Locacion,
					S.Correo,
					S.Telefono
			from Proveedor.Pro_Sociedad S
			left join Proveedor.Pro_Catalogo C with (nolock) 
			on C.Codigo = S.CodActividadEconomica 
			and C.Estado = 'A' and C.Tabla = 9898
			where S.Estado = 'A'
			and S.IdSociedad = 7777
			
			select	RA.nombrePub As NombreLic,
					PP.NomComercial As Sociedad,
					PP.Ruc,
					PC.Identificacion As IdentificacionRepLegal,
					(isnull(Nombre1,'') + ' ' + isnull(Nombre2,'') + ' '+ isnull(Apellido1,'') + ' '+ isnull(Apellido2,'')) As RepLegal,
					C0.Detalle As Cargo,
					C1.Detalle As DesPersonalidad,
					C2.Detalle As DesActividadEconomica,
					ISNULL(PP.DirCallePrinc, '') + ' ' + ISNULL(PP.DirCalleNum, '') + ' ' + ISNULL(PP.DirPisoEdificio, '') As Direccion,
					RA.monto As ValorContrato,
					PP.CorreoE As Correo,
					PP.Poblacion As Locacion,
					C3.Detalle As DesPais,
					PP.Telefono As Telefono,
					PP.Extension As Extension,
					ISNULL(CL.IdContrato, '0') IdContrato,
					CL.CodTipoContrato,
					CL.CodLineaNegocio,
					CL.CodTipoServicio,
					CL.CodPlazoSuscripcion,
					CL.AdministradorContrato,
					C1.DescAlterno As MotivoActividadComentario,
					C4.DescAlterno As FormaPagoComentario,
					C5.Detalle AS FormaPagoTipo,
					FP.NumeroCuenta As NumeroCuenta
			from Pedidos.Red_Adquisicion RA with (nolock)
			inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
			left join Proveedor.Pro_ContactoProveedor PC with (nolock) on PC.CodProveedor = RA.codproveedor and PC.RepLegal = 1
			left join Proveedor.Pro_ContactoDepartamento CD with (nolock) on CD.IdContacto = PC.IdContacto
			left join Proveedor.Pro_Catalogo C0 with (nolock) on C0.Codigo = CD.CodFuncion and C0.Estado = 'A' 
			inner join Proveedor.Pro_Tabla T0 with (nolock) on T0.Tabla = C0.Tabla and T0.TablaNombre = 'tbl_FuncionContacto'
			left join Proveedor.Pro_Catalogo C1 with (nolock) on C1.Codigo = PP.CodClaseContribuyente and C1.Estado = 'A'
			inner join Proveedor.Pro_Tabla T1 with (nolock) on T1.Tabla = C1.Tabla and T1.TablaNombre = 'tbl_ClaseImpuesto'
			left join Proveedor.Pro_ProveedorDetalle PD with (nolock) on PD.IdProveedor = PP.CodProveedor or PD.CodProveedor = PP.CodProveedorBG
			left join Proveedor.Pro_Catalogo C2 with (nolock) on C2.Codigo = PD.CodActividadEconomica and C2.Estado = 'A' 
			inner join Proveedor.Pro_Tabla T2 with (nolock) on T2.Tabla = C2.Tabla and T2.TablaNombre = 'tbl_tipoActividad'
			left join Proveedor.Pro_Catalogo C3 with (nolock) on C3.Codigo = PP.Pais and C3.Estado = 'A' 
			inner join Proveedor.Pro_Tabla T3 with (nolock) on T3.Tabla = C3.Tabla and T3.TablaNombre = 'tbl_Pais'
			left join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion
			left join Proveedor.Pro_ProveedorFrmPago FP with (nolock) on FP.CodProveedor = PP.CodProveedor and Principal = 1
			left join Proveedor.Pro_Catalogo C4 with (nolock) on C4.Codigo = FP.FormaPago and C4.Estado = 'A'
			inner join Proveedor.Pro_Tabla T4 with (nolock) on T4.Tabla = C4.Tabla and T4.TablaNombre = 'tbl_FPagoContrato'
			left join Proveedor.Pro_Catalogo C5 with (nolock) on C5.Codigo = FP.TipoCuenta and C5.Estado = 'A'
			inner join Proveedor.Pro_Tabla T5 with (nolock) on T5.Tabla = C5.Tabla and T5.TablaNombre = 'tbl_TipoCuenta'
			where RA.idAdquisicion = @idAdquisicion

		End

		--TIPO 4 = Grabado parcial campos editables contrato
		If @tipo='4'
		Begin
			
			BEGIN TRANSACTION

			If exists(select top 1 1 from Licitacion.Lic_ContratoLicitacion CL with (nolock) where CL.IdContrato = @idContrato)
			Begin
				
				update CL
				set CodTipoContrato = @codTipoContrato,
					CodLineaNegocio = @codLineaNegocio,
					CodTipoServicio = @codTipoServicio,
					CodPlazoSuscripcion = @codPlazoSuscripcion,
					AdministradorContrato = @administradorContrato,
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @usuario
				from Licitacion.Lic_ContratoLicitacion CL
				where CL.IdContrato = @idContrato

			End
			Else
			Begin
				
				insert into Licitacion.Lic_ContratoLicitacion
				(
					IdAdquisicion,
					NombreLicitacion,
					CodTipoContrato,
					CodLineaNegocio,
					CodTipoServicio,
					CodPlazoSuscripcion,
					AdministradorContrato,
					IdEstado,
					FechaCreacion,
					UsuarioCreacion
				)
				values
				(
					@idAdquisicion,
					@nombreLicitacion,
					@codTipoContrato,
					@codLineaNegocio,
					@codTipoServicio,
					@codPlazoSuscripcion,
					@administradorContrato,
					1,
					GETDATE(),
					@usuario
				)

				set @idContrato = @@IDENTITY

			End

			insert into Licitacion.Lic_LogFlujoContrato
			(
				IdAccion,
				IdContrato,
				IdEstadoContrato,
				UsuarioRegistro,
				FechaRegistro
			)
			values
			(
				1,
				@idContrato,
				1,
				@usuario,
				GETDATE()
			)

			select @idContrato IdContrato

		End

		--TIPO 5 = Genera contrato
		If @tipo='5'
		Begin
			
			BEGIN TRANSACTION

			set @fechaActual = GETDATE()

			select	@anioContrato = YEAR(@fechaActual),
					@mesContrato = MONTH(@fechaActual),
					@diaContrato = DAY(@fechaActual)

			select @tipoCont = substring(C.DescAlterno,1,2)
			from Proveedor.Pro_Catalogo C
			inner join Proveedor.Pro_Tabla T 
			on C.Tabla = T.Tabla
			where T.TablaNombre = 'tbl_TipoContrato'
			and C.Codigo = @codTipoContrato
			and C.Estado = 'A'
			and T.Estado = 'A'

			select @secuencialContrato = ISNULL(MAX(CL.Secuencial), 0) + 1
			from Licitacion.Lic_ContratoLicitacion CL
			where CL.Anio = @anioContrato

			if @secuencialContrato is null
			begin
				select @secuencialContrato = isnull(@secuencialContrato,0) + 1
			end 

			select	@numContrato = CONVERT(VARCHAR, YEAR(@fechaActual)) + '-' + RIGHT('000' + CONVERT(VARCHAR, @secuencialContrato), 3)

			select @rutaFisicaArchivo = @numContrato + '\'

			select	@nombreArchivo = CONVERT(VARCHAR, YEAR(@fechaActual)) + '-' +
					RIGHT('00' + CONVERT(VARCHAR, MONTH(@fechaActual)), 2) + '-' +
					RIGHT('00' + CONVERT(VARCHAR, DAY(@fechaActual)), 2) + '-' +
					CONVERT(VARCHAR,@tipoCont) + '-' +
					RIGHT('000' + CONVERT(VARCHAR, @secuencialContrato), 3) +
					'.pdf'

			If exists(select top 1 1 from Licitacion.Lic_ContratoLicitacion CL with (nolock) where CL.IdContrato = @idContrato)
			Begin
				
				update CL
				set NumeroContrato = @numContrato,
					Anio = @anioContrato,
					Mes = @mesContrato,
					Dia = @diaContrato,
					TipoContrato = @tipoCont,
					Secuencial = @secuencialContrato,
					CodTipoContrato = @codTipoContrato,
					CodLineaNegocio = @codLineaNegocio,
					CodTipoServicio = @codTipoServicio,
					CodPlazoSuscripcion = @codPlazoSuscripcion,
					AdministradorContrato = @administradorContrato,
					IdEstado = 2,
					RutaFisicaArchivo = @rutaFisicaArchivo,
					NombreArchivo = @nombreArchivo,
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @usuario,
					FechaGeneracion = GETDATE(),
					UsuarioGeneracion = @usuario
				from Licitacion.Lic_ContratoLicitacion CL
				where CL.IdContrato = @idContrato

			End
			Else
			Begin
				
				insert into Licitacion.Lic_ContratoLicitacion
				(
					NumeroContrato,
					Anio,
					Mes,
					Dia,
					TipoContrato,
					Secuencial,
					IdAdquisicion,
					NombreLicitacion,
					CodTipoContrato,
					CodLineaNegocio,
					CodTipoServicio,
					CodPlazoSuscripcion,
					AdministradorContrato,
					IdEstado,
					RutaFisicaArchivo,
					NombreArchivo,
					FechaCreacion,
					UsuarioCreacion,
					FechaGeneracion,
					UsuarioGeneracion
				)
				values
				(
					@numContrato,
					@anioContrato,
					@mesContrato,
					@diaContrato,
					@tipoCont,
					@secuencialContrato,
					@idAdquisicion,
					@nombreLicitacion,
					@codTipoContrato,
					@codLineaNegocio,
					@codTipoServicio,
					@codPlazoSuscripcion,
					@administradorContrato,
					1,
					@rutaFisicaArchivo,
					@nombreArchivo,
					GETDATE(),
					@usuario,
					GETDATE(),
					@usuario
				)

				set @idContrato = @@IDENTITY

			End

			insert into Licitacion.Lic_LogFlujoContrato
			(
				IdAccion,
				IdContrato,
				IdEstadoContrato,
				UsuarioRegistro,
				FechaRegistro
			)
			values
			(
				2,
				@idContrato,
				2,
				@usuario,
				GETDATE()
			)

			--Se registra para el visor de notificaciones para el proveedor
			insert into Proveedor.Pro_MensajesFlash(
				Identificacion, Titulo, Mensaje, Estado, FechaCreacion
			)
			select PP.Ruc,'Nuevo Contrato registrado','Se ha realizado un nuevo contrato ['+@numContrato+']. Verifique en la opción Contratos | Generación de contratos',
					'I', getdate()
			from Pedidos.Red_Adquisicion RA with (nolock)
			inner join Proveedor.Pro_Proveedor PP with (nolock) on PP.CodProveedor = RA.codproveedor
			inner join Pedidos.Red_Requerimientos RR with (nolock) on RR.idrequerimiento = RA.idrequerimiento and RR.estado = 'A'
			left join Licitacion.Lic_ContratoLicitacion CL with (nolock) on CL.IdAdquisicion = RA.idAdquisicion
			where CL.IdContrato = @idContrato
			and RA.estadoparticipandp = 'A' 


			select	@rutaFisicaArchivo As RutaFisicaArchivo,
					@nombreArchivo As NombreArchivo

		End

		If @tipo='6'
		Begin
			if @idEstado = 2
				set @accionRechazado = 4
			else
				set @accionRechazado = 3

			select @idContrato = IdContrato 
			from Licitacion.Lic_ContratoLicitacion
			where IdAdquisicion = @idAdquisicion
			
			if (exists(select 1
						from Licitacion.Lic_ContratoLicitacion 
						where IdContrato = @idContrato
						and IdEstado = @idEstado 
						and UsuarioModificacion = @usuario)
				and @idEstado = 5
			
			)
			begin
				select	9999 CodError,
						'Contrato ya tiene aprobación de Apoderado ' + @usuario MsgError
				return 1
			end

			if exists(select 1
						from Licitacion.Lic_ContratoLicitacion 
						where IdContrato = @idContrato
						and IdEstado = @idEstado
						and @idEstado <> 5 and @idEstado <> 2
			)
			begin
				select	9999 CodError,
						'Contrato ya fue Aprobado - ' + @usuario MsgError
				return 1
			end

			begin transaction
			declare @fap char(1)
			if @idEstado = 5
			begin
				select  @fap = substring(@usuario,1,1),
						@usuario = substring(@usuario,2,len(@usuario))

				if @fap = 1
				begin
					update Licitacion.Lic_ContratoLicitacion
					set IdEstado = @idEstado,
						FechaModificacion = GETDATE(),
						AprobacionApoderado1 = 1,
						UsuarioModificacion = @usuario
					where IdContrato = @idContrato 
					and IdAdquisicion = @idAdquisicion
				end
				else
				begin
					update Licitacion.Lic_ContratoLicitacion
					set IdEstado = @idEstado,
						FechaModificacion = GETDATE(),
						AprobacionApoderado2 = 1,
						UsuarioModificacion = @usuario
					where IdContrato = @idContrato 
					and IdAdquisicion = @idAdquisicion
				end


			end
			else
			begin
				update Licitacion.Lic_ContratoLicitacion
				set IdEstado = @idEstado,
					FechaModificacion = GETDATE()
				where IdContrato = @idContrato 
				and IdAdquisicion = @idAdquisicion
			end

			if @@rowcount = 1
			begin
				insert into Licitacion.Lic_LogFlujoContrato
				(
					IdAccion,
					IdContrato,
					IdEstadoContrato,
					UsuarioRegistro,
					FechaRegistro
				)
				values
				(
					@accionRechazado,
					@idContrato,
					@idEstado,
					@usuario,
					GETDATE()
				)
			end

		End

		If @@TRANCOUNT > 0
		Begin
			COMMIT TRANSACTION
		End

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
		Begin
			ROLLBACK TRANSACTION
		End

		select	@errorMessage  = ERROR_MESSAGE(),
				@errorSeverity = ERROR_SEVERITY(),
				@errorState = ERROR_STATE()

		RAISERROR(@errorMessage, @errorSeverity, @errorState)
    END CATCH

END
