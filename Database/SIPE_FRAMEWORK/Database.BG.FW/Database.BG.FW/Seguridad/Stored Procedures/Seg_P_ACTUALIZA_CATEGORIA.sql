



create procedure [Seguridad].[Seg_P_ACTUALIZA_CATEGORIA]
     @PV_idCategoria      int,
     @PV_descripcion      VARCHAR(100 ),
     @PV_idCatPadre       int
AS
     
   DECLARE @lv_Nivel               int
   DECLARE @lv_existe              int
   DECLARE @lv_existe2              int
  
    	Select @lv_existe = count(IdCategoria)
     	From Seguridad.Seg_CATEGORIA 
    	Where IdCatPadre = @PV_idCategoria
    	And IdCategoria  = @PV_idCatPadre  
	
	Select @lv_existe2 = count(IdCategoria)
     	From Seguridad.Seg_CATEGORIA 
    	Where upper(Descripcion) = upper(@PV_descripcion) and IdCategoria != @PV_IdCategoria
    		
	
   If ISNULL(@lv_existe,0) = 0 
	if ISNULL(@lv_existe2,0)=0 
	   BEGIN   
	    
	     	Select @lv_Nivel = Nivel + 1
			  From Seguridad.Seg_CATEGORIA
			  Where IdCategoria = @PV_idCatPadre
	    
	       UPDATE Seguridad.Seg_CATEGORIA
	              SET descripcion=@PV_descripcion,
	                  Nivel  = @lv_Nivel,
	                  idcatpadre=@PV_idCatPadre
	       WHERE idcategoria=@PV_idCategoria
	       
	       	UPDATE Seguridad.Seg_CATEGORIA 
			    Set Nivel = @lv_Nivel + 1
			    Where IdCatPadre = @PV_idCategoria
	     END
	else
	    raiserror ('Descripción de  Categoria ya existe',16,1)	    
   Else
        raiserror ('No puede asociar un padre que es su hijo',16,1)
  
       --COMMIT





