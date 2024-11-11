

create PROCEDURE [Seguridad].[Seg_P_ActDatosLegAsociados]
	( @PI_ParamXml xml )
AS
BEGIN

	BEGIN TRAN

	DECLARE
		 @IdEmpresa			INT
		,@Ruc				VARCHAR(13)
		,@Usuario			VARCHAR(20)
		,@Cedula			VARCHAR(13)
		,@CodLegacy			VARCHAR(10)
		,@UserLegacy		VARCHAR(20)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(20)'),
		@Cedula				= nref.value('@Cedula','VARCHAR(13)'),
		@CodLegacy			= nref.value('@CodLegacy','VARCHAR(10)'),
		@UserLegacy			= nref.value('@UserLegacy','VARCHAR(20)')
	FROM @PI_ParamXml.nodes('/Root') AS R(nref)

	UPDATE [Seguridad].[Seg_Usuario]
		SET [CodLegacy] = @CodLegacy,
			[UsrLegacy] = @UserLegacy,
			[FechaModificacion] = GETDATE()
	WHERE [IdEmpresa] = @IdEmpresa
		AND [Ruc] = @Ruc
		AND [Usuario] = @Usuario

	COMMIT TRAN

END


