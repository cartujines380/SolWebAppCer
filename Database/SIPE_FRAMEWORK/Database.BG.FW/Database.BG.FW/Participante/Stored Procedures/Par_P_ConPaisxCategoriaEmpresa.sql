CREATE PROCEDURE [Participante].[Par_P_ConPaisxCategoriaEmpresa]
@PI_IdEmpresaPadre int,
@PI_IdCategoriaEmpresa int
as

	SELECT c.Codigo as IdPais, c.Descripcion as Pais, e.IdParticipante, e.Nombre
	FROM Participante.Par_Participante p, Participante.Par_Empresa e, Catalogo..Ctl_Catalogo c
	WHERE p.IdParticipante = e.IdParticipante
		AND e.IdCategoriaEmpresa = @PI_IdCategoriaEmpresa
		AND e.IdEmpresaPadre = @PI_IdEmpresaPadre
		AND p.IdPais = c.Codigo
		AND c.IdTabla = 2 --Catalogo Pais









