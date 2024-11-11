




/*==============================================================*/
/* MANTENIMIENTO: HORARIOTRANS    */
/*==============================================================*/


 create procedure [Seguridad].[Seg_P_INGRESA_HORARIOTRANS]
@PV_idTransaccion   int,
      @PV_idOpcion          int,
      @PV_idOrganizacion    int,
      @PV_idHorario         int
AS
   DECLARE @In_count               int
   
   set @In_count=0
   SELECT @In_count =count(*)
	 FROM Seguridad.Seg_HORARIOTRANS
	 WHERE  idTransaccion=@PV_idTransaccion and
	        idOpcion=@PV_idOpcion and
	        idOrganizacion=@PV_idOrganizacion and
	        idHorario=@PV_idHorario
   IF @In_count = 0 
    BEGIN
     INSERT INTO Seguridad.Seg_HORARIOTRANS(idtransaccion,idopcion,idOrganizacion,idhorario)
            VALUES(@PV_idTransaccion,@PV_idOpcion,@PV_idOrganizacion,@PV_idHorario)
    END 
-- --COMMIT
   
 
  





