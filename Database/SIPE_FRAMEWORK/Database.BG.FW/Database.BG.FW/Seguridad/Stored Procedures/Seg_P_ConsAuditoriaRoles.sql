
CREATE Procedure [Seguridad].[Seg_P_ConsAuditoriaRoles]
	( @PI_DocXML varchar (max) )
AS
DECLARE @VL_IdXML int, @VL_TipoConsulta int
DECLARE @VL_IdRol int, @VL_IdEmpresa int, @VL_IdSucursal int
DECLARE @VL_IdUsuario varchar(20)

EXEC SP_XML_PREPAREDOCUMENT @VL_IdXML OUTPUT, @PI_DocXML

SELECT @VL_TipoConsulta = TipoConsulta 
	,@VL_IdRol = IdRol
	,@VL_IdEmpresa = IdEmpresa
	,@VL_IdSucursal = IdSucursal
	,@VL_IdUsuario = IdUsuario
FROM OPENXML (@VL_IdXML, '/Usuario') 
WITH (TipoConsulta int, IdRol int, IdEmpresa int, IdSucursal int, IdUsuario varchar(20))

EXEC SP_XML_REMOVEDOCUMENT @VL_IdXML

IF (@VL_TipoConsulta=0)
BEGIN
	Select
	 r.IdEmpresa, SIPE_FrameWork.Participante.Par_F_getNombreParticipante(r.IdEmpresa) as Empresa,
	 r.IdSucursal, SIPE_FrameWork.Participante.Par_F_getNombreParticipante(r.IdSucursal) as Sucursal,
	 r.IdRol, r.Nombre as Rol, r.Descripcion as DescRol, r.Status,
	 t.IdOrganizacion,
	 SIPE_FrameWork.Seguridad.Seg_F_getNombreOrganizacion(t.IdOrganizacion) as Organizacion,
	 t.IdTransaccion, t.Descripcion, t.Auditable, t.Estado
	Into #TmpReporteAuditRol
	From SIPE_FrameWork.Seguridad.Seg_Rol r
		Left outer Join SIPE_FrameWork.Seguridad.Seg_OpcionTransRol o
			On r.IdRol = o.IdRol
			Left outer Join SIPE_FrameWork.Seguridad.Seg_Transaccion t
				On o.IdOrganizacion = t.IdOrganizacion AND o.IdTransaccion = t.IdTransaccion
	Where (@VL_IdEmpresa = 0 OR r.IdEmpresa = @VL_IdEmpresa)
	  AND (@VL_IdSucursal = 0 OR r.IdSucursal = @VL_IdSucursal)
	  AND (@VL_IdUsuario IS NULL OR EXISTS(Select 1 From SIPE_FrameWork.Seguridad.Seg_RolUsuario u
											Where u.IdRol = r.IdRol AND u.IdUsuario = @VL_IdUsuario))

	Select distinct
	 IdEmpresa, Empresa, IdSucursal, Sucursal,
	 IdRol, Rol, DescRol, Status, IdOrganizacion, Organizacion,
	 IdTransaccion, Descripcion, Auditable, Estado
	From #TmpReporteAuditRol
	Order by IdEmpresa, IdSucursal, IdRol, IdOrganizacion, IdTransaccion

	Drop Table #TmpReporteAuditRol
	
	RETURN
END

IF (@VL_TipoConsulta=1)
BEGIN
	Select IdRol as Id, Nombre, Descripcion,
	 IdEmpresa, SIPE_FrameWork.Participante.Par_F_getNombreParticipante(IdEmpresa) as Empresa,
	 IdSucursal, SIPE_FrameWork.Participante.Par_F_getNombreParticipante(IdSucursal) as Sucursal,
	 Status 
	From SIPE_FrameWork.Seguridad.Seg_Rol r
	Where (@VL_IdEmpresa = 0 OR IdEmpresa = @VL_IdEmpresa)
	  AND (@VL_IdSucursal = 0 OR IdSucursal = @VL_IdSucursal)
	  AND (@VL_IdUsuario IS NULL OR EXISTS(Select 1 From SIPE_FrameWork.Seguridad.Seg_RolUsuario u
											Where u.IdRol = r.IdRol AND u.IdUsuario = @VL_IdUsuario))
	RETURN
END

IF (@VL_TipoConsulta=2)
BEGIN
	Select IdTransaccion, Descripcion, IdOrganizacion,
	 SIPE_FrameWork.Seguridad.Seg_F_getNombreOrganizacion(IdOrganizacion) as Organizacion,
	 Auditable, Estado 
	From SIPE_FrameWork.Seguridad.Seg_Transaccion t
	Where Exists (Select 1 From SIPE_FrameWork.Seguridad.Seg_OpcionTransRol r
					 Where t.IdTransaccion=r.IdTransaccion and 
						t.IdOrganizacion=r.IdOrganizacion and r.IdRol = @VL_IdRol)
	RETURN
END


