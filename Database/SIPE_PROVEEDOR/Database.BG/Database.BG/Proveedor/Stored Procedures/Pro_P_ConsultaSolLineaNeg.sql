

CREATE PROCEDURE [Proveedor].[Pro_P_ConsultaSolLineaNeg]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	DECLARE
		 @IdSolicitud			BIGINT
	
	SELECT
		@IdSolicitud		= nref.value('@IdSolicitud','bigint')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	SELECT LineaNegocio as Codigo,
	       Principal	
	FROM Proveedor.Pro_SolicitudLineaNegocio
	WHERE IDSOLICITUD = @IdSolicitud


END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Pro_P_ConsultaSolLineaNeg]'
END CATCH


