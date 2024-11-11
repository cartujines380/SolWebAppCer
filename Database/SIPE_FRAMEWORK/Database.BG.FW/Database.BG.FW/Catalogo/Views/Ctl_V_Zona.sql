create view [Catalogo].[Ctl_V_Zona] ( 
	IdZona ,Descripcion) 
AS
	Select convert(tinyint,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Zona'
		AND t.IdTabla = c.IdTabla 




