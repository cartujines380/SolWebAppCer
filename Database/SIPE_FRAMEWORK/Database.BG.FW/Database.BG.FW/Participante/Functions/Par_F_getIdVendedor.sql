create function [Participante].[Par_F_getIdVendedor](@PI_IdCliente int, @PI_IdEmpresa int)
RETURNS int
AS
BEGIN
	--si c.IdVendedor es igual a "-1", no existe vendedor asignado
	return(SELECT c.IdVendedor
	FROM Participante.Par_Cliente c
	WHERE c.IdParticipante = @PI_IdCliente
	      AND c.IdEmpresa = @PI_IdEmpresa)
	
END






