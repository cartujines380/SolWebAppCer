
create   VIEW [Participante].[Par_V_EmpresaPadre]
AS
	SELECT p.IdParticipante,p.Identificacion,p.IdUsuario,
    		e.Nombre,e.IdCategoriaEmpresa as IdCategoriaPadre
    	FROM Participante.Par_Empresa e, Participante.Par_Participante p
    	WHERE e.IdParticipante = p.IdParticipante









