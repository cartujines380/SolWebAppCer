


create procedure [Seguridad].[Seg_P_ACTUALIZA_OPCIONTRANS]
@PV_idTransaccion     int,
       @PV_opcion            int,
       @PV_descripcion       VARCHAR(100 ),
       @PV_nivel             int,
       @PV_organizacion      int
AS 
    UPDATE Seguridad.Seg_OPCIONTRANS
            SET descripcion=@PV_descripcion,
                nivel=@PV_nivel
     WHERE idtransaccion=@PV_idTransaccion AND idopcion=@PV_opcion
           AND IdOrganizacion = @PV_organizacion
     --COMMIT
   





