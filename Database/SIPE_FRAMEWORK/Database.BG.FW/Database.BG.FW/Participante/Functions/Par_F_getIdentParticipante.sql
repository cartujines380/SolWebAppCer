create function [Participante].[Par_F_getIdentParticipante] ( @PI_IdParticipante int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Identificacion varchar(100)
	SELECT @VL_Identificacion = Identificacion
	FROM Participante.Par_Participante
	WHERE IDParticipante = @PI_IdParticipante
	RETURN @VL_Identificacion

END




