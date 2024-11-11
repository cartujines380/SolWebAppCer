



create procedure [Seguridad].[Seg_P_ACTUALIZA_REGISTRO]
@PV_idUsuario           VARCHAR(20 ),
    @PV_fechaIngreso        DATETIME,
    @PV_estado              CHAR,
    @PV_fechaSalida         DATETIME,
    @PV_token               VARCHAR(32 ),
    @PV_idAplicacion        int,
    @PV_fechaUltTrans       DATETIME,
    @PV_idIdentificacion    VARCHAR(100 )
AS
     UPDATE Seguridad.Seg_REGISTRO
            SET estado=@PV_estado,
                fechasalida=@PV_fechaSalida,
                token=@PV_token,
                idaplicacion=@PV_idAplicacion,
                fechaulttrans=@PV_fechaUltTrans,
                idIdentificacion=@PV_idIdentificacion
     WHERE idusuario=@PV_idUsuario AND
           fechaIngreso=@PV_fechaIngreso
     --COMMIT
 






