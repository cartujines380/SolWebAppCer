create view [Catalogo].[Ctl_V_Pais] ( 
	IdPais,	Nombre
	)  
AS
	Select convert(int,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Pais'
		AND t.IdTabla = c.IdTabla 




