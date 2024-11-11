


/*==============================================================*/
/* Valor autorizado para WorkFlow                       */
/*==============================================================*/

 create procedure [Seguridad].[Seg_P_VALORAUTORIZADO]
@PV_idOrg                 int,
      @PV_idtran             int,
      @PV_idOpc              int,
      @PV_idUsuario          VARCHAR(20 ),
      @PV_horario            int,
      @PV_Valor              VARCHAR(20 ) OUTPUT
AS
  DECLARE @lv_valor               VARCHAR(50)
 DECLARE  @ln_count               int
   
   
           Select  @ln_count = count(*)                      
           From Seguridad.Seg_autorizacionusuario au, 
             Seguridad.Seg_autorizacion a 
             where a.idtransaccion = @PV_idtran
             and a.idopcion        = @PV_idOpc
             and a.idorganizacion  = @PV_idOrg
             and a.idhorario       = @PV_horario
             and au.idautorizacion = a.idautorizacion
             and au.idusuario      = @PV_idUsuario
   
        If ISNULL(@ln_count,0) > 0  
        
          Select  @lv_valor = ISNULL(au.valor,0)
          From Seguridad.Seg_autorizacionusuario au, 
               Seguridad.Seg_autorizacion a 
               where a.idtransaccion = @PV_idtran
               and a.idopcion        = @PV_idOpc
               and a.idorganizacion  = @PV_idOrg
               and a.idhorario       = @PV_horario
               and au.idautorizacion = a.idautorizacion
               and au.idusuario      = @PV_idUsuario
             
     SET @PV_Valor = @lv_valor
     
  





