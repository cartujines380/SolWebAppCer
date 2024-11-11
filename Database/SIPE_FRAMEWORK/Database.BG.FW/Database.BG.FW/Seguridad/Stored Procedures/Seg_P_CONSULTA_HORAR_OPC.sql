
CREATE procedure [Seguridad].[Seg_P_CONSULTA_HORAR_OPC]
	@PV_idTransaccion      int,
        @PV_idOrganizacion     int
	
AS
  select  ht.IdOpcion as IdOpcion, ht.IdHorario as idhorario, h.Descripcion as Descripcion
    from Seguridad.Seg_HORARIOTRANS ht, Seguridad.Seg_HORARIO h
    where ht.idTransaccion=@PV_idTransaccion and
     	 ht.idOrganizacion=@PV_idOrganizacion and	 
     	 ht.idHorario= h.idHorario





