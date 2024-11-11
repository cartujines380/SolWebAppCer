

CREATE  PROCEDURE [Participante].[Par_P_conClixEplEmpresa]
	@PI_IdEmpresa int,
	@PI_IdEmpleado int
AS

	
SELECT  cli.IdParticipante IdCliente,Participante.Par_F_getNombreParticipante(cli.IdParticipante) NomCliente,
		'' as IdEmpleado, '' as NomEmpleado,
		Catalogo.Ctl_F_conCatalogo(210,cli.IdCalificacion) Calificacion,
		d.Direccion,d.NombreContacto,
		Catalogo.Ctl_F_conCatalogo(1,em.IdZona)ZonaCli
	FROM Participante.Par_Cliente cli left outer join Participante.Par_Direccion d
		on cli.IdParticipante = d.IdParticipante
		and d.IdTipoDireccion = 4,
		Participante.Par_Empresa em
	WHERE cli.IdEmpresa = @PI_IdEmpresa
		and cli.IdVendedor = @PI_IdEmpleado		
		and cli.IdOficina = em.IdParticipante







