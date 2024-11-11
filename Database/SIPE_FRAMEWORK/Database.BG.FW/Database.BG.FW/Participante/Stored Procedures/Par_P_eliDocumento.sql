create procedure [Participante].[Par_P_eliDocumento]
	@PI_IdParticipante int
AS
	DELETE Participante.Par_DocumentoParticipante
	WHERE IdParticipante = @PI_IdParticipante





