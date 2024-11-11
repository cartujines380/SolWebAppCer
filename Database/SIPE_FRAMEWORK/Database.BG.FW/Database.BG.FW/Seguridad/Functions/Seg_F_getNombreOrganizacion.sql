
create function [Seguridad].[Seg_F_getNombreOrganizacion] ( @PI_IdOrganizacion int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Nombre varchar(100)
	SELECT @VL_Nombre = Descripcion
	FROM Seguridad.Seg_Organizacion
	WHERE IdOrganizacion = @PI_IdOrganizacion
	RETURN @VL_Nombre

END






