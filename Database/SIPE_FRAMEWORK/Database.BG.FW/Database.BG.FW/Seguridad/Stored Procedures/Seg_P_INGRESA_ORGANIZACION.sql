
CREATE procedure [Seguridad].[Seg_P_INGRESA_ORGANIZACION]
@PV_idCategoria          int,
      @PV_descripcion          VARCHAR(100),
      @PV_idOrgPadre           int,
      @PV_idAplicacion         int,
      @PV_CodRefApli           VARCHAR(8),
      @PV_idOrganizacion       int
AS 
   DECLARE @ln_idOrganizacion      int
   DECLARE @ln_nivel               int

--   IF (EXISTS (SELECT 1 FROM Seguridad.Seg_ORGANIZACION WHERE idorganizacion = @PV_idOrganizacion) )
--   BEGIN
--      RAISERROR ('Código de Organización ya existe',16,1)
--      RETURN
--   END

     SELECT @ln_nivel = nivel
     FROM Seguridad.Seg_ORGANIZACION
     WHERE idorganizacion=@PV_idOrgPadre

     SET @ln_nivel = @ln_nivel + 1
      Select @ln_idOrganizacion = ISNULL(max(idorganizacion),0) + 1
        From Seguridad.Seg_ORGANIZACION 
        
     INSERT INTO Seguridad.Seg_ORGANIZACION(idorganizacion,idcategoria,descripcion,idorgpadre,nivel,idaplicacion,CodRefAplicativo)
            VALUES(@ln_idOrganizacion,@PV_idCategoria,@PV_descripcion,@PV_idOrgPadre,@ln_nivel,@PV_idAplicacion,@PV_CodRefApli)
     --COMMIT
    select @ln_idOrganizacion as IdOrganizacion
  





