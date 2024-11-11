create procedure [Participante].[Par_P_ConsGenSolicitud]
AS
SELECT par.IdParticipante, 
		Catalogo.Ctl_F_ConCatalogo(7, par.IdTipoIdentificacion) as TipoIdent,
	par.Identificacion, 
	Participante.Par_F_getNombreParticipante(par.IdParticipante) as NombreCliente,
	convert(varchar(10),par.FechaRegistro,110) Fecha,
	Participante.Par_F_getTipoPart(par.IdParticipante) as TipoPart
FROM Participante.Par_Participante par,  
	Participante.Par_Persona per 
WHERE par.IdParticipante = per.IdParticipante
	--and par.Estado = 'I'
	AND not exists (select 1 from Participante.Par_RegistroCliente cli 
						where cli.IdParticipante = par.IdParticipante)
UNION
SELECT  par.IdParticipante, 
	Catalogo.Ctl_F_ConCatalogo(7, par.IdTipoIdentificacion) as TipoIdent,
	par.Identificacion, 
	Participante.Par_F_getNombreParticipante(par.IdParticipante) as NombreCliente,
	convert(varchar(10),par.FechaRegistro,110) Fecha,
	Participante.Par_F_getTipoPart(par.IdParticipante) as TipoPart
FROM Participante.Par_Participante par, Participante.Par_Empresa emp
WHERE par.IdParticipante = emp.IdParticipante
	AND not exists (select 1 from Participante.Par_RegistroCliente cli 
						where cli.IdParticipante = par.IdParticipante)


