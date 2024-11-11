




/*==============================================================*/
/* MANTENIMIENTO: SUCURSAL         */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_CONSULTA_SUCURSAL]
@PV_idEmpresa            int,
   @PV_modo                 int
AS

     IF @PV_modo=0  --retorna todas las sucursales que pertenece a @PV_idEmpresa
       
        SELECT idsucursal,nombre
        FROM Seguridad.Seg_V_SUCURSAL
        WHERE idempresa=@PV_idEmpresa
     ELSE
         --retorna todas las sucursales que no tengan idoficina en la tabla HORARIOOFICINA
         SELECT DISTINCT su.idSucursal,su.nombre
         FROM Seguridad.Seg_V_SUCURSAL su, Seguridad.Seg_V_OFICINA ofi
         WHERE su.idEmpresa=@PV_idEmpresa AND
               su.idSucursal=ofi.idSucursal AND
         NOT EXISTS(SELECT * FROM Seguridad.Seg_HORARIOOFICINA hofi WHERE ofi.idOficina=hofi.idOficina)
     
 





