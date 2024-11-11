CREATE VIEW [Participante].[Par_V_HacCliente]
AS
	SELECT  p.IdParticipante,
		p.IdUsuario,
		p.Identificacion,
		Participante.Par_F_getNombreParticipante(p.IdParticipante) as NombreCliente,
		cli.IdEmpresa,		
		emp.IdParticipante as IdEmpresaCli,
		emp.Nombre as NombreEmp,
		Participante.Par_F_TamañoHac(lot.IdEmpresa) as Tamaño
	FROM  Participante.Par_Cliente cli, Participante.Par_Participante p,Participante.Par_Contacto con, Participante.Par_Empresa emp, Participante.Par_LoteHac lot
	WHERE cli.IdParticipante = p.IdParticipante 
		and p.TipoParticipante='P'
		and cli.IdParticipante = con.IdPartContacto
		and con.IdParticipante = emp.IdParticipante
		and emp.IdParticipante = lot.IdEmpresa
		and con.IdTipoContacto = 9
union
	SELECT  p.IdParticipante,
		p.IdUsuario,
		p.Identificacion,
		Participante.Par_F_getNombreParticipante(p.IdParticipante) as NombreCliente,
		cli.IdEmpresa,		
		emp.IdParticipante as IdEmpresaCli,
		emp.Nombre as NombreEmp,
		Participante.Par_F_TamañoHac(lot.IdEmpresa) as Tamaño
	FROM  Participante.Par_Cliente cli, Participante.Par_Participante p,Participante.Par_Empresa emp, Participante.Par_LoteHac lot
	WHERE cli.IdParticipante = p.IdParticipante 
		and p.TipoParticipante='E'
		and cli.IdParticipante = emp.IdEmpresaPadre
		and emp.IdParticipante = lot.IdEmpresa		








