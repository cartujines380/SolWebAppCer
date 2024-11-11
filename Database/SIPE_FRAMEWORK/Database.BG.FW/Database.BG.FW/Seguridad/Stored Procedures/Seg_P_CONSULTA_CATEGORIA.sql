




create procedure [Seguridad].[Seg_P_CONSULTA_CATEGORIA]
@PV_idCategoria           int
AS
       SELECT idcategoria,descripcion
       FROM Seguridad.Seg_CATEGORIA
       WHERE idcategoria>=@PV_idCategoria
  





