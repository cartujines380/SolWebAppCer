-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 07-04-2016
-- Description:	REPORTE DE USUARIO NO INGRESA PORTAL
-- 319
-- =============================================

CREATE PROCEDURE [Seguridad].[Seg_UsuarioNoIngresado]
	@PI_ParamXML xml
AS
BEGIN
	DECLARE @W_USUARIO VARCHAR(20) 
	DECLARE @W_CODSAP VARCHAR(15) 
	SET NOCOUNT ON;
	SELECT	
					
			    @W_USUARIO=nref.value('@Usuario','VARCHAR(20)'),
				@W_CODSAP=nref.value('@CodSap','VARCHAR(15)')
				FROM  @PI_ParamXML.nodes('/Root') as item(nref)

IF(@W_CODSAP='')
SET @W_CODSAP=NULL

SET @W_USUARIO='%'+@W_USUARIO+'%'

	SELECT DISTINCT IdUsuario INTO #A
	FROM SIPE_FRAMEWORK.Seguridad.Seg_Auditoria AT
	WHERE AT.IdOrganizacion=39

	SELECT PO.RUC,PO.CodProveedor,PO.NomComercial,PO.Telefono,US.Usuario,
	isnull((select isnull(ud.Apellido1,' ') + ' '+ isnull(ud.Nombre1,' ') from Seguridad.Seg_UsuarioAdicional ud where ud.Usuario=us.Usuario and ud.Ruc=us.Ruc),'')as nombre,
	US.CorreoE,US.FechaRegistro
	FROM SIPE_PROVEEDOR.SEGURIDAD.SEG_USUARIO US
		INNER JOIN SIPE_PROVEEDOR.PROVEEDOR.PRO_PROVEEDOR PO
			ON US.CodProveedor=PO.CodProveedor
	WHERE US.Estado='A'
	AND  SUBSTRING(US.RUC,1,10)+US.Usuario NOT IN(
	SELECT * FROM #A)
	AND  US.Usuario LIKE(@W_USUARIO)
	AND  US.CodProveedor=ISNULL(@W_CODSAP,US.CodProveedor)
	ORDER BY RUC

DROP TABLE #A
	




END
