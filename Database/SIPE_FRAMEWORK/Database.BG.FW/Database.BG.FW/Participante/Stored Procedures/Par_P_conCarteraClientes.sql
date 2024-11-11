CREATE Procedure [Participante].[Par_P_conCarteraClientes]
	@PI_IdEmpresa int,
	@PI_IdEmpleado int,
	@PI_IdZona int,
	@PI_IdOpcion tinyint
AS
if @PI_IdOpcion = 1 
	exec Participante.Par_P_conClixZonaEmpresa @PI_IdEmpresa,@PI_IdEmpleado, @PI_IdZona
if @PI_IdOpcion = 2 
	exec Participante.Par_P_conClixEplEmpresa @PI_IdEmpresa, @PI_IdEmpleado
if @PI_IdOpcion = 3 
	exec Participante.Par_P_conClixZonaEplEmpresa @PI_IdEmpresa, @PI_IdEmpleado, @PI_IdZona
return





