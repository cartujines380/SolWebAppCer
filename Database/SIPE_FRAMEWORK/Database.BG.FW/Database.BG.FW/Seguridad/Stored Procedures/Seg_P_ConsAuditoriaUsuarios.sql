
CREATE Procedure [Seguridad].[Seg_P_ConsAuditoriaUsuarios]
	( @PI_DocXML varchar (max) )
AS
DECLARE @VL_IdXML int
DECLARE @VL_IdRol int, @VL_IdUsuario varchar(20)

EXEC SP_XML_PREPAREDOCUMENT @VL_IdXML OUTPUT, @PI_DocXML

SELECT @VL_IdRol = IdRol, @VL_IdUsuario = IdUsuario
FROM OPENXML (@VL_IdXML, '/Usuario') 
WITH (IdRol int, IdUsuario varchar(20))

EXEC SP_XML_REMOVEDOCUMENT @VL_IdXML

	Select
	 u.IdUsuario, SIPE_FrameWork.Participante.Par_F_getNombreUsuario(u.IdUsuario) as Usuario,
	 r.IdRol, r.Nombre as Rol, t.IdOrganizacion,
	 SIPE_FrameWork.Seguridad.Seg_F_getNombreOrganizacion(t.IdOrganizacion) as Organizacion,
	 t.IdTransaccion, t.Descripcion, t.Auditable, t.Estado
	Into #TmpReporteAuditRol
	From SIPE_FrameWork.Seguridad.Seg_RolUsuario u
		Left outer Join SIPE_FrameWork.Seguridad.Seg_Rol r
			On u.IdRol = r.IdRol
			Left outer Join SIPE_FrameWork.Seguridad.Seg_OpcionTransRol o
				On r.IdRol = o.IdRol
				Left outer Join SIPE_FrameWork.Seguridad.Seg_Transaccion t
					On o.IdOrganizacion = t.IdOrganizacion AND o.IdTransaccion = t.IdTransaccion
	Where (@VL_IdRol IS NULL OR r.IdRol = @VL_IdRol)
	  AND (@VL_IdUsuario IS NULL OR u.IdUsuario = @VL_IdUsuario)

	Insert Into #TmpReporteAuditRol
	Select
	 u.IdUsuario, SIPE_FrameWork.Participante.Par_F_getNombreUsuario(u.IdUsuario) as Usuario,
	 NULL as IdRol, NULL as Rol, t.IdOrganizacion,
	 SIPE_FrameWork.Seguridad.Seg_F_getNombreOrganizacion(t.IdOrganizacion) as Organizacion,
	 t.IdTransaccion, t.Descripcion, t.Auditable, t.Estado
	From SIPE_FrameWork.Seguridad.Seg_TransUsuario u
		Left outer Join SIPE_FrameWork.Seguridad.Seg_Transaccion t
			On u.IdOrganizacion = t.IdOrganizacion AND u.IdTransaccion = t.IdTransaccion
	Where (@VL_IdUsuario IS NULL OR u.IdUsuario = @VL_IdUsuario)

	Select distinct 
	 IdUsuario, Usuario, IdRol, Rol, IdOrganizacion, Organizacion,
	 IdTransaccion, Descripcion, Auditable, Estado
	From #TmpReporteAuditRol
	Order by IdUsuario, IdRol, IdOrganizacion, IdTransaccion

	Drop Table #TmpReporteAuditRol


