
-- EXEC [Seguridad].[Seg_P_ConsDatosUsrAdmin] '<Root IdEmpresa="1" Ruc="1790824977001" Usuario="ADM00001" CodSAP="115081" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsDatosUsrAdmin]
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa		INT
		,@Ruc			VARCHAR(13)
		,@Usuario		VARCHAR(1)
		,@CodProveedor	VARCHAR(10)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(20)'),
		@CodProveedor		= nref.value('@CodSAP','VARCHAR(10)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)

	SELECT	u.IdEmpresa, u.Ruc, u.Usuario, u.IdParticipante, isnull(u.CodProveedor, '') as CodSAP,
			u.CorreoE, isnull(u.Telefono, '') as Telefono, isnull(u.Celular, '') as Celular,
			u.EsAdmin, u.Estado, u.UsrAutorizador,
			convert(varchar, u.FechaRegistro, 120) as FechaRegistro, -- 120 = yyyy-MM-dd HH:mm:ss
			isnull(convert(varchar, u.FechaModificacion, 120), '') as FechaModificacion
		FROM [Seguridad].[Seg_Usuario] u
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

	SELECT	p.Ruc, p.NomComercial, p.CorreoE, p.Movil, p.Telefono, p.Pais,
			isnull(p.DirCalleNum, '') + ' ' + isnull(p.DirCallePrinc, '') + ' ' + isnull(p.DirPisoEdificio, '') as Direccion
		FROM [Proveedor].[Pro_Proveedor] p
		WHERE p.CodProveedor = @CodProveedor

END

