create  procedure [Participante].[Par_P_ConEmpresaGrupo]
@PI_IdEmpresa int
AS

	select e1.IdParticipante, e1.Nombre 
	from Participante.Par_Empresa e, Participante.Par_Empresa e1
	where e.IdParticipante = @PI_IdEmpresa
	      and (e.IdEmpresaPadre = e1.IdEmpresaPadre
		   or e.IdEmpresaPadre = e1.IdParticipante)




