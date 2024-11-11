


create procedure [Seguridad].[Seg_P_ELIMINA_ORGANIZACION]
@PV_idOrganizacion      int
AS 
   DECLARE @lv_existe              int
   
   
     Select @lv_existe = COUNT(idorganizacion)
			From Seguridad.Seg_ORGANIZACION
			where IDORGPADRE = @PV_idOrganizacion 
      
      If ISNULL(@lv_existe,0) = 0 
         DELETE FROM Seguridad.Seg_ORGANIZACION
         WHERE idorganizacion=@PV_idOrganizacion
      else
         raiserror ('Error: No puede eliminar una organización si tiene hijos',16,1)

     
     --COMMIT
  






