
CREATE FUNCTION [Participante].[Par_F_MedioContacto](
	@PI_IdParticipante int,
	@PI_IdDireccion int,
	@PI_IdTipoMedioContacto int)
RETURNS Varchar(50)
AS
BEGIN
	DECLARE @Valor varchar(50)
	SELECT @Valor = Valor
	FROM Participante.Par_MedioContacto
	where IdParticipante = @PI_IdParticipante
		and IdDireccion = @PI_IdDireccion
		and IdTipoMedioContacto = @PI_IdTipoMedioContacto
	RETURN @Valor
END

