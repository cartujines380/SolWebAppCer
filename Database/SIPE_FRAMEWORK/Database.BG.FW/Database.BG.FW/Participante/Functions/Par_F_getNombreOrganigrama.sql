CREATE function [Participante].[Par_F_getNombreOrganigrama] (@PI_IdEmpresa int, @PI_IdOrganigrama int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Nombre varchar(100)
	SELECT @VL_Nombre = Descripcion
	FROM Participante.Par_Organigrama
	WHERE IdEmpresa = @PI_IdEmpresa
		and IdOrganigrama = @PI_IdOrganigrama 

	RETURN @VL_Nombre

END







