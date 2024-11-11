

create  procedure [Participante].[Par_P_ConEmpleadoId] 
@PI_IdParticipante int,
@PI_IdEmpresa int

AS
	select 	e.IdTipoEmpleado, e.IdEmpresaPertenece, 
		p.IdUsuario as IdUsuEmpPertenece, 
		em.Nombre as NombreEmpPertenece,
		e.IdOrganigrama, o.Descripcion as DescOrganigrama,
		e.IdCargo, e.Sueldo,e.IdMoneda, Catalogo.Ctl_F_conCatalogo(16,e.IdMoneda) as Moneda, --e.NumeroCarga,
		e.HorasExtras,
		e.LibretaSeguro,
		convert(varchar,e.FechaIngSeguro,110) as FechaIngSeguro,
		convert(varchar,e.FechaNotEgreso,110) as FechaNotEgreso
	FROM Participante.Par_Empleado e, Participante.Par_Empresa em, Participante.Par_Participante p, Participante.Par_Organigrama o
	where   e.IdParticipante = @PI_IdParticipante
		And e.IdEmpresa = @PI_IdEmpresa
		and e.IdEmpresaPertenece = em.IdParticipante
		and em.IdParticipante = p.IdParticipante
		and e.IdOrganigrama = o.IdOrganigrama

		







