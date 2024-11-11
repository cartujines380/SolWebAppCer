



create procedure [Seguridad].[Seg_P_ELIMINA_USERREEMPTRAN]
@PV_idOrg           int,
      @PV_idtran             int,
      @PV_idOpc              int,
      @PV_idUsuario          VARCHAR(20 ),
      @PV_usrReemplazo       VARCHAR(20 )
AS
        Delete From  Seguridad.Seg_TRANSUSUARIO
        Where IDUSUARIO          =  @PV_usrReemplazo
              And IDORGANIZACION =  @PV_idOrg
              And IDTRANSACCION  =  @PV_idtran
              And IDOPCION       =  @PV_idOpc
              And USRREEMPLAZA   =  @PV_idUsuario
   
    --COMMIT
 





