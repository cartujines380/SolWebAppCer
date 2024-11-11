CREATE proc [Seguridad].[Seg_P_MT_Secuencia]
@PV_Trama varchar(max),
@PI_IdAplicacion int,
@PI_IdTransaccion int,
@PO_operacion_mt bigint output
AS
BEGIN TRY	
         BEGIN TRAN

	   	 INSERT INTO	Seguridad.Seg_SecuenciaMT (Trama,FechaTrama,IdAplicacion,IdTransaccion)
		 Values (@PV_Trama,GETDATE(),@PI_IdAplicacion,@PI_IdTransaccion)
		
		 Select @PO_operacion_mt = @@IDENTITY

		 IF @@TRANCOUNT > 0
				COMMIT	TRAN		
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
	ROLLBACK TRAN
		EXEC Seguridad.Seg_P_Error 'Seguridad.Seg_P_MT_Secuencia'
END CATCH
