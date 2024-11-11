CREATE VIEW [Participante].[Par_V_RegistroCliente]
AS
	SELECT p.IdParticipante, p.IdUsuario, p.IdTipoIdentificacion,
		p.Identificacion,TipoParticipante = 'Persona',
		Participante.Par_F_getNombreParticipante(p.IdParticipante) as Nombre,
		pe.IdEmpRel  as IdEmpPadre, 
		Participante.Par_F_getNombreParticipante(pe.IdEmpRel) as NombreEmpPadre
	FROM Participante.Par_RegistroCliente r,Participante.Par_Persona pe,Participante.Par_Participante  p
	WHERE r.IdParticipante = pe.IdParticipante
		AND pe.IdParticipante = p.IdParticipante
	
	UNION

	SELECT p.IdParticipante, p.IdUsuario, p.IdTipoIdentificacion,
		p.Identificacion, TipoParticipante = 'Empresa',
        e.Nombre,
		e.IdEmpresaPadre as IdEmpPadre,
		Participante.Par_F_getNombreParticipante(e.IdEmpresaPadre) as NombreEmpPadre		
	FROM Participante.Par_RegistroCliente r, Participante.Par_Empresa e, Participante.Par_Participante p
	WHERE r.IdParticipante = e.IdParticipante
		AND e.IdParticipante = p.IdParticipante








