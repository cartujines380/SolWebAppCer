



create procedure [Seguridad].[Seg_P_ACTUALIZA_AUTORIZUSUARIO]
@PV_idAutorizacion       int,
          @PV_idUsuario            VARCHAR(20 ),
          @PV_fechaInicio          DATETIME,
          @PV_fechaFin             DATETIME,
          @PV_numAutorizacion      int,
          @PV_valor                VARCHAR(50 ),
          @PV_userAut              VARCHAR(20 ),
          @PV_UsrAlterno           VARCHAR(20 ),
          @PV_UsrJefe              VARCHAR( 20)
AS
     UPDATE Seguridad.Seg_AUTORIZACIONUSUARIO
            SET fechainicio=@PV_fechaInicio,
                fechafin=@PV_fechaFin,
                numautorizacion=@PV_numAutorizacion,
                valor=@PV_valor,
                idusuarioAutorizador=@PV_userAut,
                usralterno=@PV_UsrAlterno,
                usrjefe=@PV_UsrJefe
     WHERE idautorizacion=@PV_idAutorizacion AND idusuario=@PV_idUsuario
     --COMMIT






