
-- EXEC [Seguridad].[Seg_P_ConsDatosUsrAdicional]  '<Root IdEmpresa="1" Ruc="0987654321001" Usuario="admin" />'
-- EXEC [Seguridad].[Seg_P_ConsDatosUsrAdicional]  '<Root IdEmpresa="1" Ruc="0987654321001" Usuario="usrTemporal01" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsDatosUsrAdicional]
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

	SELECT	u.IdEmpresa, u.Ruc, u.Usuario, u.IdParticipante, isnull(u.CodProveedor, '') as CodSAP,
			u.CorreoE, isnull(u.Telefono, '') as Telefono, isnull(u.Celular, '') as Celular,
			u.EsAdmin, u.Estado, u.UsrAutorizador,
			convert(varchar, u.FechaRegistro, 120) as FechaRegistro, -- 120 = yyyy-MM-dd HH:mm:ss
			isnull(convert(varchar, u.FechaModificacion, 120), '') as FechaModificacion,u.UsrCargo as Cargo,u.UsrFuncion as Funcion
		FROM [Seguridad].[Seg_Usuario] u
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

	SELECT	u.TipoIdent, u.Identificacion, u.Apellido1, isnull(u.Apellido2, '') as Apellido2,
			u.Nombre1, isnull(u.Nombre2, '') as Nombre2, u.EstadoCivil, u.Genero,
			u.Pais, u.Provincia, u.Ciudad, u.Direccion, isnull(u.RecibeActas,CONVERT(BIT,0)) as recActas
		FROM [Seguridad].[Seg_UsuarioAdicional] u
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

	SELECT	u.Zona, z.Detalle as DesZona
		FROM [Seguridad].[Seg_UsuarioZona] u
			INNER JOIN [Proveedor].[Pro_Catalogo] z 
				ON z.Codigo = u.Zona where z.Tabla = 1031
		and
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

	SELECT	u.Zona, u.Almacen
		FROM [Seguridad].[Seg_UsrZonaAlmacen] u
			
		WHERE
			u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc AND u.Usuario = @Usuario

END

