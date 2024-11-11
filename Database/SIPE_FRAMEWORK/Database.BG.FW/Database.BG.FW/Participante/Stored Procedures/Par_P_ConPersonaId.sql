
create  procedure [Participante].[Par_P_ConPersonaId]
@PI_IdParticipante int
AS
	select 	p.Apellido1, p.Apellido2, p.Nombre1, p.Nombre2,
		p.IdTitulo, Catalogo.Ctl_F_conCatalogo(220,p.IdTitulo) as Titulo,
		p.Sexo,
		convert(varchar,p.FechaNacimiento,110) as FechaNacimiento,
		p.EstadoCivil,Catalogo.Ctl_F_conCatalogo(6,p.EstadoCivil) as EstCivil,
		p.Ruc,p.IdEmpRel,
		Participante.Par_F_getUsuarioParticipante(p.IdEmpRel) as IdUsuEmpRel,
		Participante.Par_F_getNombreParticipante(p.IdEmpRel) as NombreEmpRel
	FROM Participante.Par_Persona p
	where   p.IdParticipante = @PI_IdParticipante
				






