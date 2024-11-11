
create function [Participante].[Par_F_getOpidentParticipante] ( @PI_IdParticipante int)
RETURNS varchar(10)
AS
BEGIN
	DECLARE @VL_Opident varchar(10)
	SELECT @VL_Opident = Opident
	FROM Participante.Par_Participante
	WHERE IDParticipante = @PI_IdParticipante
	RETURN @VL_Opident

END





