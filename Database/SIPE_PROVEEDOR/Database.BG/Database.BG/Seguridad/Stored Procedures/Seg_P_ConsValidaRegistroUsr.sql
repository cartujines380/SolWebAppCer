
-- EXEC [Seguridad].[Seg_P_ConsValidaRegistroUsr] '<Root IdEmpresa="1" Ruc="0987654321001" Usuario="usrTesting01" />'
-- EXEC [Seguridad].[Seg_P_ConsValidaRegistroUsr] '<Root IdEmpresa="1" Ruc="1790331709001" Usuario="CER00001" />'
-- EXEC [Seguridad].[Seg_P_ConsValidaRegistroUsr] '<Root IdEmpresa="1" Ruc="1702576651001" Usuario="prueba03" />'
-- EXEC [Seguridad].[Seg_P_ConsValidaRegistroUsr] '<Root IdEmpresa="1" Ruc="usrmtraframe" Usuario="usrmtraframe" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsValidaRegistroUsr]
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa		INT
		,@Ruc			VARCHAR(13)
		,@Usuario		VARCHAR(20)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(20)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)

	DECLARE @IdParticipante int, @Identificacion varchar(13), @CodSAP varchar(10), @Nombre varchar(400),
			@CorreoE varchar(50), @Celular varchar(50), @EsAdmin bit, @Estado varchar(1)

	SELECT
		@IdParticipante		= isnull(u.IdParticipante, -1),
		@Identificacion		= isnull(a.Identificacion, u.Ruc),
		@CodSAP				= isnull(u.CodProveedor, ''),
		@Nombre				= isnull(p.NomComercial, a.Apellido1 + ' ' + ISNULL(a.Apellido2, '') + ' ' + a.Nombre1 + ' ' + ISNULL(a.Nombre2, '')),
		@CorreoE			= u.CorreoE,
		@Celular			= u.Celular,
		@EsAdmin			= u.EsAdmin,
		@Estado				= u.Estado
		FROM [Seguridad].[Seg_Usuario] u
			LEFT OUTER JOIN [Seguridad].[Seg_UsuarioAdicional] a
				ON u.IdEmpresa = a.IdEmpresa AND u.Ruc = a.Ruc AND u.Usuario = a.Usuario AND u.EsAdmin = 0
			LEFT OUTER JOIN [Proveedor].[Pro_Proveedor] p
				ON u.CodProveedor = p.CodProveedor AND u.EsAdmin = 1
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario


	IF (@Estado = 'I')
	BEGIN
		RAISERROR('Usuario no se encuentra activo.', 16, 1)
		RETURN
	END
	ELSE
	BEGIN
		IF (@EsAdmin=0)
		BEGIN
			IF (EXISTS(SELECT 1 FROM [Seguridad].[Seg_Usuario] u
				WHERE u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.EsAdmin = 1 AND u.Estado = 'I'))
			BEGIN
				RAISERROR('Usuario principal del proveedor no se encuentra activo.', 16, 1)
				RETURN
			END
		END
	END

	SELECT
			Ruc				= @Ruc,
			Usuario			= @Usuario,
			IdParticipante	= @IdParticipante,
			Identificacion	= @Identificacion,
			CodSAP			= @CodSAP,
			Nombre			= @Nombre,
			CorreoE			= @CorreoE,
			Celular			= @Celular,
			EsAdmin			= @EsAdmin,
			Estado			= @Estado
		WHERE NOT @Estado IS NULL

END

