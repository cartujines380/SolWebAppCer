
create  procedure [Participante].[Par_P_ConEmpresaHijo]
@PI_IdEmpresa int
AS
	SELECT  ei.IdParticipante as IdEmpresa, ei.Nombre as NombreEmpresa,
		ei.IdCategoriaEmpresa, Participante.Par_F_getNombreCategoriaEmp(ei.IdCategoriaEmpresa) CategoriaEmpresa
	FROM  Participante.Par_Empresa e, Participante.Par_Empresa ei
	WHERE e.IdParticipante = @PI_IdEmpresa
		and e.IdCategoriaEmpresa = 1 --Grupo
		and e.IdParticipante  = ei.IdEmpresaPadre






