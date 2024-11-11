CREATE PROC [Participante].[Par_P_Error] (@SP varchar(100))
AS
	DECLARE @ErrorMessage NVARCHAR(4000),
			@ErrorSeverity INT,
			@ErrorState INT 
	SELECT 
        @ErrorMessage = 'SP(' + @SP + '): ' + ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE()

	IF (@ErrorState<1 OR @ErrorState>127)
		SET @ErrorState=1

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               )




