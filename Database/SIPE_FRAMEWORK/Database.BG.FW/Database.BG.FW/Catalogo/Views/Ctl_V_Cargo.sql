create view [Catalogo].[Ctl_V_Cargo] ( 
	IdCargo, Descripcion, Jerarquia)  
AS
	Select convert(tinyint,c.Codigo), c.Descripcion, c.DescAlterno
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Cargo'
		AND t.IdTabla = c.IdTabla





