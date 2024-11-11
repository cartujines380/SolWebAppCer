
CREATE PROCEDURE [Participante].[Par_P_ConsOficinas]
@PI_IdEmpresa int
AS

	SELECT o.IdOficina as IdParticipante , e.Nombre, o.IdZona, o.OficinaSRI
	FROM (Participante.Par_Oficina o INNER JOIN Participante.Par_Empresa e
		ON o.IdOficina = e.IdParticipante) INNER JOIN Participante.Par_Participante p
			ON e.IdParticipante = p.IdParticipante
	WHERE o.IdEmpresa = @PI_IdEmpresa
		AND p.Estado = 'A'






