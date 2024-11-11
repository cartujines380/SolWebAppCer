Create procedure [Participante].[Par_P_ConDirParticipanteContacto]
@PI_IdParticipante int

as

	SELECT distinct d.IdDireccion, d.IdTipoDireccion, d.Direccion, d.IdPais,
		d.IdProvincia, d.IdCiudad
	FROM Participante.Par_Direccion d
	WHERE   d.IdParticipante = @PI_IdParticipante





