



create procedure [Seguridad].[Seg_P_ELIMINA_HORARIOTRANS]
@PV_idTransaccion     int,
        @PV_idOpcion          int,
        @PV_idOrganizacion    int,
        @PV_idHorario         int
AS 
   DECLARE @lv_count               int
   
   select @lv_count = count(*)
 from Seguridad.Seg_AUTORIZACION
   where IdTransaccion=@PV_idTransaccion and IdOpcion=@PV_idOpcion and
         IdOrganizacion=@PV_idOrganizacion and IdHorario=@PV_idHorario
     IF @lv_count=0  
	BEGIN
	     DELETE FROM Seguridad.Seg_HORARIOTRANS
	     WHERE idtransaccion=@PV_idTransaccion AND
	           idopcion=@PV_idOpcion AND
	           idOrganizacion=@PV_idOrganizacion AND
	           idhorario=@PV_idHorario
	  --   --COMMIT
     END 
     ELSE
       raiserror ('Organizacion asociada a una Autorizacion',16,1)
   





