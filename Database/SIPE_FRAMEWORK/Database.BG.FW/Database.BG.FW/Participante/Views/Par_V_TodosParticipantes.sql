CREATE View [Participante].[Par_V_TodosParticipantes]
as
	SELECT p.IdParticipante, p.IdUsuario,
		p.Identificacion,TipoParticipante = 'Persona',
		isnull(per.Apellido1,'') + ' ' + isnull(per.Apellido2,'') + ' '+ isnull(per.Nombre1,'') + ' ' + isnull(per.Nombre2,'') as Nombre,
		p.Estado

	FROM Participante.Par_Persona per,Participante.Par_Participante p				
	WHERE per.IdParticipante = p.IdParticipante
UNION
	SELECT p.IdParticipante, p.IdUsuario,
		p.Identificacion,TipoParticipante = 'Empleado',
		emp.Nombre,p.Estado
	FROM Participante.Par_Empresa emp,Participante.Par_Participante p
	WHERE emp.IdParticipante = p.IdParticipante
		and emp.IdCategoriaEmpresa > 0







