


CREATE   view [Seguridad].[Seg_v_transaccion] as
select Trans.IdTransaccion, Trans.Descripcion,
       Org.Idorganizacion , Org.Descripcion as Organizacion
 From Seguridad.Seg_Transaccion Trans , Seguridad.Seg_Organizacion Org
 where Org.Idorganizacion = Trans.Idorganizacion







