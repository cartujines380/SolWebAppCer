CREATE PROCEDURE [Participante].[Par_P_ConCuentaParticipante]
@PI_IdParticipante int
AS

	SELECT  d.Cuenta, d.IdTipoCuenta,
		Catalogo.Ctl_F_conCatalogo(221,d.IdTipoCuenta) TipoCuenta,
		d.IdBanco, Participante.Par_F_getNombreParticipante(d.IdBanco) Banco,
		d.OficialCuenta, d.Telefono,
		d.DescCuentaBanco
	FROM Participante.Par_CuentaParticipante d
	WHERE d.IdParticipante = @PI_IdParticipante
		




