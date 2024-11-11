


CREATE  procedure [Participante].[Par_P_ConClienteGeneral] 
@PI_IdParticipante int
AS
	
select 	c.IdEmpresa, 
		ee.Nombre as NombreEmpresa,
		c.IdOficina, eo.Nombre as Oficina,
		c.IdOficinaGestion, 
		Participante.Par_F_getNombreParticipante(c.IdOficinaGestion) as OficinaGestion,
		c.PorcentajeDescuento, c.IdVendedor, 
		Participante.Par_F_getUsuarioParticipante(c.IdVendedor) as IdUsuVendedor,
		Participante.Par_F_getNombreParticipante(c.IdVendedor) as NombreVendedor,		
		c.ContribuyenteEspecial,
		c.IdCalificacion,Catalogo.Ctl_F_conCatalogo(210,c.IdCalificacion) as Calificacion,
		c.Iva,c.Estado,
		Catalogo.Ctl_F_conCatalogo(15,c.Estado) as DescEstado		
	FROM Participante.Par_Cliente c,	Participante.Par_Empresa ee, Participante.Par_Empresa eo 
	WHERE   c.IdParticipante = @PI_IdParticipante
		and c.IdEmpresa = ee.IdParticipante
		and c.IdOficina = eo.IdParticipante
	





