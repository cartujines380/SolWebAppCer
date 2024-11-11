
CREATE PROCEDURE [Participante].[Par_P_ConNaturalezaEmpresa]
	@PI_IdEmpresa  int,
	@PI_IdNaturalezaNegocio tinyint
AS
	
SELECT emp.IdParticipante, emp.Nombre
	FROM   Participante.Par_Empresa emp inner join Participante.Par_Participante p on emp.IdEmpresaPadre= p.IdParticipante
	WHERE  emp.IdEmpresaPadre = @PI_IdEmpresa		
		And p.IdNaturalezaNegocio = @PI_IdNaturalezaNegocio




