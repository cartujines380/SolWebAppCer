

CREATE PROCEDURE [Seguridad].[Seg_P_ConsDatosLegAsociados]
	( @PI_ParamXml xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa			INT
		,@Ruc				VARCHAR(13)
		,@Nombres			VARCHAR(100)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Nombres			= nref.value('@Nombres','VARCHAR(100)')
	FROM @PI_ParamXml.nodes('/Root') AS R(nref)

	SET @Ruc = CASE WHEN @Ruc = '' THEN NULL ELSE @Ruc END
	SET @Nombres = CASE WHEN @Nombres = '' THEN NULL ELSE @Nombres END

	SELECT	
		u.[Ruc] Ruc,
		u.Usuario,
		CASE [TipoIdent] 
			WHEN 'O' THEN '' 
			ELSE [Identificacion] END Cedula,
		CASE WHEN u.EsAdmin = 1 THEN u.Usuario
			ELSE ISNULL([Nombre1],'') + ' ' + ISNULL([Nombre2],'') END Nombres,
		CASE WHEN u.EsAdmin = 1 THEN ''
			ELSE ISNULL([Apellido1],'') + ' ' + ISNULL([Apellido2],'') END Apellidos,
		ISNULL([CodLegacy],'') CodLegacy,
		ISNULL([UsrLegacy],'') UserLegacy,
		u.CorreoE,
		u.Celular,
		(select top 1 COUNT(0)  from SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario rus where rus.IdUsuario in (
		select par.IdUsuario from SIPE_FRAMEWORK.Participante.Par_Participante par where par.IdParticipante = u.IdParticipante  )
		and rus.IdRol = 24) as rolAdmin,
		(select top 1 COUNT(0)  from SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario rus where rus.IdUsuario in (
		select par.IdUsuario from SIPE_FRAMEWORK.Participante.Par_Participante par where par.IdParticipante = u.IdParticipante  )
		and rus.IdRol = 25) as rolContable,
		(select top 1 COUNT(0)  from SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario rus where rus.IdUsuario in (
		select par.IdUsuario from SIPE_FRAMEWORK.Participante.Par_Participante par where par.IdParticipante = u.IdParticipante  )
		and rus.IdRol = 26) as rolLogistico,
		(select top 1 COUNT(0)  from SIPE_FRAMEWORK.Seguridad.Seg_RolUsuario rus where rus.IdUsuario in (
		select par.IdUsuario from SIPE_FRAMEWORK.Participante.Par_Participante par where par.IdParticipante = u.IdParticipante  )
		and rus.IdRol = 27) as rolComercial,
		 ISNULL(u.UsrCargo, '') as Departamento,
		ISNULL(u.UsrFuncion, '') as Funcion
	FROM [Seguridad].[Seg_Usuario] u
		LEFT JOIN [Seguridad].[Seg_UsuarioAdicional] a ON (u.IdEmpresa = a.IdEmpresa AND u.Ruc = a.Ruc AND u.Usuario = a.Usuario)
	WHERE u.IdEmpresa = @IdEmpresa 
		AND u.Ruc = ISNULL(@Ruc,u.Ruc) AND U.Estado='A'
		AND ISNULL([Nombre1],'') + ' ' + ISNULL([Nombre2],'') LIKE '%' + ISNULL(@Nombres,ISNULL([Nombre1],'') + ' ' + ISNULL([Nombre2],'')) + '%'

END


