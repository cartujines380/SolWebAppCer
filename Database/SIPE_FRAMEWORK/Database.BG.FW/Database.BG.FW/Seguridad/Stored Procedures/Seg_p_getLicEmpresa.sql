

/* Procedimientos clientes */

CREATE  procedure [Seguridad].[Seg_p_getLicEmpresa]
@IdEmpresa int
AS
	Select nombre,licencia
	FROM Participante.par_EMPRESA
	WHERE IdParticipante = @IdEmpresa







