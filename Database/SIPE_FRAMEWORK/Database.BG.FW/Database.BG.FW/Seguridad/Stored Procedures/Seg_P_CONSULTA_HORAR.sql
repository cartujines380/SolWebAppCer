



create procedure [Seguridad].[Seg_P_CONSULTA_HORAR] 
@PV_idTransaccion        int,
    @PV_idOrganizacion     int,
    @PV_idOpcion           int
AS
         SELECT distinct ht.IdHorario, h.Descripcion
         FROM Seguridad.Seg_HorarioTrans ht, Seguridad.Seg_Horario h
         WHERE  ht.IdOrganizacion=@PV_idOrganizacion AND
                ht.IdTransaccion = @PV_idTransaccion AND
                ht.IdOpcion= @PV_idOpcion AND
                ht.IdHorario=h.IdHorario
  





