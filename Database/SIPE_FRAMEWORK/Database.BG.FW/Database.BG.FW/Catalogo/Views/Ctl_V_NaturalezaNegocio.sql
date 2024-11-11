create view [Catalogo].[Ctl_V_NaturalezaNegocio] ( 
	IdNaturalezaNegocio,Descripcion)
AS
	Select convert(smallint,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'NaturalezaNegocio'
		AND t.IdTabla = c.IdTabla 




