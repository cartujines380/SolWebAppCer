
-- EXEC [Seguridad].[Seg_P_ConsInfoSeguraUsuarioParticipante] '1702576651prueba01'
-- EXEC [Seguridad].[Seg_P_ConsInfoSeguraUsuarioParticipante] '1702576651CER00009'
-- EXEC [Seguridad].[Seg_P_ConsInfoSeguraUsuarioParticipante] '1001402211CER00008'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsInfoSeguraUsuarioParticipante]
	@PV_IdUsuario VARCHAR(50)
AS
	DECLARE
			@IdParticipante		INT,
			@EsClaveBloqueo		VARCHAR(1),
			@EsClaveNuevo		VARCHAR(1),
			@ImagenSecreta		VARCHAR(10)

	SELECT @IdParticipante = IdParticipante
		FROM Participante.Par_Participante
		WHERE IdUsuario = @PV_IdUsuario

	SELECT
		@EsClaveBloqueo	= c.EsClaveBloqueo,
		@EsClaveNuevo	= c.EsClaveNuevo,
		@ImagenSecreta	= c.ImagenSecreta
	FROM [Seguridad].[Seg_Clave] c
	WHERE c.IdParticipante = @IdParticipante

	IF (@EsClaveNuevo = 'S')
	BEGIN
		RAISERROR('El usuario es nuevo y no ha realizado aún su primer inicio de sesión.', 16, 1)
		RETURN
	END
	
	IF (@EsClaveBloqueo = 'S')
	BEGIN
		RAISERROR('El usuario se encuentra con la contraseña bloqueada.', 16, 1)
		RETURN
	END
	
	IF (NOT EXISTS(SELECT TOP 1 r.CodPregunta FROM [Seguridad].[Seg_RespuestaSegura] r WHERE r.IdParticipante = @IdParticipante))
	BEGIN
		RAISERROR('Usuario no tiene información de seguridad, puede que sea nuevo o migrado, favor verificar con el administrador de su cuenta.', 16, 1)
		RETURN
	END
	
	SELECT
		@ImagenSecreta as CodImagenSecreta,
		ISNULL(Catalogo.Ctl_F_conCatalogo(50, @ImagenSecreta), '') as DesImagenSecreta

	SELECT
		r.CodPregunta, Catalogo.Ctl_F_conCatalogo(51, r.CodPregunta) as DesPregunta, r.Respuesta
	FROM [Seguridad].[Seg_RespuestaSegura] r
	WHERE r.IdParticipante = @IdParticipante
	ORDER BY 2


