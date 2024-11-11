
CREATE Procedure [Seguridad].[Seg_P_Error](@SP varchar(100))
AS
DECLARE @ErrorMessage NVARCHAR(4000),
			@ErrorSeverity INT,
			@ErrorState INT 
	SELECT 
        @ErrorMessage = 'SP(' + @SP + '): ' + ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE()

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               )


