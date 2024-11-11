create function [Participante].[Par_F_getNombreUsuario] ( @PI_IdUsuario varchar(20))
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Nombre varchar(100)
	
	SELECT @VL_Nombre = pe.Nombre1 + ' ' + pe.Apellido1 
	FROM Participante.Par_Participante p, Participante.Par_Persona pe
	WHERE p.IdUsuario = @PI_IdUsuario
	      AND p.IdParticipante = pe.IdParticipante
	
	RETURN @VL_Nombre

END




