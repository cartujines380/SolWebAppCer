
CREATE FUNCTION [Participante].[Par_F_ConZonaOficina](@PI_IdEmpresa int, @PI_IdOficina int)
RETURNS varchar(50)
begin
	return(SELECT Catalogo.Ctl_F_conCatalogo(1,o.IdZona)
	FROM   Participante.Par_Oficina o
	WHERE  o.IdEmpresa = @PI_IdEmpresa
		and o.IdOficina = @PI_IdOficina)
	
end






