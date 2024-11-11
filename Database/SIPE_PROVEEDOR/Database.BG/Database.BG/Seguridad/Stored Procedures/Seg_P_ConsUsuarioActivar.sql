
-- EXEC [Seguridad].[Seg_P_ConsDatosUsrAdicional]  '<Root IdEmpresa="1" Ruc="0987654321001" Usuario="admin" />'
-- EXEC [Seguridad].[Seg_P_ConsDatosUsrAdicional]  '<Root IdEmpresa="1" Ruc="0987654321001" Usuario="usrTemporal01" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsUsuarioActivar]
	( @PI_ParamXML xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa		INT
		,@Ruc			VARCHAR(13)
		,@Usuario		VARCHAR(20)
		,@tipo		VARCHAR(5)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(20)'),
		@tipo				= nref.value('@tipo','VARCHAR(20)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)


	IF(@tipo='1')
	BEGIN
		UPDATE [Seguridad].[Seg_Usuario] SET Estado='I'
		FROM [Seguridad].[Seg_Usuario] u
		WHERE U.Ruc=@Ruc
		AND	  U.Usuario=@Usuario

		SELECT 1 AS RETORNO
	END
	IF(@tipo='2')
	BEGIN
		UPDATE [Seguridad].[Seg_Usuario] SET Estado='A'
		FROM [Seguridad].[Seg_Usuario] u
		WHERE U.Ruc=@Ruc
		AND	  U.Usuario=@Usuario

		SELECT 1 AS RETORNO
	END
	
	

END

