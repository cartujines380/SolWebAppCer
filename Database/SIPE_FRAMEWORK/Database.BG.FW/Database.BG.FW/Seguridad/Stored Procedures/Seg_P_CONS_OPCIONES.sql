
create procedure [Seguridad].[Seg_P_CONS_OPCIONES]  
@PV_idTransaccion        int,
  @PV_idorganizacion        int
AS
         SELECT distinct ot.IdOpcion, ot.Descripcion
         FROM Seguridad.Seg_OpcionTrans ot
         WHERE ot.IdTransaccion = @PV_idTransaccion
               AND ot.idorganizacion = @PV_idorganizacion

   






