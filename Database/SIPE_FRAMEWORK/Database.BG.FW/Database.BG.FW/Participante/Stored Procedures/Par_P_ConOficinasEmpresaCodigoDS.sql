
CREATE Procedure [Participante].[Par_P_ConOficinasEmpresaCodigoDS]
@PI_IdEmpresa int
AS
	SELECT o.IdOficina, Participante.Par_F_getNombreParticipante(o.IdOficina) as Oficina,
		o.IdZona, Catalogo.Ctl_F_conCatalogo(1,o.IdZona) as Zona,
		isnull(o.OficinaSRI,0) as OficinaSRI
	FROM Participante.Par_Oficina o 
	WHERE o.IdEmpresa = @PI_IdEmpresa
		





