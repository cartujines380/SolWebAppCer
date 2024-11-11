


create procedure [Seguridad].[Seg_P_ACTUALIZA_AUTORIZACION]
@PV_idAutorizacion       int,
        @PV_idTransaccion        int,
        @PV_idOpcion             int,
        @PV_idOrganizacion       int,
        @PV_idHorario            int,
        @PV_parametro            VARCHAR(100 ),
        @PV_operador             VARCHAR(50 ),
        @PV_valorAutorizado      VARCHAR(50 )
AS 

     UPDATE Seguridad.Seg_AUTORIZACION
            SET idtransaccion=@PV_idTransaccion,
                idopcion=@PV_idOpcion,
                idOrganizacion=@PV_idOrganizacion,
                idhorario=@PV_idHorario,
                parametro=@PV_parametro,
                operador=@PV_operador,
                valorautorizado=@PV_valorAutorizado
     WHERE idautorizacion=@PV_idAutorizacion
     --COMMIT
  





