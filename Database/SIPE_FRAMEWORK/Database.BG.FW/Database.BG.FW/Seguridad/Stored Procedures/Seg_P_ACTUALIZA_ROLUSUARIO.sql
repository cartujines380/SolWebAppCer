




/*==============================================================*/
/* MANTENIMIENTO: ROLUSUARIO      */
/*==============================================================*/

create procedure [Seguridad].[Seg_P_ACTUALIZA_ROLUSUARIO]
@PV_idRol                  int,
      @PV_idUsuario              VARCHAR(20 ),
      @PV_idHorario              int,
      @PV_estado                 CHAR,
      @PV_fechaIncial            DATETIME,
      @PV_fechaFinal             DATETIME,
      @PV_tipoIdentificacion     CHAR,
      @PV_identificacion         VARCHAR(100 ),
      @PV_usrReemplazo           VARCHAR( 20)
AS
 
     UPDATE Seguridad.Seg_ROLUSUARIO
            SET idhorario         =@PV_idHorario,
                estado            =@PV_estado,
                fechainicial      =@PV_fechaIncial,
                fechafinal        =@PV_fechaFinal,
                tipoidentificacion=@PV_tipoIdentificacion,
                ididentificacion  =@PV_identificacion,
                usrreemplazo      =@PV_usrReemplazo
     WHERE idrol=@PV_idRol AND idusuario=@PV_idUsuario
     --COMMIT
 





