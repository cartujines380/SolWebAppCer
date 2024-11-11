create function [Catalogo].[Ctl_F_conCatalogo](@PI_IdTabla int, @PI_Codigo varchar(50))
returns varchar(150)
AS
BEGIN
	DECLARE @Descripcion varchar(max)
	
	RETURN (SELECT Descripcion FROM Catalogo.Ctl_Catalogo
		WHERE IdTabla = @PI_IdTabla AND Codigo = @PI_Codigo)

END




