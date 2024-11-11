



create procedure [Seguridad].[Seg_P_CONSULTA_OPCIONTRN]
@PV_idTransaccion    int,
      @PV_Organizacion     int
AS
   select distinct opc.idopcion,opc.descripcion,opc.nivel
      from Seguridad.Seg_opciontrans opc 
        where opc.idtransaccion=@PV_idTransaccion
        AND opc.idorganizacion = @PV_Organizacion
   





