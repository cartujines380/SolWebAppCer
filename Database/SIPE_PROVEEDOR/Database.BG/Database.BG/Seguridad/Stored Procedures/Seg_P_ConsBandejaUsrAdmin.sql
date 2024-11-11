
CREATE PROCEDURE [Seguridad].[Seg_P_ConsBandejaUsrAdmin] 
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa			INT
		,@CodSAP			VARCHAR(10)
		,@Ruc				VARCHAR(13)
		,@Nombre			VARCHAR(35)
		,@Estado			VARCHAR(1)
		,@ConUsuario		VARCHAR(1)
		,@Usuario			VARCHAR(20)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@CodSAP				= nref.value('@CodSAP','VARCHAR(10)'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Nombre				= nref.value('@Nombre','VARCHAR(35)'),
		@Estado				= nref.value('@Estado','VARCHAR(1)'),
		@ConUsuario			= nref.value('@ConUsuario','VARCHAR(1)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(20)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)
	
	IF (NOT @CodSAP IS NULL)
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			WHERE p.CodProveedor = @CodSAP
	END
	ELSE IF (NOT @Ruc IS NULL)
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			WHERE p.Ruc = @Ruc
	END
	ELSE IF (NOT @Nombre IS NULL)
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			WHERE p.NomComercial like '%'+@Nombre+'%'
			ORDER BY p.NomComercial
	END
	ELSE IF (NOT @Estado IS NULL)
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			INNER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			WHERE u.Estado = @Estado
	END
	ELSE IF (@ConUsuario = 'T')
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			ORDER BY u.Usuario
	END
	ELSE IF (@ConUsuario = 'S')
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			INNER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa
			ORDER BY u.Usuario
	END
	ELSE IF (@ConUsuario = 'N')
	BEGIN
		SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(p.CorreoE, '') as CorreoE, '' as Telefono,
				'' as Celular, '' as Usuario,
				'A' as Estado, -1 as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
		  WHERE NOT EXISTS (
			SELECT TOP 1 1 FROM [Seguridad].[Seg_Usuario] u WHERE
				p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = @IdEmpresa )
			ORDER BY p.FechaMod DESC
	END
	ELSE IF (NOT @Usuario IS NULL)
	BEGIN
			SELECT 
				p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
				ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
				ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
				ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		  FROM [Proveedor].[Pro_Proveedor] p
			LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
				p.CodProveedor = u.CodProveedor --AND u.EsAdmin = 1 
				AND u.IdEmpresa = @IdEmpresa
			WHERE u.Usuario = @Usuario
	END
END

