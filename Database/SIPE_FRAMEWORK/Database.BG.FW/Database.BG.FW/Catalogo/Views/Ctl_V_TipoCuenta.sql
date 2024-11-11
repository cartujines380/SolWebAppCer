CREATE view [Catalogo].[Ctl_V_TipoCuenta] ( 
	IdTipoCuenta ,Descripcion) 
AS
	Select c.Codigo, c.Descripcion
	FROM Catalogo.Ctl_Tabla t, Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'TipoProducto'
		AND t.IdTabla = c.IdTabla

		 







