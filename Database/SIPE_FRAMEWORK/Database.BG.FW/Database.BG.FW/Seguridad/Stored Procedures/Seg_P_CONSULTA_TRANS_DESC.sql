


create procedure [Seguridad].[Seg_P_CONSULTA_TRANS_DESC]
@PV_desc             VARCHAR(100)
AS
       SELECT idTransaccion,descripcion
       FROM Seguridad.Seg_TRANSACCION
       WHERE descripcion like @PV_desc + '%'
   





