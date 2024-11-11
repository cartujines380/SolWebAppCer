create function [Participante].[Par_F_Msg](@PI_CodMsg int)
returns varchar(200)
AS
BEGIN
	RETURN (SELECT Mensaje FROM Participante.Par_P_Msg 
		WHERE IdMensaje = @PI_CodMsg)

END




