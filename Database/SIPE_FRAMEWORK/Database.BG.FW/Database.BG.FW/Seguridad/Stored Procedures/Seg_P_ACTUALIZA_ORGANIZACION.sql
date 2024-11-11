



create procedure [Seguridad].[Seg_P_ACTUALIZA_ORGANIZACION]
@PV_idOrganizacion       int,
        @PV_idCategoria          int,
        @PV_descripcion          VARCHAR(100),
        @PV_idOrgPadre           int,
        @PV_idAplicacion         int,
      @PV_CodRefApli           VARCHAR(8)
AS 
   DECLARE @lv_Nivel             int
   DECLARE @lv_existe            int
   DECLARE @lv_existe2		 int

   SET @lv_Nivel = 0
	set @lv_existe = 0

   	Select @lv_existe = count(idOrganizacion)
      	From Seguridad.Seg_ORGANIZACION 
    	Where idorgpadre = @PV_idOrganizacion
    	And idOrganizacion  = @PV_idOrgPadre
	
	Select @lv_existe2 = count(IdOrganizacion)
     	From Seguridad.Seg_ORGANIZACION 
    	Where upper(Descripcion) = upper(@PV_descripcion) and IdOrganizacion != @PV_idOrganizacion
 

   If ISNULL(@lv_existe,0) = 0 
	if ISNULL(@lv_existe2,0)=0 
	    BEGIN   
	    
	     	Select @lv_Nivel = Nivel + 1
			  From Seguridad.Seg_ORGANIZACION
			  Where idOrganizacion = @PV_idOrgPadre
	      
	     UPDATE Seguridad.Seg_ORGANIZACION
	            SET descripcion=@PV_descripcion,
	                idCategoria=@PV_idCategoria,
	                idorgpadre=@PV_idOrgPadre,
	                idAplicacion=@PV_idAplicacion,
					CodRefAplicativo=@PV_CodRefApli
	     WHERE idOrganizacion=@PV_idOrganizacion
	    
	   	UPDATE Seguridad.Seg_ORGANIZACION 
			    Set Nivel = @lv_Nivel
			    Where idOrganizacion = @PV_idOrganizacion
	     END   
	Else
        	raiserror ('Error: Descripcion de Producto ya existe',16,1)

   Else
        raiserror ('Error: No puede asociar un padre que es su hijo',16,1)
         
     --COMMIT
  





