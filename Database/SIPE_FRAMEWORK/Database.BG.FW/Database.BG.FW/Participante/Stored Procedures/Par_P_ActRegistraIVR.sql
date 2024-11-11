CREATE PROCEDURE [Participante].[Par_P_ActRegistraIVR] (@PI_IdParticipante int, 
									@PI_Clave varchar(50), @PI_Estado char)
AS

BEGIN TRY
	BEGIN TRAN
	-- Registra Cliente
	UPDATE Participante.Par_RegistroIVR 
	 SET Clave = @PI_Clave, Estado = @PI_Estado
	WHERE IdParticipante = @PI_IdParticipante

	IF XACT_STATE() = 1
		COMMIT TRAN

END TRY	
BEGIN CATCH
	--Preguntar si existe transaccion
	IF XACT_STATE() IN (1,-1)
		ROLLBACK TRAN
	-- Produce un RAISERROR con el msg de error
	exec Participante.Par_P_Error 'Participante.Par_P_ActRegistraIVR'
END CATCH





