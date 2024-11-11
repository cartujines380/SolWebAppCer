


CREATE procedure [Seguridad].[Seg_P_ELIMINA_APLICACION]
 @PV_Aplicacion     int
AS
   DECLARE @lv_count               int
	SET  @lv_count = 0
   
    select @lv_count = count(*)
     from Seguridad.Seg_organizacion
    where idAplicacion=@PV_Aplicacion
    if  ISNULL(@lv_count,0) = 0 
     BEGIN
		begin tran
       Delete Seguridad.Seg_PARAMAPLICACION 
              Where idaplicacion =  @PV_Aplicacion 
       if (@@error<>0)
       begin
			rollback tran
			return
       end
       Delete Seguridad.Seg_ServAplicacion
              Where IdAplicacion =  @PV_Aplicacion 
       if (@@error<>0)
       begin
			rollback tran
			return
       end
       Delete Seguridad.Seg_PuertoAplicacion
              Where IdAplicacion =  @PV_Aplicacion 
       if (@@error<>0)
       begin
			rollback tran
			return
       end
       Delete Seguridad.Seg_APLICACION 
              Where idaplicacion =  @PV_Aplicacion 
       if (@@error<>0)
       begin
			rollback tran
			return
       end
       commit tran
       select '0'
     END 
     else            
        select '1'
 





