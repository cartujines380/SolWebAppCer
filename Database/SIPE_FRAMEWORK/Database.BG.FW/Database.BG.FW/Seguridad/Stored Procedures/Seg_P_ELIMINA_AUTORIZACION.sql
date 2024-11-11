



create procedure [Seguridad].[Seg_P_ELIMINA_AUTORIZACION]
@PV_idAutorizacion      int,
      @PV_retorna         CHAR OUTPUT
AS 
   DECLARE @In_count               int
   
     SET @In_count=0
     SELECT @In_count = count(*) 
	FROM Seguridad.Seg_AUTORIZACIONUSUARIO
	WHERE  IDAUTORIZACION=@PV_idAutorizacion
     IF @In_count = 0 
     BEGIN
        DELETE FROM Seguridad.Seg_AUTORIZACION
        WHERE idautorizacion=@PV_idAutorizacion
        set @PV_retorna='0'
        --COMMIT
      END
      ELSE
         set @PV_retorna='1'
   






