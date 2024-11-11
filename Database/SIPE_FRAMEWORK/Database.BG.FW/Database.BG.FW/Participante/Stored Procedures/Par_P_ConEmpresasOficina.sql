CREATE Procedure [Participante].[Par_P_ConEmpresasOficina]
@PI_IdOficina int
AS
	SELECT o.IdEmpresa, Participante.Par_F_getNombreParticipante(o.IdEmpresa) as NombreEmpresa,
		o.IdZona, Catalogo.Ctl_F_conCatalogo(1,o.IdZona) as Zona,		
		isnull(o.OficinaSRI,0) as OficinaSRI
	FROM Participante.Par_Oficina o 
	WHERE o.IdOficina = @PI_IdOficina
		





