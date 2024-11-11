
-- EXEC [Seguridad].[Seg_P_ConsDatosRecuperaClave] '<Root IdEmpresa="1" Ruc="1001402211001" Correo="cvera@sipecom.com" />'
-- EXEC [Seguridad].[Seg_P_ConsDatosRecuperaClave] '<Root IdEmpresa="1" Ruc="1702576651001" Correo="cvera@sipecom.com" />'
-- EXEC [Seguridad].[Seg_P_ConsDatosRecuperaClave] '<Root IdEmpresa="1" Ruc="1702576651001" Correo="vmunoz@sipecom.com" />'
-- EXEC [Seguridad].[Seg_P_ConsDatosRecuperaClave] '<Root IdEmpresa="1" Ruc="usrmtraframe" Correo="usrmtraframe" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsDatosRecuperaClave]
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE	@IdEmpresa	int,
			@Ruc		varchar(13),
			@CorreoE	varchar(50)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@CorreoE			= nref.value('@Correo','VARCHAR(50)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)

	DECLARE @IdParticipante int,
			@Usuario		varchar(20),
			@Nombre			varchar(400),
			@Celular		varchar(50),
			@Estado			varchar(1),
			@NomComercial   varchar(35)

	SELECT
		@IdParticipante		= isnull(u.IdParticipante, -1),
		@Usuario			= u.Usuario,
		@Nombre				= isnull(p.NomComercial, a.Apellido1 + ' ' + ISNULL(a.Apellido2, '') + ' ' + a.Nombre1 + ' ' + ISNULL(a.Nombre2, '')),
		@Celular			= u.Celular,
		@Estado				= u.Estado,
		@NomComercial       = (SELECT TOP 1 pr.NomComercial FROM Proveedor.Pro_Proveedor pr WHERE pr.Ruc = @Ruc )
		FROM [Seguridad].[Seg_Usuario] u
			LEFT OUTER JOIN [Seguridad].[Seg_UsuarioAdicional] a
				ON u.IdEmpresa = a.IdEmpresa AND u.Ruc = a.Ruc AND u.Usuario = a.Usuario AND u.EsAdmin = 0
			LEFT OUTER JOIN [Proveedor].[Pro_Proveedor] p
				ON u.CodProveedor = p.CodProveedor AND u.EsAdmin = 1
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.CorreoE = @CorreoE


	IF (@Estado = 'I')
	BEGIN
		RAISERROR('Usuario no se encuentra activo.', 16, 1)
		RETURN
	END

	SELECT
			Ruc				= @Ruc,
			Usuario			= @Usuario,
			IdParticipante	= @IdParticipante,
			Nombre			= @Nombre,
			CorreoE			= @CorreoE,
			Celular			= @Celular,
			NomComercial    = @NomComercial
		WHERE NOT @Estado IS NULL

END

