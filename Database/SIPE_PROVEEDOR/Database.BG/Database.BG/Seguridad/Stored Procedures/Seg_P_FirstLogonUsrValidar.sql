
-- EXEC [Seguridad].[Seg_P_FirstLogonUsrValidar] '<Root IdEmpresa="1" Ruc="1702576651001" Usuario="usrTesting01" />'
-- EXEC [Seguridad].[Seg_P_FirstLogonUsrValidar] '<Root IdEmpresa="1" Ruc="1790331709001" Usuario="CER00001" />'
-- EXEC [Seguridad].[Seg_P_FirstLogonUsrValidar] '<Root IdEmpresa="1" Ruc="usrmtraframe" Usuario="usrmtraframe" />'

CREATE PROCEDURE [Seguridad].[Seg_P_FirstLogonUsrValidar]
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa		INT
		,@Ruc			VARCHAR(13)
		,@Usuario		VARCHAR(20)

	SELECT
		@IdEmpresa	= nref.value('@IdEmpresa','INT'),
		@Ruc		= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario	= nref.value('@Usuario','VARCHAR(20)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)

	DECLARE @IdParticipante INT, @IdentReprLegal varchar(13), @CodSAP varchar(10),
			@CorreoE varchar(50), @Celular varchar(50), @Telefono varchar(50), @NomComercial varchar(35)

	SELECT
		@IdParticipante	= u.IdParticipante,
		@CodSAP			= u.CodProveedor,
		@CorreoE		= u.CorreoE,
		@Celular		= u.Celular,
		@Telefono		= ISNULL(u.Telefono, ''),
		@NomComercial   = (SELECT TOP 1 PR.NomComercial FROM Proveedor.Pro_Proveedor PR WHERE PR.Ruc = @Ruc)
	FROM [Seguridad].[Seg_Usuario] u
		WHERE u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

	IF (@CodSAP IS NULL)
		BEGIN
			SELECT @CodSAP = u.CodProveedor
			FROM [Seguridad].[Seg_Usuario] u
				WHERE u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.EsAdmin = 1
		END

	SELECT @IdentReprLegal = LEFT(LTRIM(p.ApoderadoIdFiscal), 13)
	FROM [Proveedor].[Pro_Proveedor] p
		WHERE p.CodProveedor = @CodSAP

	--IF (@IdentReprLegal IS NULL OR @IdentReprLegal = '')
	--	BEGIN
	--		SELECT TOP 1 @IdentReprLegal = LEFT(LTRIM(p.CodContacto), 13)
	--		FROM [Proveedor].[Pro_ProveedorContacto] p
	--			WHERE p.CodProveedor = @CodSAP
	--			ORDER BY p.Funcion ASC
	--	END

	IF (@IdentReprLegal = '')
		BEGIN
			SET @IdentReprLegal = NULL
		END

	SELECT
			Ruc					= @Ruc,
			Usuario				= @Usuario,
			IdParticipante		= @IdParticipante,
			CodSAP				= @CodSAP,
			IdentReprLegal		= ISNULL(@IdentReprLegal, @Ruc),
			CorreoE				= @CorreoE,
			Celular				= @Celular,
			Telefono			= @Telefono,
			NomComercial        = @NomComercial

END

