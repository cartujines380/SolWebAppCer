
create function [Seguridad].[Seg_F_getUrlAplicacion]( @PI_IdAplicacion int)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @VL_Url varchar(100)
	SELECT @VL_Url = Valor 
	FROM Seguridad.Seg_ParamAplicacion
	WHERE IdAplicacion = @PI_IdAplicacion
			AND Parametro = 'Url'
	RETURN @VL_Url

END




/*
Seguridad.Seg_F_getUrlAplicacion

*/


