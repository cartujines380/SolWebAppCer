
CREATE FUNCTION [Seguridad].[Seg_F_getHorario]
	( @PI_IdHorario int)
RETURNS VARCHAR(100)
BEGIN

DECLARE @VL_Descripcion VARCHAR(100)

SELECT
	@VL_Descripcion = Descripcion
FROM	Seguridad.Seg_Horario
WHERE
	IdHorario = @PI_IdHorario

RETURN @VL_Descripcion

END




