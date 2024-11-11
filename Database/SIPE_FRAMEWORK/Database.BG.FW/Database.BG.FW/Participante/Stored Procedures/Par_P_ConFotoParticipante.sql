
CREATE  PROCEDURE [Participante].[Par_P_ConFotoParticipante]
@PI_IdParticipante int
AS
	SELECT NombreArchivo,	Documento
	FROM Participante.Par_DocumentoParticipante 
	WHERE IdParticipante = @PI_IdParticipante
		AND IdDocumento = 0		





