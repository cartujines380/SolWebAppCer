



create procedure [Seguridad].[Seg_P_CONSULTA_HORARIOORGANIZ]
@PV_idHorario       int
AS

     select 1 Sec, ht.IdOrganizacion CodOrg, 
	o.Descripcion Organizacion,
     ht.IdTransaccion AS "o-CodTran", t.Descripcion Transaccion, 
     ht.IdOpcion AS "o-CodOp", ot.Descripcion Opcion
     from Seguridad.Seg_HorarioTrans ht, Seguridad.Seg_Organizacion o, Seguridad.Seg_Transaccion t, Seguridad.Seg_OpcionTrans ot
     where ht.IdHorario=@PV_idHorario and (
       ht.IdOrganizacion=o.IdOrganizacion and
      (ht.IdOrganizacion=t.IdOrganizacion and
       ht.IdTransaccion=t.IdTransaccion) and
      (ht.IdOrganizacion=ot.IdOrganizacion and
       ht.IdTransaccion=ot.IdTransaccion and
       ht.IdOpcion=ot.IdOpcion))
         






