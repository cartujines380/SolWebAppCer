


create procedure [Seguridad].[Seg_P_ELIMINA_HORAR_TRAN]
@PV_idTransaccion   int,
     @PV_idOrganizacion  int,
     @PV_idOpcion        int,
      @PV_idHorario      int,
      @PV_retorna         CHAR OUTPUT
AS
  DECLARE @In_count               int
   
     set @In_count=0
     SELECT @In_count = count(*)
	FROM Seguridad.Seg_AUTORIZACION
	WHERE  IdTransaccion=@PV_idTransaccion AND
	       IdOpcion=@PV_idOpcion AND
	       IdOrganizacion=@PV_idOrganizacion
     IF @In_count = 0 
      BEGIN
        DELETE FROM Seguridad.Seg_HORARIOTRANS
        WHERE idHorario=@PV_idHorario
        SET @PV_retorna='0'
        --COMMIT
      END      
     ELSE
         SET @PV_retorna=''
     
   





