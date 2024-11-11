



/*==============================================================*/
/* MANTENIMIENTO: TRANSUSUARIO    */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_TRANSUSUARIO] 
@PV_idorganizacion      int,
      @PV_idTransaccion          int,
      @PV_idOpcion               int,
      @PV_estado                 CHAR,
      @PV_fechaInicial           DATETIME,
      @PV_fechaFinal             DATETIME,
      @PV_idHorario              int,
      @PV_tipoIdentificacion     CHAR,
      @PV_idIdentificacion       VARCHAR(100 ),
      @PV_usrReemplaza           VARCHAR(20 ),
      @PV_idUsuario              VARCHAR(20 )
AS 
   DECLARE @lv_Opcion int
	SET @lv_Opcion = 0
   
   
        Select @lv_Opcion = count(*)
        From Seguridad.Seg_TRANSUSUARIO
        Where idorganizacion = @PV_idorganizacion
        And idtransaccion    = @PV_idTransaccion
        And idopcion         = @PV_idOpcion
        And idusuario        = @PV_idUsuario
        
        If ISNULL(@lv_Opcion,0) = 0 
           INSERT INTO Seguridad.Seg_TRANSUSUARIO(idorganizacion, idtransaccion,idopcion,estado,fechainicial,
      fechafinal,idhorario,tipoidentificacion,
      ididentificacion,usrreemplaza,idusuario)
                  VALUES(@PV_idorganizacion,@PV_idTransaccion,@PV_idOpcion,@PV_estado,@PV_fechaInicial,
@PV_fechaFinal,@PV_idHorario,@PV_tipoIdentificacion,
@PV_idIdentificacion,@PV_usrReemplaza,@PV_idUsuario)
        else
          exec  Seguridad.Seg_P_ACTUALIZA_TRANSUSUARIO
        @PV_idTransaccion,
        @PV_idOpcion,
        @PV_estado,
        @PV_fechaInicial,
        @PV_fechaFinal,
        @PV_idHorario,
        @PV_tipoIdentificacion,
        @PV_idIdentificacion,
        @PV_usrReemplaza,
        @PV_idUsuario,
        @PV_idorganizacion
        
     --COMMIT
  





