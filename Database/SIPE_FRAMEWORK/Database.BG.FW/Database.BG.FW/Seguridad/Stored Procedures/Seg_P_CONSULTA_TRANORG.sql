
create procedure [Seguridad].[Seg_P_CONSULTA_TRANORG] 
@PV_idOrg        int
AS
         SELECT distinct t.IdTransaccion, t.Descripcion
         FROM Seguridad.Seg_Organizacion o, Seguridad.Seg_Transaccion t
         WHERE t.idorganizacion = @PV_idOrg 

  






