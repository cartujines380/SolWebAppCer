
CREATE procedure [Seguridad].[Seg_P_CONSULTA_ORGANIZACION]
@PV_idOrganizacion    int
        
AS
DECLARE @PV_descripcion      VARCHAR(100 ) ,
        @PV_idCategoria      int ,
        @PV_idOrgPadre       int ,
        @PV_idAplicacion     int ,
        @PV_descCategoria    VARCHAR(100 ) ,
        @PV_descOrgPadre     VARCHAR(100 ) ,
        @PV_CodrefApli       VARCHAR(8 ) 
  
     SELECT 
      @PV_descripcion=descripcion,
      @PV_idCategoria=idCategoria,
      @PV_idOrgPadre=idOrgPadre,
      @PV_idAplicacion=idAplicacion,
      @PV_CodrefApli=CodRefAplicativo 

     FROM Seguridad.Seg_ORGANIZACION
     WHERE idOrganizacion=@Pv_idOrganizacion

    
      SELECT @PV_descCategoria = descripcion  
      FROM Seguridad.Seg_CATEGORIA
      WHERE idcategoria=@PV_idCategoria

      SELECT @PV_descOrgPadre = descripcion 
      FROM Seguridad.Seg_ORGANIZACION
      WHERE idorganizacion=@PV_idOrgPadre

select  @PV_descripcion as descripcion,
        @PV_idCategoria as categoria,
        @PV_idOrgPadre as orgpadre,
        @PV_descCategoria as desccat,
        @PV_descOrgPadre as descorgpadre,
        @PV_idAplicacion as aplicacion,
        @PV_CodrefApli as CodrefApli

 







