
CREATE PROCEDURE [Proveedor].[Pro_P_ConsProvCodLegacyPrinc]
( @PI_XmlParam xml )
AS
/*
Ejemplo:
--	exec [Proveedor].[Pro_P_ConsProvCodLegacyPrinc] '<Root IdEmpresa="1" CodLegacy="" CodProveedor="103115" />'
--	exec [Proveedor].[Pro_P_ConsProvCodLegacyPrinc] '<Root IdEmpresa="1" CodLegacy="5360" CodProveedor="" />'
--	exec [Proveedor].[Pro_P_ConsProvCodLegacyPrinc] '<Root IdEmpresa="1" TipoFilNombre="C" Nombre="FLO" />'
*/

DECLARE
	 @IdEmpresa			INT
	,@TipoFilNombre		VARCHAR(35)
	,@Nombre			VARCHAR(35)
	,@CodLegacy			VARCHAR(10)
	,@CodProveedor		VARCHAR(10)
	,@ERROR				VARCHAR(200)
	
DECLARE @CODLEG TABLE (CodLegacy VARCHAR(10))

BEGIN TRY

     SELECT 
			@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		    @TipoFilNombre     	= nref.value('@TipoFilNombre','VARCHAR(1)'),
			@Nombre     		= ISNULL(nref.value('@Nombre','VARCHAR(35)'), ''),
			@CodLegacy     		= ISNULL(nref.value('@CodLegacy','VARCHAR(10)'), ''),
			@CodProveedor		= ISNULL(nref.value('@CodProveedor','VARCHAR(10)'), '')
     FROM @PI_XmlParam.nodes('/Root') AS R(nref)

	IF (@Nombre = '')
		BEGIN
			SET @Nombre = '%'
		END
		ELSE
		BEGIN
			IF (@TipoFilNombre = 'E')
				BEGIN
					SET @Nombre = @Nombre + '%'
				END
			IF (@TipoFilNombre = 'C')
				BEGIN
					SET @Nombre = '%' + @Nombre + '%'
				END
			IF (@TipoFilNombre = 'T')
				BEGIN
					SET @Nombre = '%' + @Nombre
				END
		END
		
	SELECT l.CodLegacy as CodLegacyPrin, l.CodLegacy, p.CodProveedor, p.NomComercial, p.Ruc, l.Principal
		INTO #tblConsCodPrinc
	FROM Proveedor.Pro_ProveedorLegacy l
		INNER JOIN Proveedor.Pro_Proveedor p
			ON l.CodProveedor = p.CodProveedor
	WHERE l.CodLegacy IN (
		SELECT CodLegacy FROM Proveedor.Pro_ProveedorLegacy GROUP BY CodLegacy HAVING COUNT(1) > 1)

	INSERT INTO @CODLEG
	SELECT CodLegacyPrin
		FROM #tblConsCodPrinc
		WHERE NomComercial like @Nombre
		  AND (@CodProveedor = '' OR CodProveedor = @CodProveedor)
		  AND (@CodLegacy = '' OR CodLegacy = @CodLegacy)
		  
	DELETE #tblConsCodPrinc WHERE CodLegacyPrin NOT IN (SELECT x.CodLegacy FROM @CODLEG x)

	UPDATE T SET CodLegacy = l.CodLegacy
		FROM #tblConsCodPrinc T
			INNER JOIN Proveedor.Pro_ProveedorLegacy l
				ON l.CodProveedor = T.CodProveedor
					AND LEN(l.CodLegacy) > 5 AND l.CodLegacy like '%'+ T.CodLegacyPrin +'_'

	SELECT t.CodLegacyPrin, t.CodLegacy, t.CodProveedor, t.NomComercial, t.Ruc, t.Principal, t.Principal as NewPrincipal
	FROM #tblConsCodPrinc t
		--INNER JOIN Proveedor.Pro_ProveedorLegacy l
		--	ON l.CodProveedor = t.CodProveedor
	ORDER BY t.CodLegacyPrin, t.CodLegacy

END  TRY
BEGIN CATCH

	SET @ERROR = ERROR_MESSAGE()
	RAISERROR (@ERROR, 16, 1 )
END CATCH


