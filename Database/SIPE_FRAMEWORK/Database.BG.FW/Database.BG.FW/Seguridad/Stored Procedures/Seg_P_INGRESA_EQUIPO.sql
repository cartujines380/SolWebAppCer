


/*==============================================================*/
/* MANTENIMIENTO: EQUIPO  ---VMC                       */
/*==============================================================*/

create procedure [Seguridad].[Seg_P_INGRESA_EQUIPO]
 @PV_nombre            VARCHAR(30),
  @PV_idEmpresa             int,
  @PV_descripcion           VARCHAR(100 ),
  @PV_identificacion1       VARCHAR(100 ),
  @PV_identificacion2       VARCHAR(100 ),
  @PV_area                  VARCHAR(100 ),
  @PV_idOficina              int,
  @PV_idSucursal             int

AS 

   DECLARE @ln_idEquipo            int

     --SELECT Seguridad.Seg_S_equipo.NEXTVAL INTO @ln_idEquipo FROM dual
     Select @ln_idEquipo = ISNULL(max(idequipo),0) + 1
        From Seguridad.Seg_EQUIPO 
        
     INSERT INTO Seguridad.Seg_EQUIPO(idequipo, nombre,idempresa,descripcion,
                        ididentificacion1,ididentificacion2,area,idoficina,
                       idSucursal)
            VALUES(@ln_idEquipo, @PV_nombre, @PV_idEmpresa, @PV_descripcion,
                   @PV_identificacion1,@PV_identificacion2,@PV_area,@PV_idOficina,
                   @PV_idSucursal)
     --COMMIT
    select @ln_idEquipo
 






