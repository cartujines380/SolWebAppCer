


create procedure [Seguridad].[Seg_P_RepTransaccion]
AS
select o.Descripcion, t.IdTransaccion
 from Seguridad.Seg_organizacion o,Seguridad.Seg_transaccion t
where o.Idorganizacion = t.Idorganizacion
order by o.Descripcion








