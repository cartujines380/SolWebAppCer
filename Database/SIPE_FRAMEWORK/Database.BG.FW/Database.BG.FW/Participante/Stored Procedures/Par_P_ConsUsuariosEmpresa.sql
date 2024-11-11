CREATE Procedure [Participante].[Par_P_ConsUsuariosEmpresa]
@IdEmpresa int
AS
	
select p.IdUsuario
FROM Participante.Par_Empleado e INNER JOIN
	Participante.Par_Participante p
	ON e.IdParticipante = p.IdParticipante
WHERE e.IdEmpresa = @IdEmpresa  AND p.Estado = 'A'



