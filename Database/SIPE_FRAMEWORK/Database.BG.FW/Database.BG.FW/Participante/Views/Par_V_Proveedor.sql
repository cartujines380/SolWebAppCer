CREATE VIEW [Participante].[Par_V_Proveedor]
AS
	SELECT p.IdParticipante, pro.IdEmpresa, p.IdUsuario, p.IdTipoIdentificacion,
		p.Identificacion,TipoParticipante = 'Persona',		
		isnull(pe.Apellido1,'') + ' ' + isnull(pe.Apellido2,'') + ' '+ isnull(pe.Nombre1,'') + ' ' + isnull(pe.Nombre2,'')as Nombre, pro.Estado          
	FROM Participante.Par_Proveedor pro, Participante.Par_Persona pe, Participante.Par_Participante p
	WHERE pro.IdParticipante = pe.IdParticipante 
		AND pe.IdParticipante = p.IdParticipante
	UNION
	SELECT p.IdParticipante, pro.IdEmpresa, p.IdUsuario, p.IdTipoIdentificacion,
		p.Identificacion, TipoParticipante = 'Empresa',		
                	e.Nombre, pro.Estado 
	FROM Participante.Par_Proveedor pro, Participante.Par_Empresa e, Participante.Par_Participante p
	WHERE pro.IdParticipante = e.IdParticipante 
		AND e.IdParticipante = p.IdParticipante 









