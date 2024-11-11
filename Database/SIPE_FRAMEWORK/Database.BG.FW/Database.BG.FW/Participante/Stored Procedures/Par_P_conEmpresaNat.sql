
CREATE PROCEDURE [Participante].[Par_P_conEmpresaNat]
	@PI_IdNaturalezaNegocio varchar(10)
AS

	
SELECT p.IdParticipante, 
Participante.Par_F_getNombreParticipante(p.IdParticipante) as Nombre
	FROM   Participante.Par_Participante p
	WHERE  p.IdNaturalezaNegocio = @PI_IdNaturalezaNegocio
	




