create view [Participante].[Par_V_ContactoDireccion]
AS
	SELECT IdParticipante, IdDireccion, max(IdMedioContacto) as Cantidad
	FROM Participante.Par_MedioContacto
	GROUP BY IdParticipante, IdDireccion





