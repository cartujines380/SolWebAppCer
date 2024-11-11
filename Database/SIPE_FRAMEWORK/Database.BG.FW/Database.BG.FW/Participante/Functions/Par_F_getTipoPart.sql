CREATE function [Participante].[Par_F_getTipoPart]( @PI_IdParticipante int)
RETURNS varchar(10)
AS
BEGIN
	DECLARE @VL_TipoPart varchar(10)
	IF EXISTS(SELECT 1 FROM Par_Empleado WHERE IdParticipante = @PI_IdParticipante)
		SET @VL_TipoPart = 'EMPLEADO'
	ELSE
		IF EXISTS(SELECT 1 FROM Par_Cliente WHERE IdParticipante = @PI_IdParticipante)
			SET @VL_TipoPart = 'CLIENTE'
		ELSE
			IF EXISTS(SELECT 1 FROM Par_Proveedor WHERE IdParticipante = @PI_IdParticipante)
				SET @VL_TipoPart = 'PROVEEDOR'
			ELSE
				SET @VL_TipoPart = 'USUARIO'
	RETURN @VL_TipoPart

END

