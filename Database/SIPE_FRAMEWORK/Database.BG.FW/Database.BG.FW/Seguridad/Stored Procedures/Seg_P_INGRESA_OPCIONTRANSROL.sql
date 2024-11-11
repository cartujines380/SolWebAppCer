



/*==============================================================*/
/* MANTENIMIENTO: OPCIONTRANSROL  */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_OPCIONTRANSROL]
@PV_idTransaccion     int,
        @PV_idOpcion          int,
        @PV_idOrganiza        int,
        @PV_idRol             int
AS
   DECLARE @lv_opcion              int
	set  @lv_opcion = 0
  
   
     Select @lv_opcion = count(*)  
     From Seguridad.Seg_OPCIONTRANSROL
     Where IDROL = @PV_idRol
     and IDORGANIZACION = @PV_idOrganiza
     and idtransaccion = @PV_idTransaccion
     and idopcion = @PV_idOpcion
     
     If ISNULL(@lv_opcion,0) = 0 
         INSERT INTO Seguridad.Seg_OPCIONTRANSROL(idtransaccion,idopcion,idrol, IDORGANIZACION)
                VALUES(@PV_idTransaccion,@PV_idOpcion,@PV_idRol, @PV_idOrganiza)
    --else
	--raiserror ('Transaccion ya asociada al rol',16,1)
     --COMMIT
  





