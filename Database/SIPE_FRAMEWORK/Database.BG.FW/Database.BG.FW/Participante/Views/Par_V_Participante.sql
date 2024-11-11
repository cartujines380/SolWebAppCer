create view [Participante].[Par_V_Participante]
as
	SELECT  c.IdParticipante, c.IdEmpresa, c.IdUsuario, 
		c.Identificacion,c.TipoParticipante,
		c.Nombre, c.Estado,
		'TipoPart'=1 
	FROM Participante.Par_V_Cliente c
UNION 
	SELECT e.IdParticipante, e.IdEmpresa, e.IdUsuario,
		e.Identificacion,'TipoParticipante'= 'Persona',
		e.Nombre, e.Estado,
		'TipoPart'=2 
    	FROM Participante.Par_V_Empleado e
UNION 
	SELECT pro.IdParticipante, pro.IdEmpresa, pro.IdUsuario, 
		pro.Identificacion,pro.TipoParticipante,
		pro.Nombre, pro.Estado,
		'TipoPart'=3
	FROM Participante.Par_V_Proveedor pro
UNION
	SELECT p.IdParticipante, 'IdEmpresa'='', p.IdUsuario, 
		p.Identificacion,p.TipoParticipante,
		p.Nombre, p.Estado,
		'TipoPart'=''
	FROM Participante.Par_V_ParticipanteNotCliEplPro p







