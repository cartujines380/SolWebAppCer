CREATE PROCEDURE [Participante].[Par_P_ConEmpleadoEmpresaExt] 
@PI_IdEmpresa int
AS
	SELECT e.IdParticipante, Participante.Par_F_getNombreParticipante(e.IdParticipante) as NombreEmpleado
	FROM Participante.Par_Participante p, Participante.Par_Empleado e 
	where e.IdEmpresaPertenece = @PI_IdEmpresa
		and p.IdParticipante = e.IdParticipante
		




