



create procedure [Seguridad].[Seg_P_ACTUALIZA_USERREEMPTRAN]
@PV_idOrg           int,
      @PV_idtran                 int,
      @PV_idOpc                  int,
      @PV_idUsuario              VARCHAR(20 ),
      @PV_fechaIncial            DATETIME,
      @PV_fechaFinal             DATETIME,
      @PV_usrReemplazo           VARCHAR(20 )
AS 
   DECLARE @lv_opcion int 
      
   Select @lv_opcion = count(*)  
        From  Seguridad.Seg_TRANSUSUARIO
        Where IDUSUARIO          =  @PV_usrReemplazo
              And IDORGANIZACION =  @PV_idOrg
              And IDTRANSACCION  =  @PV_idtran
              And IDOPCION       =  @PV_idOpc
    
        If ISNULL(@lv_opcion,0) = 0 
         BEGIN
              Insert Into Seguridad.Seg_TRANSUSUARIO(IDORGANIZACION,IDTRANSACCION,IDOPCION,IDUSUARIO,
                     IDHORARIO,ESTADO,FECHAINICIAL,FECHAFINAL,USRREEMPLAZA)
              Select @PV_idOrg, @PV_idtran,@PV_idOpc,@PV_usrReemplazo,IDHORARIO,'A',
                     @PV_fechaIncial,@PV_fechaFinal,@PV_idUsuario
                  From Seguridad.Seg_TRANSUSUARIO
                  Where IDUSUARIO          =  @PV_idUsuario
                        And IDORGANIZACION =  @PV_idOrg
                        And IDTRANSACCION  =  @PV_idtran
                        And IDOPCION       =  @PV_idOpc
   
            --Inactiva los roles del usuario que se va            
                UPDATE Seguridad.Seg_TRANSUSUARIO
                SET estado            ='I',
                    fechainicial      =@PV_fechaIncial,
                    fechafinal        =@PV_fechaFinal
                  Where IDUSUARIO          =  @PV_idUsuario
                        And IDORGANIZACION =  @PV_idOrg
                        And IDTRANSACCION  =  @PV_idtran
                        And IDOPCION       =  @PV_idOpc
        END
                  
     --COMMIT
    





