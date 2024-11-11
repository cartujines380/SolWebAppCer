

CREATE  VIEW [Seguridad].[Seg_V_CategoriaPadre]
AS
	SELECT c.IdCategoria, c.Descripcion, c.IdCategoria as IdCategoriaHijo
	FROM Seguridad.Seg_Categoria c, Seguridad.Seg_Organizacion o
	where c.IdCatPadre = o.IdCategoria






