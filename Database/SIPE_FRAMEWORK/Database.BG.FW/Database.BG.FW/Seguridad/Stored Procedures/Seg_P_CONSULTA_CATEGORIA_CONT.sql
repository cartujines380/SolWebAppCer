


create procedure [Seguridad].[Seg_P_CONSULTA_CATEGORIA_CONT]
@PV_desc             VARCHAR(100 )
AS
       SELECT idcategoria,descripcion,idcatpadre
       FROM Seguridad.Seg_CATEGORIA
       WHERE UPPER(descripcion) like '%' + UPPER(@PV_desc) + '%'
  





