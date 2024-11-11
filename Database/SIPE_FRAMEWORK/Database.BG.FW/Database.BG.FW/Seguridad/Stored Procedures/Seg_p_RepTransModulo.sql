


create procedure [Seguridad].[Seg_p_RepTransModulo]
as

select o.Descripcion, CantTrans = count(*)
from Seguridad.Seg_transaccion t,Seguridad.Seg_organizacion o
where t.idorganizacion = o.idorganizacion
group by o.Descripcion










