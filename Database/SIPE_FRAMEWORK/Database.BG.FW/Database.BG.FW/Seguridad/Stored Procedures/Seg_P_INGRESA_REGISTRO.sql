



/*==============================================================*/
/* MANTENIMIENTO: REGISTRO        */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_REGISTRO]
@PV_idUsuario            VARCHAR(20 ),
  @PV_estado               CHAR,
  @PV_fechaSalida          DATETIME,
  @PV_token                VARCHAR(32 ),
  @PV_idAplicacion         int,
  @PV_fechaUltTrans        DATETIME,
  @PV_idIdentificacion     VARCHAR(100 )
AS 
     INSERT INTO Seguridad.Seg_REGISTRO(idusuario,fechaingreso,estado,fechasalida,
 token,idaplicacion,fechaulttrans,idIdentificacion)
            VALUES(@PV_idUsuario,getdate(),@PV_estado,
                   @PV_fechaSalida,@PV_token,@PV_idAplicacion,@PV_fechaUltTrans,@PV_idIdentificacion)
     --COMMIT
  





