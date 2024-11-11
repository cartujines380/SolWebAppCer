CREATE function [Participante].[Par_F_getNombreCategoriaEmp](@PI_IdCategoria int)
RETURNS varchar(100)
AS
BEGIN
	return(SELECT Descripcion
	FROM Participante.Par_CategoriaEmpresa
	WHERE IdCategoriaEmpresa = @PI_IdCategoria) 

END





