CREATE FUNCTION [Participante].[Par_F_TamañoHac](@PI_IdEmpresa int)
returns float(8)
AS
Begin
	return(SELECT SUM(Tamaño) as Tamaño
	FROM Participante.Par_LoteHac
	WHERE IdEmpresa = @PI_IdEmpresa)
end





