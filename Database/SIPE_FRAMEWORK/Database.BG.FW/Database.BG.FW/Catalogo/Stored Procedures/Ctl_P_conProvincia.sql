create PROCEDURE [Catalogo].[Ctl_P_conProvincia]
@PI_Pais varchar(50)
AS
	Select c.Codigo, c.Descripcion
	FROM .Catalogo.Ctl_Tabla t, .Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Provincia'
		AND t.IdTabla = c.IdTabla
		AND c.Estado = 'A' AND c.DescAlterno = @PI_Pais




