
CREATE FUNCTION [Participante].[Par_F_getNatNegocioEmpresa]
(
	@PI_IdEmpresa int
)
RETURNS VARCHAR (50)
BEGIN

	DECLARE @VL_IdNaturalezaNegocio VARCHAR (50)

	SELECT @VL_IdNaturalezaNegocio = IdNaturalezaNegocio
	FROM Participante.Par_Participante
	WHERE IdParticipante = @PI_IdEmpresa

	RETURN @VL_IdNaturalezaNegocio

END




