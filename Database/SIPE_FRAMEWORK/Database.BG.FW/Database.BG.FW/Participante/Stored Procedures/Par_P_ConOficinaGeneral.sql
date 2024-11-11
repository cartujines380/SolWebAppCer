CREATE Procedure [Participante].[Par_P_ConOficinaGeneral]
@PI_IdEmpresaPadre int
AS
	Select p.IdParticipante,e.Nombre
	FROM (Participante.Par_Empresa e INNER JOIN Participante.Par_CategoriaEmpresa c
		ON e.IdCategoriaEmpresa = c.IdCategoriaEmpresa ) INNER JOIN Participante.Par_Participante P
		ON e.IdParticipante = p.IdParticipante
	WHERE IdEmpresaPadre = @PI_IdEmpresaPadre
		AND c.Descripcion = 'Oficina'
		AND p.Estado = 'A'
		
		





