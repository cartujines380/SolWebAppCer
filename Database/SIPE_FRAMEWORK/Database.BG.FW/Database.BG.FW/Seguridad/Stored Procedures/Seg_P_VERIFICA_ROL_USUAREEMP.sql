



create procedure [Seguridad].[Seg_P_VERIFICA_ROL_USUAREEMP]
@PV_idRol             int,
          @PV_usrReemplazo      VARCHAR(20 ),
          @PV_retorna           CHAR OUTPUT
AS 
declare   @lv_opcion              int
 
   Select @lv_opcion = count(idrol) 
        From  Seguridad.Seg_ROLUSUARIO
        Where idrol = @PV_idRol And idusuario = @PV_usrReemplazo
   
   IF ISNULL(@lv_opcion,0) = 0 
     set @PV_retorna='0'
   else
          set @PV_retorna='-1'
  





