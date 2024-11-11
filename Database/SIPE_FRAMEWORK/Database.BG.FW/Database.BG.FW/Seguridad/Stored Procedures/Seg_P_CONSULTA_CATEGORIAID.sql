
CREATE  procedure [Seguridad].[Seg_P_CONSULTA_CATEGORIAID]
@PV_idCategoria  int
     -- @PV_descripcion    VARCHAR(100 ) OUTPUT,
     -- @PV_CatPadre       int OUTPUT,
     -- @PV_DescPadre      VARCHAR(100 ) OUTPUT
AS 

    SELECT a.descripcion,
           a.idcatpadre as catpadre,
           b.descripcion as descpadre
     FROM Seguridad.Seg_CATEGORIA a  LEFT OUTER JOIN  Seguridad.Seg_CATEGORIA b
			ON a.idcatpadre = b.idcategoria
     WHERE a.idcategoria= @Pv_idCategoria
	   







