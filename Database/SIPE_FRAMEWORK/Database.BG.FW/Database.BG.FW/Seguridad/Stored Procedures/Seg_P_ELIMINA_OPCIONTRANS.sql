



create procedure [Seguridad].[Seg_P_ELIMINA_OPCIONTRANS]
@PV_idTransaccion     int,
     @PV_idOpcion          int,
     @PV_Organizacion      int
AS

  DECLARE @In_Count               int

    SET @In_count=0
     SELECT @In_count = count(*)
            	FROM Seguridad.Seg_HORARIOTRANS
    	WHERE  IdTransaccion=@PV_idTransaccion AND
	       IdOpcion=@PV_idOpcion AND
	       IdOrganizacion=@PV_Organizacion
     IF @In_count = 0 
	BEGIN 
	     DELETE FROM Seguridad.Seg_OPCIONTRANS
	     WHERE idtransaccion=@PV_idTransaccion 
	           AND idopcion=@PV_idOpcion
	           AND idOrganizacion=@PV_Organizacion
	     --COMMIT
	END
     ELSE
      raiserror ('Horarios asociados',16,1)
  





