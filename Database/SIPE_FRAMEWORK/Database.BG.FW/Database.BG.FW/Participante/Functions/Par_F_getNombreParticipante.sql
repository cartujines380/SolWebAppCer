CREATE function [Participante].[Par_F_getNombreParticipante] ( @PI_IdParticipante int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Nombre varchar(100)
	
	SELECT @VL_Nombre = isnull(Apellido1,'') + ' ' +  isnull(Apellido2,'') + ' ' + isnull(Nombre1,'') + ' ' + isnull(Nombre2,'')
	FROM Participante.Par_Persona
	WHERE IDParticipante = @PI_IdParticipante
	IF @VL_Nombre is null
	BEGIN
		SELECT @VL_Nombre = Nombre 
		FROM Participante.Par_Empresa
		WHERE IDParticipante = @PI_IdParticipante
	END
	RETURN @VL_Nombre

END





