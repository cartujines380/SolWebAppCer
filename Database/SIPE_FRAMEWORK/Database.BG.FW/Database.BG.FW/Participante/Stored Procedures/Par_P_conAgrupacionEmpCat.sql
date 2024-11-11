create procedure [Participante].[Par_P_conAgrupacionEmpCat]
as

select c.Descripcion,IdParticipante,Nombre 
from Participante.Par_empresa e, Participante.Par_categoriaEmpresa c
where e.IdCategoriaEmpresa = c.IdCategoriaEmpresa






