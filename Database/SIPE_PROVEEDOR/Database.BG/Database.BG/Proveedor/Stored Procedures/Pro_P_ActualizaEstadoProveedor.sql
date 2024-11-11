
CREATE PROCEDURE [Proveedor].[Pro_P_ActualizaEstadoProveedor]
		@PI_ParamXML xml
	

AS
SET ARITHABORT ON;
BEGIN TRY
    
    
	BEGIN TRAN
	Declare @CodProveedor varchar(10)
	Declare @Estado varchar(2)

	SELECT 
		@CodProveedor = nref.value('@CodProveedor','varchar(10)')
		,@Estado = nref.value('@Estado','VARCHAR(2)')
		
	FROM @PI_ParamXML.nodes('/Root') as item(nref) 

	--Validar si codigo existe
	IF NOT EXISTS (SELECT   TOP 1 1 FROM Proveedor.Pro_Proveedor WITH (NOLOCK)	WHERE   codProveedor = @CodProveedor)
	BEGIN
	   SELECT '1-Código de Proveedor no existe.'
	   return
	END

	--Registrar en bitacora de estado proveedor
	INSERT INTO Proveedor.Pro_Estado_Proveedor values (@CodProveedor,@Estado,GETDATE())

	IF @@TRANCOUNT > 0
	BEGIN
			COMMIT	TRAN
			 SELECT '0-Estado de proveedor actualizado correctamente'
    END
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Pro_P_ActualizaEstadoProveedor]'
END CATCH


