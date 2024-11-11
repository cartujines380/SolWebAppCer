create view [Catalogo].[Ctl_V_TituloProfesional] ( 
	IdTitulo,Descripcion)  
AS
	Select convert(smallint,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TituloProfesional'
		AND t.IdTabla = c.IdTabla 




