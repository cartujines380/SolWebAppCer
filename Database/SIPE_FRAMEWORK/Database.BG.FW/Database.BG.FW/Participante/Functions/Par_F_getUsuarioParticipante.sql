CREATE function [Participante].[Par_F_getUsuarioParticipante]( @PI_IdParticipante int)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @VL_Usuario varchar(50)
	SELECT @VL_Usuario = IdUsuario
	FROM Par_Participante
	WHERE IDParticipante = @PI_IdParticipante
	RETURN @VL_Usuario

END


