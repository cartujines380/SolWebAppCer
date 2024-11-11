



create procedure [Seguridad].[Seg_P_ELIMINA_OPCIONTRANSROL]
@PV_idTransaccion     int,
        @PV_idOpcion          int,
        @PV_idOrganiza        int,
        @PV_idRol             int
AS
     DELETE FROM Seguridad.Seg_OPCIONTRANSROL
     WHERE idrol  =@PV_idRol AND
           idtransaccion                   =@PV_idTransaccion AND
           idorganizacion                  =@PV_idOrganiza AND
           idopcion                        =@PV_idOpcion
           

     --COMMIT
    





