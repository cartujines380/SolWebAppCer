CREATE  view [Catalogo].[Ctl_V_TipoDireccion] ( 
	IdTipoDireccion ,
	Descripcion)  
AS
	Select convert(int,c.Codigo), c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TipoDireccion'
		AND t.IdTabla = c.IdTabla 





