CREATE VIEW [Participante].[Par_V_EmpresaParGen]
AS
	SELECT DISTINCT e1.IdParticipante, e1.Nombre 
	FROM Participante.Par_Empresa e, Participante.Par_Empresa e1, Participante.Par_CategoriaEmpresa c
	WHERE c.IdCategoriaEmpresa = e.IdCategoriaEmpresa 
	      and (e.IdEmpresaPadre = e1.IdEmpresaPadre
		   or e.IdEmpresaPadre = e1.IdParticipante)
	      and e.IdEmpresaPadre=1
	/*	SELECT DISTINCT e1.IdParticipante, e1.Nombre 
	FROM Participante.Par_Empresa e, Participante.Par_Empresa e1, Participante.Par_CategoriaEmpresa c
	WHERE c.IdCategoriaEmpresa = e.IdCategoriaEmpresa and UPPER(c.descripcion) <> 'oficina' and  UPPER(c.descripcion) <>'pista'
	      and (e.IdEmpresaPadre = e1.IdEmpresaPadre
		   or e.IdEmpresaPadre = e1.IdParticipante)		 */





