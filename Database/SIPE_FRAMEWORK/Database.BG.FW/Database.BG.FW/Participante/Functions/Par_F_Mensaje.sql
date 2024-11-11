create function [Participante].[Par_F_Mensaje](@PI_CodMsg int)
returns varchar(200)
AS
BEGIN
	RETURN (SELECT Mensaje FROM Participante.Par_Mensaje
		WHERE IdMensaje = @PI_CodMsg)

END




