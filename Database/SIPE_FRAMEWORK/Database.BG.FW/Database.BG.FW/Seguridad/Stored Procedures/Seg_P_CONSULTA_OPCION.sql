


create procedure [Seguridad].[Seg_P_CONSULTA_OPCION]  
@PV_idTransaccion      int,
     @PV_idOrganizacion       int
AS
         SELECT distinct ht.IdOpcion, ot.Descripcion
         FROM Seguridad.Seg_HorarioTrans ht, Seguridad.Seg_OpcionTrans ot
         WHERE ht.IdOrganizacion= @PV_idOrganizacion AND 
               ht.IdTransaccion = @PV_idTransaccion AND
              (ht.IdOrganizacion=ot.IdOrganizacion AND 
               ht.IdTransaccion=ot.IdTransaccion AND
               ht.IdOpcion= ot.IdOpcion)
 






