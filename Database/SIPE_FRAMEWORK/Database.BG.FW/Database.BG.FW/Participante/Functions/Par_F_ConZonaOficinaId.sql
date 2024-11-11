﻿CREATE FUNCTION [Participante].[Par_F_ConZonaOficinaId](@PI_IdEmpresa int, @PI_IdOficina int)
RETURNS int
begin
	return(SELECT o.IdZona
	FROM   Participante.Par_Oficina o
	WHERE  o.IdEmpresa = @PI_IdEmpresa
		and o.IdOficina = @PI_IdOficina)
	
end




