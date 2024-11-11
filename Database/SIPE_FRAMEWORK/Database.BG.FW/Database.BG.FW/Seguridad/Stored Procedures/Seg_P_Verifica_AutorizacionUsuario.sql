
create procedure [Seguridad].[Seg_P_Verifica_AutorizacionUsuario]
	@PV_idTransaccion     int,
        @PV_idOpcion          int,
        @PV_idOrganizacion    int,
        @PV_idHorario         int,
	@PV_idAutorizacion    int
AS
   DECLARE @In_Count        int
   
    set @In_count=0
     SELECT @In_count = count(*) 
            	FROM Seguridad.Seg_Autorizacion a join Seguridad.Seg_AutorizacionUsuario u on a.IdAutorizacion = u.IdAutorizacion
    	WHERE  a.IdTransaccion=@PV_idTransaccion AND	           
	       a.IdOrganizacion=@PV_idOrganizacion AND
	       a.IdOpcion=@PV_idOpcion AND
               a.IdHorario=@PV_idHorario AND
	       a.IdAutorizacion=@PV_idAutorizacion
     IF @In_count = 0  
      	select 1
      ELSE
       select 0
  






