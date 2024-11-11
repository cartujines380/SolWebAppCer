-- =============================================
-- Author:		Fabricio Castro S.
-- Create date: 22-06-2016
-- Description:	Ingreso, actualización de contratos del proveedor
-- =============================================
CREATE PROCEDURE [Proveedor].[Pro_MantenimientoContrato]
	@PI_ParamXML xml
AS
BEGIN TRY
	
	BEGIN TRAN

	SET NOCOUNT ON;
	


	--Ingresar nuevo contratos
	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root/Contrato') AS item(nref) where nref.value('@Accion','char(1)') = 'I')
	BEGIN
	    
		--Validar si ya existe contrato
		

		
		IF (( SELECT  COUNT(*) 	   
			 FROM Proveedor.Pro_Contrato con
				INNER JOIN @PI_ParamXML.nodes('/Root/Contrato') item(nref) 
					ON (con.NumContrato= nref.value('@NumContrato','varchar(10)') AND nref.value('@Accion','char(1)') = 'I')
					) 
		    = 0 )
		BEGIN

			INSERT INTO Proveedor.Pro_Contrato
			   (
				CodProveedor
			   ,NumContrato
			   ,NomArchivo
			   ,FechaRegistro          
			   ,Estado)
			SELECT 
					nref.value('@CodProveedor','varchar(10)'),
					nref.value('@NumContrato','varchar(10)'),
					nref.value('@NomArchivo','varchar(60)'),				
					GETDATE(),
					nref.value('@Estado','varchar(1)')
			FROM @PI_ParamXML.nodes('/Root/Contrato') as item(nref)
			WHERE nref.value('@Accion','char(1)') = 'I';
			
			SELECT '0-OK' as mensaje
		END
		ELSE
		BEGIN
		   SELECT '1-Número de contrato ya registrado' as mensaje
		END

		
	END
	--Ingresar nuevo contratos
	IF EXISTS (SELECT TOP 1 1 FROM @PI_ParamXML.nodes('/Root/Contrato') AS item(nref) where nref.value('@Accion','char(1)') = 'U')
	BEGIN

	   IF (( SELECT  COUNT(*) 	   
			 FROM Proveedor.Pro_Contrato con
				INNER JOIN @PI_ParamXML.nodes('/Root/Contrato') item(nref) 
					ON (con.NumContrato= nref.value('@NumContrato','varchar(10)') AND nref.value('@Accion','char(1)') = 'U')
					) 
		    = 0 )
	   BEGIN
	       
		   SELECT '2-Número de contrato no registrado' as mensaje

	   END
	   ELSE
	   BEGIN
	    
			UPDATE con
				   SET Estado = nref.value('@Estado','varchar(1)'),
					   FechaActualizacion = GETDATE()
				 FROM Proveedor.Pro_Contrato con
					INNER JOIN @PI_ParamXML.nodes('/Root/Contrato') item(nref) 
						ON (con.NumContrato= nref.value('@NumContrato','varchar(10)') AND nref.value('@Accion','char(1)') = 'U');
			SELECT '0-OK' as mensaje
		END
	END
	
	
	

IF @@TRANCOUNT > 0
         --SELECT '0-OK'
		COMMIT	TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	exec SP_PROV_Error @sp='[Art_P_SolicitudArticulo]'
END CATCH


