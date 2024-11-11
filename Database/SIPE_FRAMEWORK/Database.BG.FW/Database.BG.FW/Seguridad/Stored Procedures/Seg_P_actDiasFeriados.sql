
CREATE Procedure [Seguridad].[Seg_P_actDiasFeriados]
@xmlDatos xml
AS
	-- Variables de Error
	DECLARE @ErrorMessage NVARCHAR(4000),@ErrorSeverity INT,@ErrorState INT 
	
	BEGIN TRY
		BEGIN TRAN
		-- Ingresa los dias nuevos
		INSERT Seguridad.Seg_DiasFeriados(IdEmpresa,IdSucursal,Dia)
		SELECT	nref.value('@IdEmpresa','int'),nref.value('@IdSucursal','int'), nref.value('@Dia','datetime')
		FROM @xmlDatos.nodes('/Dias/Dia') AS R(nref)
		WHERE NOT EXISTS(SELECT 1 FROM Seguridad.Seg_DiasFeriados d
							WHERE d.IdEmpresa = nref.value('@IdEmpresa','int')
							    AND d.IdSucursal = nref.value('@IdSucursal','int')
							    AND d.Dia = nref.value('@Dia','datetime') ) 
		
		-- Elimina los dias que no existen
		DELETE Seguridad.Seg_DiasFeriados
		WHERE NOT EXISTS(SELECT 1 FROM @xmlDatos.nodes('/Dias/Dia') AS R(nref)
							WHERE IdEmpresa = nref.value('@IdEmpresa','int')
							    AND IdSucursal = nref.value('@IdSucursal','int')
							    AND Dia = nref.value('@Dia','datetime') ) 
		COMMIT TRAN
	END TRY	
	BEGIN CATCH
		--Preguntar si existe transaccion
		IF XACT_STATE() IN (1,-1)
			ROLLBACK TRAN
		-- Produce un RAISERROR con el msg de error
		SELECT 
			@ErrorMessage	= ERROR_MESSAGE(),
			@ErrorSeverity	= ERROR_SEVERITY(),
			@ErrorState		= ERROR_STATE()
		IF (@ErrorState<1 OR @ErrorState>127)
			SET @ErrorState=1
		RAISERROR (@ErrorMessage,@ErrorSeverity,@ErrorState)
	END CATCH	


