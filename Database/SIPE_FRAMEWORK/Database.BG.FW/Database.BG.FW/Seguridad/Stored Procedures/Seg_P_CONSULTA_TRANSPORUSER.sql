



create procedure [Seguridad].[Seg_P_CONSULTA_TRANSPORUSER] 
@PV_Codusuario      VARCHAR(20 ),
          @PV_org             int
AS
         SELECT distinct  t.idtransaccion, t.descripcion
         FROM Seguridad.Seg_TRANSACCION t, Seguridad.Seg_TRANSUSUARIO tru
         WHERE tru.idusuario = @PV_Codusuario
               AND tru.idorganizacion = @PV_org
               and t.idorganizacion = tru.idorganizacion
               and t.idtransaccion = tru.idtransaccion
        
  





