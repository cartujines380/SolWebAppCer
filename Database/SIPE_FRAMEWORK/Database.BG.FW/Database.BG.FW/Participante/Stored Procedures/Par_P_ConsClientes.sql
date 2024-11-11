
CREATE Procedure [Participante].[Par_P_ConsClientes]
(
		@IdEmpresa int
)
AS
	SELECT e.IdParticipante, e.Nombre
	FROM Participante.Par_Cliente c INNER JOIN 
			Participante.Par_Empresa e
				ON c.IdParticipante = e.IdParticipante
	WHERE c.IdEmpresa = @IdEmpresa AND c.Estado = 'A'


