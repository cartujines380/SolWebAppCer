
create PROCEDURE [Catalogo].[Ctl_P_conCiudad]
@PI_Provincia varchar(50)
AS
	Select c.Codigo, c.Descripcion
	FROM .Catalogo.Ctl_Tabla t, .Catalogo.Ctl_Catalogo c
	WHERE t.Nombre = 'Ciudad'
		AND t.IdTabla = c.IdTabla
		AND c.Estado = 'A' AND c.DescAlterno = @PI_Provincia




