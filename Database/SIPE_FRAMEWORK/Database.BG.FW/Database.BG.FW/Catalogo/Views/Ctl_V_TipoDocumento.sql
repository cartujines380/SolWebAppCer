create view [Catalogo].[Ctl_V_TipoDocumento] ( 
	IdTipoDocumento ,
	Descripcion )  
AS
	Select convert(int,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TipoDocumento'
		AND t.IdTabla = c.IdTabla





