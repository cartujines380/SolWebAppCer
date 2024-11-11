create view [Catalogo].[Ctl_V_Documento] ( 
	IdDocumento ,
	Descripcion )  
AS
	Select convert(tinyint,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Documento'
		AND t.IdTabla = c.IdTabla




