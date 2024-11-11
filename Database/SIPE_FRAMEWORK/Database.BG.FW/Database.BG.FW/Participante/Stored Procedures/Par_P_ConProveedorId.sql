create  procedure [Participante].[Par_P_ConProveedorId] 
@PI_IdParticipante int,
@PI_IdEmpresa int
AS	
	select	
	ContribuyenteEspecial
	FROM Participante.Par_Proveedor 
	where   IdParticipante = @PI_IdParticipante
		AND IdEmpresa = @PI_IdEmpresa
	





