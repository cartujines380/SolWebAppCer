


create procedure [Seguridad].[Seg_P_CONSULTA_TRANSACCION]
@PV_idTransaccion           int
AS
      SELECT idTransaccion,descripcion
      FROM Seguridad.Seg_TRANSACCION
      WHERE idTransaccion >= @PV_idTransaccion
 






