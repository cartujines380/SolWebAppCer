


--Participante.Par_P_ConEmpleadoGeneral 62,2,
CREATE  procedure [Participante].[Par_P_ConEmpleadoGeneral]
@PI_IdParticipante int
AS
	
select 	e.IdEmpresa, 
		ee.Nombre as NombreEmpresa,
		e.IdOficina, eo.Nombre as Oficina,
		e.IdTipoEmpleado, e.IdEmpresaPertenece,
		Participante.Par_F_getUsuarioParticipante(e.IdEmpresaPertenece) as IdUsuEmpresaPertenece,
		Participante.Par_F_getNombreParticipante(e.IdEmpresaPertenece) as NombreEmpPertenece,
		e.IdOrganigrama, o.Descripcion as DescOrganigrama,
		e.IdCargo, Catalogo.Ctl_F_conCatalogo(207,e.IdCargo) as Cargo,
		e.Sueldo,
		e.IdMoneda, Catalogo.Ctl_F_conCatalogo(16,e.IdMoneda) as Moneda,-- e.NumeroCarga,
		e.HorasExtras,
		e.LibretaSeguro,
		convert(varchar,e.FechaIngSeguro,110) as FechaIngSeguro,
		convert(varchar,e.FechaNotEgreso,110) as FechaNotEgreso,
		e.Estado,
		Catalogo.Ctl_F_conCatalogo(216,e.Estado) as DescEstado
	FROM Participante.Par_Empleado e, Participante.Par_Empresa ee, Participante.Par_Empresa eo, 
		Participante.Par_Organigrama o		
	where   e.IdParticipante = @PI_IdParticipante
		and e.IdEmpresa = ee.IdParticipante
		and e.IdOficina = eo.IdParticipante
		and (e.IdOrganigrama = o.IdOrganigrama and e.IdEmpresa = o.IdEmpresa)
		





