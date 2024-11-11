CREATE PROCEDURE [Participante].[Par_P_ConParametroParticipante]
@PI_IdParticipante int
AS
	SELECT 	d.IdParametro,
		d.IdTipoParametro,
		Catalogo.Ctl_F_conCatalogo(211,d.IdTipoParametro) Parametro,
		d.Valor
	FROM Participante.Par_ParametroParticipante d
	WHERE d.IdParticipante = @PI_IdParticipante





