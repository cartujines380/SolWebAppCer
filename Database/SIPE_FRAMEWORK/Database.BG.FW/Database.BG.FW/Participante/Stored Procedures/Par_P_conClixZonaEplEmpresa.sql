

CREATE  PROCEDURE [Participante].[Par_P_conClixZonaEplEmpresa]
	@PI_IdEmpresa int,
	@PI_IdEmpleado int,
	@PI_IdZona int
AS
	
SELECT  cli.IdParticipante IdCliente,Participante.Par_F_getNombreParticipante(cli.IdParticipante) NomCliente,	
		'' as IdEmpleado, '' as NomEmpleado,
		Catalogo.Ctl_F_conCatalogo(210,cli.IdCalificacion) Calificacion,
		d.Direccion,d.NombreContacto,
		Catalogo.Ctl_F_conCatalogo(1,o.IdZona)ZonaCli
	FROM Participante.Par_Cliente cli left outer join Participante.Par_Direccion d 
		 on cli.IdParticipante = d.IdParticipante and d.IdTipoDireccion = 6,
         Participante.Par_Oficina o
	WHERE cli.IdEmpresa = @PI_IdEmpresa
		and cli.IdVendedor = @PI_IdEmpleado
        and cli.IdEmpresa = o.IdEmpresa
        and cli.IdOficina = o.IdOficina
        and o.IdZona = @PI_IdZona
		






