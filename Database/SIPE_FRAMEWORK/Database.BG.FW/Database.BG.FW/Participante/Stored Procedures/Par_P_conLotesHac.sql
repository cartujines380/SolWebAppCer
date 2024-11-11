
CREATE PROCEDURE [Participante].[Par_P_conLotesHac]
@PI_IdParticipante int
as

	SELECT  IdLote, Tamaño, IdTipoCultivo,
		Catalogo.Ctl_F_conCatalogo(219,IdTipoCultivo) as DescCultivo,
		Copropietario
	FROM    Participante.Par_LoteHac
	WHERE   IdEmpresa = @PI_IdParticipante






