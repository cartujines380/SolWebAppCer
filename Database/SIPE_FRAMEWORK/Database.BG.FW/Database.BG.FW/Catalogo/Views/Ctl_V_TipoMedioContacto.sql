create view [Catalogo].[Ctl_V_TipoMedioContacto] ( 
	IdTipoMedioContacto,Descripcion )
AS
	Select convert(int,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TipoMedioContacto'
		AND t.IdTabla = c.IdTabla 





