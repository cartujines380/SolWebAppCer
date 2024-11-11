CREATE PROCEDURE [Participante].[Par_P_ConEmpleadoCargo]
	@PI_IdEmpresa int,
	@PI_IdCargo int
AS
	SELECT
		e.IdParticipante,
		e.IdUsuario,
		e.Identificacion,
		NombreEmpleado = e.Nombre,
		e.IdCargo,
		e.Cargo,
		e.IdOficina,
		NombreOficina = e.Oficina,
		e.Estado,
		Correo = isnull( 
			(SELECT TOP 1 mc.Valor FROM Participante.Par_MedioContacto mc
				WHERE mc.IdParticipante = e.IdParticipante
					AND mc.IdTipoMedioContacto = 3
						ORDER BY IdDireccion, IdMedioContacto), '' )
	FROM Participante.Par_V_Empleado e
	WHERE	IdEmpresa = @PI_IdEmpresa
		And IdCargo = @PI_IdCargo
		And e.Estado = 'A'




