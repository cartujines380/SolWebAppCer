
CREATE Function [Participante].[Par_F_ConClienteOficGes]
(@PI_IdEmpresa int, @PI_IdParticipante int)

RETURNS int
AS	
	


BEGIN
	declare	
		@VL_IdOficinaGestion  int
	select 
	
	@VL_IdOficinaGestion = isNull(IdOficinaGestion,-1)
	from Participante.Par_Cliente 
	where
	IdEmpresa      = @PI_IdEmpresa and
	IdParticipante = @PI_IdParticipante
	return @VL_IdOficinaGestion
END





