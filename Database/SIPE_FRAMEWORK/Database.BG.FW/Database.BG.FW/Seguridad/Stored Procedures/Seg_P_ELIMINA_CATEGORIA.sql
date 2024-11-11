


/* Se calbio el menaje por error  VMC en Migracion */
 create procedure [Seguridad].[Seg_P_ELIMINA_CATEGORIA]
@PV_idCategoria           int

AS 
   DECLARE @lv_existe              int
   DECLARE @lv_cuenta              int
	SET @lv_existe = 0
	set @lv_cuenta = 0
  
     Select @lv_existe = COUNT(IdCatPadre) 
		 From Seguridad.Seg_CATEGORIA
		 where IdCatPadre = @PV_idCategoria 
     
     select @lv_cuenta = count(*)
       from Seguridad.Seg_organizacion
      where idCategoria=@PV_idCategoria
      
      If ISNULL(@lv_existe,0) = 0 and ISNULL(@lv_cuenta,0)=0
         DELETE FROM Seguridad.Seg_CATEGORIA
         WHERE idcategoria=@PV_idCategoria
      Else
	BEGIN
         if ISNULL(@lv_existe,0) <> 0 
         	raiserror ('Esta categoria es Padre no se puede eliminar',16,1)
         else
           raiserror ('Esta categoria se encuentra en una organizacion',16,1)
         
      END
            
    --COMMIT
   






