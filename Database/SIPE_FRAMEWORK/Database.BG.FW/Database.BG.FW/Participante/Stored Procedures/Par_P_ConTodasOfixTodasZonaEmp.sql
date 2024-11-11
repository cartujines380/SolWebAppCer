
CREATE PROCEDURE [Participante].[Par_P_ConTodasOfixTodasZonaEmp]
@PI_IdParticipante int
AS
--Este procedimiento presenta todas las oficinas de una empresa especifica
SELECT e2.IdParticipante, e2.Nombre
FROM Participante.Par_Oficina o, Participante.Par_Empresa e2 ,Catalogo..Ctl_CatalogoEmpresa c
WHERE o.IdEmpresa = @PI_IdParticipante
	and o.IdOficina = e2.IdParticipante
	and o.IdEmpresa = c.IdEmpresa
	and c.IdTabla = 1
	and o.IdZona = c.Codigo





