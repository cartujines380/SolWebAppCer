CREATE FUNCTION [Participante].[Par_F_MCParticipante](
	@PI_IdParticipante int,
	@PI_IdDireccion int,
	@PI_IdTipoMedioContacto int)
RETURNS TABLE
AS
RETURN(SELECT IdMedioContacto,Valor, isnull(ValorAlt,'') ValorAlt
	FROM Participante.Par_MedioContacto
	where IdParticipante = @PI_IdParticipante
		and IdDireccion = @PI_IdDireccion
		and IdTipoMedioContacto = @PI_IdTipoMedioContacto)








