

CREATE  VIEW [Seguridad].[Seg_V_ProductoPadre]
AS
	SELECT o.IdOrganizacion, o.Descripcion, c.IdCategoria as IdCategoriaHijo
	FROM Seguridad.Seg_Categoria c, Seguridad.Seg_Organizacion o
	where c.IdCatPadre = o.IdCategoria







