CREATE  procedure [Participante].[Par_P_ConsAgencias]
@PI_IdEmpresa int
AS
	Select p.IdParticipante,e.Nombre, c.Descripcion as Ciudad
	FROM ( Participante.Par_Empresa e INNER JOIN Participante.Par_Participante P
		ON e.IdParticipante = p.IdParticipante ) 
			INNER JOIN Catalogo..Ctl_Catalogo c
		ON p.IdCiudad = c.Codigo AND c.IdTabla = 4
	WHERE IdEmpresaPadre = @PI_IdEmpresa
		AND p.Estado = 'A' AND e.IdCategoriaEmpresa in (3,4)
	order by c.Descripcion




