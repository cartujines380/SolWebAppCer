



create procedure [Seguridad].[Seg_P_PARAM_PROCEDURE]
@PV_IdTransaccion		int,
@PV_Parametros		varchar(2000) OUTPUT

AS
	SELECT @PV_Parametros = Parametros
	FROM Seguridad.Seg_TRANSACCION
	WHERE IdTransaccion = @PV_IdTransaccion






