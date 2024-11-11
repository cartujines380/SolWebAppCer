
create procedure [Seguridad].[Seg_P_ACTUALIZA_TOKEN]
@PV_idUsuario            VARCHAR(20),
 @PV_Token                VARCHAR(32),
 @PV_Maquina              VARCHAR(100),
 @PV_idAplicacion         INT
AS
        UPDATE Seguridad.Seg_REGISTRO
        SET FechaUltTrans = getdate(), IdAplicacion = @PV_idAplicacion
        WHERE IdUsuario = @PV_idUsuario --AND Token = @PV_Token
		AND IdIdentificacion = @PV_Maquina
      --  COMMIT
  






