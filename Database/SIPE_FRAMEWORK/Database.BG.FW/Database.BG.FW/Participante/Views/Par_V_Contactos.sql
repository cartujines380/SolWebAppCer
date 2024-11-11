CREATE VIEW [Participante].[Par_V_Contactos]
AS
SELECT p.IdParticipante,		
		p.IdUsuario,
		p.Identificacion,
		con.IdPartContacto,
		con.IdTipoContacto,
		Catalogo.Ctl_F_conCatalogo(208,con.IdTipoContacto) as TipoContacto,
		pc.Identificacion as IdentificacionCont,
		pc.IdUsuario as IdUsuarioCont,
		pc.Nombre as NombreCont		
	FROM  Participante.Par_Persona pe, Participante.Par_Participante p,Participante.Par_Contacto con, Participante.Par_V_Persona pc
	WHERE pe.IdParticipante = p.IdParticipante 
		and pe.IdParticipante = con.IdParticipante
		and con.IdPartContacto = pc.IdParticipante

union 

SELECT p.IdParticipante,		
		p.IdUsuario,
		p.Identificacion,
		con.IdPartContacto,
		con.IdTipoContacto,
		Catalogo.Ctl_F_conCatalogo(208,con.IdTipoContacto) as TipoContacto,
		pc.Identificacion as IdentificacionCont,
		pc.IdUsuario as IdUsuarioCont,
		pc.Nombre as NombreCont		
	FROM  Participante.Par_Empresa e, Participante.Par_Participante p,Participante.Par_Contacto con, Participante.Par_V_Persona pc
	WHERE e.IdParticipante = p.IdParticipante 
		and e.IdParticipante = con.IdParticipante
		and con.IdPartContacto = pc.IdParticipante











