

CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaLineaNeg]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @CodProveedor			Varchar(10)
	
	SELECT
		@CodProveedor		= nref.value('@CodProveedor','varchar(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	SELECT LineaNegocio as Codigo,
	       Principal	
	FROM Proveedor.Pro_LineaNegocio
	WHERE CodProveedor = @CodProveedor


END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaLineaNeg]'
END CATCH


