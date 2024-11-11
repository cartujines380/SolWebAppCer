
create procedure [Seguridad].[Seg_P_VERIFICA_USUARIO_ASOC]
@PV_idTransaccion     int,
         @PV_idOpcion          int,
         @PV_Organizacion      int
AS
  DECLARE @In_Count               int
   
    SET @In_count=0
     SELECT @In_count = count(*) 
            	FROM Seguridad.Seg_TRANSUSUARIO
    	WHERE  IdTransaccion=@PV_idTransaccion AND
	       IdOpcion=@PV_idOpcion AND
	       IdOrganizacion=@PV_Organizacion
     IF @In_count = 0  
      	select 1 as Estado
      ELSE
       select 0 as Estado
 





