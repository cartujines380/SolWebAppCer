
CREATE procedure [Seguridad].[Seg_P_CONS_TRANS_OPC_xORG] 
@PV_idOrg        int
AS
SELECT distinct t.IdTransaccion, t.Descripcion as Transaccion, ot.IdOpcion, ot.Descripcion as Opcion, cast(0 as bit) as Band
         FROM Seguridad.Seg_Transaccion t left outer join
              Seguridad.Seg_OpcionTrans ot
              ON t.IdOrganizacion = ot.IdOrganizacion
              AND t.IdTransaccion = ot.IdTransaccion
         WHERE t.IdOrganizacion = @PV_idOrg
			AND t.Estado  = 'A'

