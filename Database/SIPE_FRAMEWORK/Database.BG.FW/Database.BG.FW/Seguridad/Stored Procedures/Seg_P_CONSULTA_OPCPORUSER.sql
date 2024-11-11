



create procedure [Seguridad].[Seg_P_CONSULTA_OPCPORUSER] 
@PV_Codusuario      VARCHAR(20 ),
        @PV_org             int,
        @PV_transa          int
AS
         SELECT distinct o.idopcion, o.descripcion
         FROM Seguridad.Seg_Opciontrans o, Seguridad.Seg_TRANSUSUARIO tru
         WHERE tru.idusuario = @PV_Codusuario
               and tru.idorganizacion = @PV_org
               and tru.idtransaccion = @PV_transa
               and o.idorganizacion = tru.idorganizacion
               and o.idtransaccion  = tru.idtransaccion
               and o.idopcion = tru.idopcion
        
   





