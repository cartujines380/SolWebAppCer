create function [Catalogo].[Ctl_F_Mensaje](@PI_IdMensaje int)
returns varchar(200)
AS
BEGIN
	RETURN (SELECT Mensaje FROM Catalogo.Ctl_Mensaje
		WHERE IdMensaje = @PI_IdMensaje)

END




