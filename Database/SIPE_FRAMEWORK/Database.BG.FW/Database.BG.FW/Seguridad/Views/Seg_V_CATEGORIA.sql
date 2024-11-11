
CREATE  VIEW [Seguridad].[Seg_V_CATEGORIA] AS
SELECT Seguridad.Seg_CATEGORIA.IDCATEGORIA as "Codigo" ,
    Seguridad.Seg_CATEGORIA.DESCRIPCION as Descripcion, Seguridad.Seg_CATEGORIA.NIVEL ,
    Seguridad.Seg_CATEGORIA.IDCATPADRE as "CodigoPadre"
    FROM Seguridad.Seg_CATEGORIA 






