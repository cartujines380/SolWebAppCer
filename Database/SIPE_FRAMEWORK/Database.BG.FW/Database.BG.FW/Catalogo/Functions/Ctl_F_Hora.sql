CREATE  FUNCTION [Catalogo].[Ctl_F_Hora](@Hora datetime)
RETURNS char(8)
AS
BEGIN
	RETURN CONVERT(char(8),@Hora,108)
END






