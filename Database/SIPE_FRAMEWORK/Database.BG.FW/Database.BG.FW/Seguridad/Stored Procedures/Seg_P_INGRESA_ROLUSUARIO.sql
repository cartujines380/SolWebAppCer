



create procedure [Seguridad].[Seg_P_INGRESA_ROLUSUARIO]
@PV_idRol                int,
     @PV_idUsuario              VARCHAR( 20),
     @PV_idHorario              int,
     @PV_estado                 CHAR,
     @PV_fehaInicial            DATETIME,
     @PV_fechaFinal             DATETIME,
     @PV_tipoIdentificacion     CHAR,
     @PV_identificacion         VARCHAR(100 ),
     @PV_usrReemplazo           VARCHAR( 20)
AS 
 DECLARE   @lv_opcion int
	SET @lv_opcion = 0

      Select @lv_opcion = count(idrol) 
        From  Seguridad.Seg_ROLUSUARIO
        Where idrol =               @PV_idRol
              And idusuario =       @PV_idUsuario
    
        If ISNULL(@lv_opcion,0) = 0 
        BEGIN
           INSERT INTO Seguridad.Seg_ROLUSUARIO(idrol,idusuario,idhorario,estado,fechainicial,
   fechafinal,tipoidentificacion,ididentificacion,
   usrreemplazo)
            VALUES(@PV_idRol,@PV_idUsuario,@PV_idHorario,@PV_estado,@PV_fehaInicial,
                   @PV_fechaFinal,@PV_tipoIdentificacion,@PV_identificacion,
                   @PV_usrReemplazo)
        END 
	else
exec  Seguridad.Seg_P_ACTUALIZA_ROLUSUARIO
      @PV_idRol,
      @PV_idUsuario,
      @PV_idHorario,
      @PV_estado,
      @PV_fehaInicial,
      @PV_fechaFinal,
      @PV_tipoIdentificacion,
      @PV_identificacion,
      @PV_usrReemplazo        
    
     --COMMIT






