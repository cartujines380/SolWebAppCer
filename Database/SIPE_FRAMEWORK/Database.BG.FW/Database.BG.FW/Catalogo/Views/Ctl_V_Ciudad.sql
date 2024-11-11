create view [Catalogo].[Ctl_V_Ciudad] ( 
	IdCiudad,
	Nombre,
	IdProvincia
)
AS
	Select convert(int,c.Codigo), c.Descripcion, convert(int,c.DescAlterno)
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Ciudad'
		AND t.IdTabla = c.IdTabla 





