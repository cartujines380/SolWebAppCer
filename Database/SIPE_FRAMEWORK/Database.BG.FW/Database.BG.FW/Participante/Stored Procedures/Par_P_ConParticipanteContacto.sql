CREATE PROCEDURE [Participante].[Par_P_ConParticipanteContacto]
@PI_IdParticipante int
as
SELECT distinct p.Identificacion, p.IdTipoIdentificacion , p.IdUsuario,
		pe.Apellido1, pe.Apellido2, pe.Nombre1, pe.Nombre2,
		pe.IdTitulo, pe.Sexo,
		convert(varchar,pe.FechaNacimiento,101) as FechaNacimiento,
		pe.EstadoCivil, pe.Ruc
	FROM Participante.Par_Participante p inner join Participante.Par_Persona pe on p.IdParticipante = pe.IdParticipante
	WHERE   p.IdParticipante = @PI_IdParticipante




