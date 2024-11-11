create function [Catalogo].[Ctl_F_conCatalogoDescAlt](@PI_IdTabla int, @PI_Codigo varchar(50))
returns varchar(150)
AS
BEGIN
	RETURN (SELECT DescAlterno FROM Catalogo.Ctl_Catalogo
		WHERE IdTabla = @PI_IdTabla AND Codigo = @PI_Codigo)
END



