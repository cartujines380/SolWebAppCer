CREATE view [Participante].[Par_V_ParticipanteNotCliEplPro]
as
	SELECT p.IdParticipante, p.IdUsuario,
		p.Identificacion,TipoParticipante = 'Persona',
		isnull(per.Apellido1,'') + ' ' + isnull(per.Apellido2,'') + ' '+ isnull(per.Nombre1,'') + ' ' + isnull(per.Nombre2,'') as Nombre,
		p.Estado
	FROM Participante.Par_Persona per,Participante.Par_Participante p				
	WHERE per.IdParticipante = p.IdParticipante
		and per.IdParticipante not in(Select IdParticipante from Participante.Par_Cliente)
		and per.IdParticipante not in(Select IdParticipante from Participante.Par_Proveedor)
		and per.IdParticipante not in(Select IdParticipante from Participante.Par_Empleado)
UNION
	SELECT p.IdParticipante, p.IdUsuario,
		p.Identificacion,TipoParticipante = 'Empresa',
		emp.Nombre, p.Estado
	FROM Participante.Par_Empresa emp,Participante.Par_Participante p
	WHERE emp.IdParticipante = p.IdParticipante
		and emp.IdCategoriaEmpresa > 0
		and emp.IdParticipante not in(Select IdParticipante from Participante.Par_Cliente)
		and emp.IdParticipante not in(Select IdParticipante from Participante.Par_Proveedor)






