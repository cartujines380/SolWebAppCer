




/*==============================================================*/
/* MANTENIMIENTO: AUTORIZACIONUSUARIO  */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_AUTORIZUSUARIO]
@PV_idAutorizacion       int,
        @PV_idUsuario            VARCHAR(20 ),
        @PV_fechaInicio          DATETIME,
        @PV_fechaFin             DATETIME,
        @PV_numAutorizacion      int,
        @PV_valor                VARCHAR(50 ),
        @PV_userAut              VARCHAR(20 ),
        @PV_userAlt              VARCHAR(20 ),
        @PV_userJef              VARCHAR(20 )
AS
  DECLARE @lv_opcion int 
	SET @lv_opcion = 0
      
        SELECT @lv_opcion = count(*)
        From Seguridad.Seg_AUTORIZACIONUSUARIO
        Where idautorizacion = @PV_idAutorizacion
        And idusuario        = @PV_idUsuario
        
     IF ISNULL(@lv_opcion,0) = 0 
     
       INSERT INTO Seguridad.Seg_AUTORIZACIONUSUARIO(idautorizacion,idusuario,fechainicio,
         fechafin,numautorizacion,valor,IDUSUARIOAUTORIZADOR, USRALTERNO, USRJEFE)
              VALUES(@PV_idAutorizacion,@PV_idUsuario,@PV_fechaInicio,@PV_fechaFin,
                     @PV_numAutorizacion,@PV_valor,@PV_userAut, @PV_userAlt, @PV_userJef)
     ELSE
       exec  Seguridad.Seg_P_ACTUALIZA_AUTORIZUSUARIO
	@PV_idAutorizacion,
          @PV_idUsuario,
          @PV_fechaInicio,
          @PV_fechaFin,
          @PV_numAutorizacion,
          @PV_valor,
          @PV_userAut,
          @PV_userAlt,
          @PV_userJef
      
     --COMMIT
 





