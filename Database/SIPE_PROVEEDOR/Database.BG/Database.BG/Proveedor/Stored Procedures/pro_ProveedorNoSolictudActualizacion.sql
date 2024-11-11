-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 07-04-2016
-- Description:	REPORTE DE PORVEEDOR NO SOLICITUD
-- 323
-- =============================================

CREATE PROCEDURE [Proveedor].[pro_ProveedorNoSolictudActualizacion]
	@PI_ParamXML xml
AS
BEGIN
	DECLARE @W_RUC VARCHAR(20) 
	DECLARE @W_CODSAP VARCHAR(15) 
	SET NOCOUNT ON;
	SELECT	
					
			    @W_RUC=nref.value('@Ruc','VARCHAR(20)'),
				@W_CODSAP=nref.value('@CodSap','VARCHAR(15)')
				FROM  @PI_ParamXML.nodes('/Root') as item(nref)

IF(@W_CODSAP='')
	SET @W_CODSAP=NULL

IF(@W_RUC='')
	SET @W_RUC=NULL

	select distinct CodSapProveedor into #a from Proveedor.Pro_SolProveedor sp where sp.CodSapProveedor is not null and sp.CodSapProveedor<>'' 

	select distinct codproveedor INTO #b 
	from Seguridad.Seg_Usuario us 
	where us.Usuario like('CER0%') 
	AND	   us.CodProveedor=ISNULL(@W_CODSAP,us.CodProveedor)
	AND	   us.Ruc=ISNULL(@W_RUC,us.Ruc)

select  po.CodProveedor,po.Ruc,po.NomComercial,po.DirCalleNum,po.Poblacion ciudad,po.Telefono,po.CorreoE,po.ApoderadoApe + ' '+po.ApoderadoNom representante
from Proveedor.Pro_Proveedor po 
	inner join #b  b
	on	po.CodProveedor=b.CodProveedor
where po.CodProveedor not in(
select * from #a)


drop table #a
drop table #b

END
