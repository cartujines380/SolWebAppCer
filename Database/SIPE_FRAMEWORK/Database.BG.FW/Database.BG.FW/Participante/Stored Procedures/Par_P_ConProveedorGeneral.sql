

create  procedure [Participante].[Par_P_ConProveedorGeneral]
@PI_IdParticipante int

AS
	-- Prueba desde VS2003
	select 	p.IdEmpresa, 
		ee.Nombre as NombreEmpresa,
		p.IdOficina, eo.Nombre as Oficina,
		p.ContribuyenteEspecial,p.Estado,
		Catalogo.Ctl_F_conCatalogo(15,p.Estado) as DescEstado
	FROM Participante.Par_Proveedor p, Participante.Par_Empresa ee, Participante.Par_Empresa eo		
	where   p.IdParticipante = @PI_IdParticipante
		and p.IdEmpresa = ee.IdParticipante
		and p.IdOficina = eo.IdParticipante
		
	






