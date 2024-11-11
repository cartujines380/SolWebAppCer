


/*==============================================================*/
/* Usuario reemplaza transaccion                      */
/*==============================================================*/

  create procedure [Seguridad].[Seg_P_CONSULTA_ORGPORUSER] 
@PV_Codusuario      VARCHAR(20 )
AS
         SELECT distinct o.idorganizacion, o.descripcion
         FROM Seguridad.Seg_Organizacion o, Seguridad.Seg_TRANSUSUARIO tru
         WHERE tru.idusuario = @PV_Codusuario
               and o.idorganizacion = tru.idorganizacion
        
    





