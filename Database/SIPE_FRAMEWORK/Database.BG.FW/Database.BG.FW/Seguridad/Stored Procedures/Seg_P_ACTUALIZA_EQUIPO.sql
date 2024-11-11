


create procedure [Seguridad].[Seg_P_ACTUALIZA_EQUIPO]
@PV_idEquipo          int,
  @PV_Nombre              VARCHAR(30 ),
  @PV_idEmpresa           int,
  @PV_descripcion         VARCHAR(100 ),
  @PV_identificacion1     VARCHAR(100 ),
  @PV_identificacion2     VARCHAR(100 ),
  @PV_area                VARCHAR(100 ),
  @PV_IdOficina           int,
  @PV_IdSucursal          int
AS 
     UPDATE Seguridad.Seg_EQUIPO
            SET idempresa=@PV_idEmpresa,
                nombre= @PV_Nombre,
                descripcion=@PV_descripcion,
                ididentificacion1=@PV_identificacion1,
                ididentificacion2=@PV_identificacion2,
                area=@PV_area,
                idoficina=@PV_idOficina, 
                idsucursal= @PV_IdSucursal
     WHERE idequipo=@PV_idEquipo
     --COMMIT
 







