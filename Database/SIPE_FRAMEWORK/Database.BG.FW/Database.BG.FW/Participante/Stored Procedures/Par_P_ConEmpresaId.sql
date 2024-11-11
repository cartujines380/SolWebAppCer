-- Participante.Par_P_ConEmpresaId 7
CREATE PROCEDURE [Participante].[Par_P_ConEmpresaId] 
@PI_IdParticipante int
AS
	
	SELECT 	e.Nombre, Participante.Par_F_getUsuarioParticipante(e.IdParticipante) IdUsuario,
			e.IdCategoriaEmpresa, ce.Descripcion as Categoria,ce.IdCategoriaEmpPadre,
			e.Nivel,e.IdEmpresaPadre,e.Licencia, e.Marca, e.NumeroPatronal,
			ep.IdUsuario as IdUsuEmpPadre,
			ep.Nombre as NombreEmpPadre,
			--e.IdZona,Catalogo.Ctl_F_conCatalogo(1,e.IdZona) as Zona,
			e.IdRazonSocial,Catalogo.Ctl_F_conCatalogo(213,e.IdRazonSocial) as RazonSocial		
	FROM	Participante.Par_Empresa e, Participante.Par_CategoriaEmpresa ce, 
			Participante.Par_V_EmpresaParticipante ep
	WHERE   e.IdParticipante = @PI_IdParticipante
		AND e.IdCategoriaEmpresa = ce.IdCategoriaEmpresa
		AND e.IdEmpresaPadre = ep.IdParticipante








