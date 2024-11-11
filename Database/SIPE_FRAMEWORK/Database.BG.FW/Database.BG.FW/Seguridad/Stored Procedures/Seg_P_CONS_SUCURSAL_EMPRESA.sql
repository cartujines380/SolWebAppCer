


/*==============================================================*/
/* PERMISOS                       */
/*==============================================================*/


 
/*==============================================================*/
/* CONSULTAS VARIAS               */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_CONS_SUCURSAL_EMPRESA]
@PV_idOficina      int,
       @PV_idSucursal int OUTPUT,
       @PV_idEmpresa  int OUTPUT
AS
      SELECT 
     @PV_idSucursal=idsucursal,
     @PV_idEmpresa=idempresa

      FROM Seguridad.Seg_V_OFICINA ofi, Seguridad.Seg_V_SUCURSAL su
      WHERE ofi.idoficina=@Pv_idOficina AND
            ofi.idsucursal=su.idsucursal

  





