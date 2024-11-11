


/*==============================================================*/
/* MANTENIMIENTO: CATEGORIA       */
/*==============================================================*/


create procedure [Seguridad].[Seg_P_INGRESA_CATEGORIA]
@PV_descripcion           VARCHAR(100),
   @PV_idCatPadre            int
AS
   DECLARE @In_nivel               	int
   DECLARE  @ln_idCategoria      int
   DECLARE @lv_existe		int
 

     SELECT @In_nivel = nivel 
     FROM Seguridad.Seg_CATEGORIA
     WHERE idcategoria=@PV_idCatPadre

--     SELECT Seguridad.Seg_S_categoria.NEXTVAL INTO @ln_idCategoria FROM dual
       Select @ln_idCategoria = ISNULL(max(idcategoria),0) + 1
        From Seguridad.Seg_CATEGORIA 

        Select @lv_existe = count(IdCategoria)
        From Seguridad.Seg_CATEGORIA 
        Where upper(Descripcion) = upper(@PV_descripcion)

if ISNULL(@lv_existe,0) = 0
  Begin
     INSERT INTO Seguridad.Seg_CATEGORIA(idcategoria,descripcion,idcatpadre,nivel)
            VALUES(@ln_idCategoria,@PV_descripcion,@PV_idCatPadre,@In_nivel+1)
     --COMMIT
    select @ln_idCategoria
   end
else
	raiserror ('Descripción de  Categoria ya existe',16,1)





