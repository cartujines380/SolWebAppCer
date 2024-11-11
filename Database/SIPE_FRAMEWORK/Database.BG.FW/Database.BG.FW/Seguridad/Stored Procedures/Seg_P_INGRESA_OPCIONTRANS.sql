


/*==============================================================*/
/* MANTENIMIENTO: OPCIONTRANS     */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_OPCIONTRANS]
@PV_idOpcion        int,
     @PV_idTransaccion     int,
     @PV_descripcion       VARCHAR( 100),
     @PV_nivel             int,
     --@PV_opcion            int,
     @PV_Organizacion      int
AS

 DECLARE  @In_count              int
 
    SET @In_count=0
     --@In_idOpcion=0

     SELECT @In_count = count(*)
     FROM Seguridad.Seg_OPCIONTRANS
     WHERE idtransaccion = @PV_idTransaccion
     AND idopcion = @PV_idopcion
     AND idOrganizacion=@PV_Organizacion

     IF @In_count = 0 
          INSERT INTO Seguridad.Seg_OPCIONTRANS(idtransaccion,idopcion,descripcion,nivel,idOrganizacion)
           VALUES(@PV_idTransaccion,@PV_idOpcion,@PV_descripcion,@PV_nivel,@PV_Organizacion)
     ELSE
        UPDATE Seguridad.Seg_OPCIONTRANS
               SET descripcion=@PV_descripcion,
                   nivel=@PV_nivel
        WHERE idtransaccion=@PV_idTransaccion
          AND idopcion=@PV_idopcion
          And idOrganizacion=@PV_Organizacion
     
     --COMMIT

  





