




/*==============================================================*/
/* MANTENIMIENTO: OFICINA         */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_CONSULTA_OFICINA]
@PV_idSucursal           int,
  @PV_modo                 int
AS
     IF @PV_modo=0 
         SELECT ofi.idoficina,ofi.nombre
         FROM Seguridad.Seg_V_OFICINA ofi
         WHERE Ofi.idsucursal = @PV_idSucursal
     ELSE
         SELECT DISTINCT ofi.idOficina,ofi.nombre
         FROM Seguridad.Seg_V_OFICINA ofi
         WHERE ofi.idSucursal = @PV_idSucursal AND
         NOT EXISTS(SELECT * FROM Seguridad.Seg_HORARIOOFICINA hofi WHERE ofi.idOficina=hofi.idoficina)
 






