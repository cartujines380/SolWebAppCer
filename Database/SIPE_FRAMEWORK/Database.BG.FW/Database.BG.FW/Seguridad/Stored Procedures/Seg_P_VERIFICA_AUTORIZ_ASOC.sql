




create procedure [Seguridad].[Seg_P_VERIFICA_AUTORIZ_ASOC]
	@PV_idTransaccion     int,
         @PV_idOpcion          int,
         @PV_idOrganizacion    int,
         @PV_idHorario         int
AS
   DECLARE @In_Count        int
   
    set @In_count=0
     SELECT @In_count = count(*) 
            	FROM Seguridad.Seg_AUTORIZACION
    	WHERE  IdTransaccion=@PV_idTransaccion AND
	           IdOpcion=@PV_idOpcion AND
	           IdOrganizacion=@PV_idOrganizacion AND
             IdHorario=@PV_idHorario
     IF @In_count = 0  
      	select 1
      ELSE
       select 0
  





