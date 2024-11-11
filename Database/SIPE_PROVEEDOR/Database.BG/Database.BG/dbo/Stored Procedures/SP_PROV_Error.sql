
CREATE PROCEDURE [dbo].[SP_PROV_Error]
@sp varchar(100)
as
  Declare @ErrorMessage nvarchar(4000), @ErrorSeverity int, @ErrorState int 
  Select  @ErrorMessage  = 'SP(' +  @sp + ') : ' + ERROR_MESSAGE(),
          @ErrorSeverity = ERROR_SEVERITY(),  
          @ErrorState    = ERROR_STATE()
  IF @ErrorState = 0
       SET @ErrorState=1
  RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState)
  


