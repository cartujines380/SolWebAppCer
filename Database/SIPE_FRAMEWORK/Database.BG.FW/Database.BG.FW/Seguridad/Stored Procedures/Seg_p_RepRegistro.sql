

create procedure [Seguridad].[Seg_p_RepRegistro]
as

select IdUsuario, Fecha = convert(char,FechaIngreso,110)
from Seguridad.Seg_registro









