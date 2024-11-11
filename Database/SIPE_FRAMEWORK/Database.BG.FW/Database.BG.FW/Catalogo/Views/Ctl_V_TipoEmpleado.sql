
create view [Catalogo].[Ctl_V_TipoEmpleado]
	 (IdTipoEmpleado,Descripcion)  
AS
	Select convert(tinyint,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TipoEmpleado'
		AND t.IdTabla = c.IdTabla





