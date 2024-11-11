
CREATE PROCEDURE [Participante].[Par_P_ConsEmpresaxCategoria]
@PI_IdEmpresaPadre int,
@PI_IdCategoriaEmpresa int
AS
	SELECT	p.IdParticipante, p.Identificacion, e.Nombre
	FROM	Participante.Par_Participante p 
			inner join Participante.Par_Empresa e on p.IdParticipante = e.IdParticipante 			
	WHERE	e.IdEmpresaPadre = @PI_IdEmpresaPadre
			and e.IdCategoriaEmpresa = @PI_IdCategoriaEmpresa		





