

create procedure [Seguridad].[Seg_P_CONSULTA_TRANS] 
@PV_idOrganizacion       int
AS
         SELECT distinct ht.idtransaccion, t.Descripcion
         FROM Seguridad.Seg_HorarioTrans ht, Seguridad.Seg_Transaccion t
         WHERE ht.IdOrganizacion=@PV_idOrganizacion AND   
            ht.IdTransaccion=t.IdTransaccion AND 
            t.idorganizacion=@PV_idOrganizacion
  





