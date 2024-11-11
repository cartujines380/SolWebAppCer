

CREATE  procedure [Participante].[Par_P_ConEmpresa]
@PI_IdEmpresaPadre int,
@PI_Consulta int = 0 --1 si quiere el campo todos
AS
-- Si Pi_Consulta es 3 = trae todas las empresas sin importar el padre

	IF exists(select 1 from Participante.Par_Empresa 
		where IdEmpresaPadre = @PI_IdEmpresaPadre)
	BEGIN

		If @PI_Consulta = 0
		Begin
			Select p.IdParticipante,e.Nombre
			FROM Participante.Par_Empresa e INNER JOIN Participante.Par_Participante P
				ON e.IdParticipante = p.IdParticipante
			WHERE IdEmpresaPadre = @PI_IdEmpresaPadre
				AND p.Estado = 'A' AND e.IdCategoriaEmpresa in(2,3,5)
		End
		IF @PI_Consulta = 1 -- recupera todos
		Begin
			Select p.IdParticipante,e.Nombre
			FROM Participante.Par_Empresa e INNER JOIN Participante.Par_Participante P
				ON e.IdParticipante = p.IdParticipante
			WHERE IdEmpresaPadre = @PI_IdEmpresaPadre
				AND p.Estado = 'A' AND e.IdCategoriaEmpresa in (2,3,5)
			union 
			Select IdParticipante = @PI_IdEmpresaPadre, Nombre = 'Todos'
		End
		If @PI_Consulta = 2 -- todas las empresas
		Begin
			Select p.IdParticipante,e.Nombre
			FROM Participante.Par_Empresa e INNER JOIN Participante.Par_Participante P
				ON e.IdParticipante = p.IdParticipante
			WHERE p.Estado = 'A' AND e.IdCategoriaEmpresa in(2,3,5)
		End
	END
	ELSE
		Select 0 as IdParticipante, 'N/A' as Nombre







