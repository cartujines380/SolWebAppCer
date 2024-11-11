CREATE view [Catalogo].[Ctl_V_Provincia] ( 
	IdProvincia,
	Nombre,
	IdPais
)
AS
	Select convert(int,c.Codigo), c.Descripcion, convert(int,c.DescAlterno)
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Provincia'
		AND t.IdTabla = c.IdTabla 






