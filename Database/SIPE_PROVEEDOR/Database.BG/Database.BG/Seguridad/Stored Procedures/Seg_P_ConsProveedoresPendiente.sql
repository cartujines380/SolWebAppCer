
CREATE PROCEDURE [Seguridad].[Seg_P_ConsProveedoresPendiente] 
AS
BEGIN


		--SELECT 
		--		p.Ruc, p.CodProveedor as CodSAP, p.NomComercial as RazonSocial,
		--		ISNULL(u.CorreoE, ISNULL(p.CorreoE, '')) as CorreoE, ISNULL(u.Telefono, '') as Telefono,
		--		ISNULL(u.Celular, '') as Celular, ISNULL(u.Usuario, '') as Usuario,
		--		ISNULL(u.Estado, 'A') as Estado, ISNULL(u.IdParticipante, -1) as IdParticipante,p.ApoderadoIdFiscal as IdRepresentante
		--  FROM [Proveedor].[Pro_Proveedor] p
		--	LEFT OUTER JOIN [Seguridad].[Seg_Usuario] u ON 
		--		p.CodProveedor = u.CodProveedor AND u.EsAdmin = 1 AND u.IdEmpresa = 1
		--	ORDER BY p.CodProveedor

			Select 
  Prov.Ruc
, Prov.CodProveedor as CodSAP
, Prov.NomComercial as RazonSocial
, Prov.CorreoE
, Prov.Telefono
, '' as Celular --Celular proviene de la tabla SIPE_PROVEEDOR.Seguridad.Seg_Usuario
, '' as Clave
, 'A' as Estado
, Prov.ApoderadoIdFiscal as IdRepresentante
from SIPE_PROVEEDOR.[Proveedor].[Pro_Proveedor] Prov
where Prov.CorreoE is not null or Prov.CorreoE = ''
and Prov.CodProveedor not in ( 
select codproveedor from SIPE_PROVEEDOR.Seguridad.Seg_Usuario u where Estado = 'A'
AND u.EsAdmin = 1
AND u.IdEmpresa = 1
)


	
END

