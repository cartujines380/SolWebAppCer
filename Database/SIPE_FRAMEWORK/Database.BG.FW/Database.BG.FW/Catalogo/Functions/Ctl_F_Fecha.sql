CREATE FUNCTION [Catalogo].[Ctl_F_Fecha](@Fecha datetime)
RETURNS char(10)
AS
BEGIN
	RETURN CONVERT(char(10),@Fecha,110)
END




