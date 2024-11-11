



create procedure [Seguridad].[Seg_P_VERIFICA_TRAN_USUAREEMP]
@PV_idOrg             int,
      @PV_idtran                 int,
      @PV_idOpc                  int,
      @PV_idUsuario              VARCHAR(20 ),
      @PV_usrReemplazo           VARCHAR(20 ),
      @PV_retorna                CHAR OUTPUT
AS
  declare   @lv_opcion              int
  
   Select @lv_opcion = count(*) 
        From  Seguridad.Seg_TRANSUSUARIO
        Where IDUSUARIO          =  @PV_usrReemplazo
              And IDORGANIZACION =  @PV_idOrg
              And IDTRANSACCION  =  @PV_idtran
              And IDOPCION       =  @PV_idOpc
   
   IF ISNULL(@lv_opcion,0) = 0 
     set @PV_retorna='0'
   else
        set  @PV_retorna='-1'
  






