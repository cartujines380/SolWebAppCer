


create procedure [Seguridad].[Seg_p_RepTransModuloGen]
as

select o.idorganizacion,o.Descripcion, t.idtransaccion, t.descripcion
from Seguridad.Seg_transaccion t,Seguridad.Seg_organizacion o
where t.idorganizacion = o.idorganizacion
order by o.idorganizacion








