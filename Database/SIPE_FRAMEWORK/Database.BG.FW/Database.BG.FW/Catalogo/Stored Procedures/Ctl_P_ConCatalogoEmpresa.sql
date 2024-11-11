CREATE PROC [Catalogo].[Ctl_P_ConCatalogoEmpresa]
	@PI_IdEmpresa int,
	@PI_IdTabla int = NULL,
	@PI_Ordenamiento char(1)
AS

IF @PI_Ordenamiento = '0'
BEGIN
	SELECT 
		t.IdTabla, 
		c.Codigo, 
		c.Descripcion, 
		c.DescAlterno 
	  FROM .Catalogo.Ctl_Tabla t, .Catalogo.Ctl_Catalogo c 
	  WHERE t.IdTabla = c.IdTabla 
		AND ( t.IdTabla = @PI_IdTabla OR @PI_IdTabla IS NULL) 
		AND c.Estado = 'A' 
		AND EXISTS (SELECT 1 FROM .Catalogo.Ctl_CatalogoEmpresa e 
			WHERE t.IdTabla = e.IdTabla 
				AND e.IdEmpresa = @PI_IdEmpresa 
				AND ( e.IdTabla = @PI_IdTabla OR @PI_IdTabla IS NULL) 
				AND e.Codigo = c.Codigo ) 
	  ORDER BY t.IdTabla, c.Descripcion 
END
ELSE
BEGIN
	IF @PI_Ordenamiento = '1' --int
	BEGIN
		SELECT 
			t.IdTabla, 
			c.Codigo, 
			c.Descripcion, 
			c.DescAlterno 
		  FROM .Catalogo.Ctl_Tabla t, .Catalogo.Ctl_Catalogo c 
		  WHERE t.IdTabla = c.IdTabla 
			AND ( t.IdTabla = @PI_IdTabla OR @PI_IdTabla IS NULL) 
			AND c.Estado = 'A' 
			AND EXISTS (SELECT 1 FROM .Catalogo.Ctl_CatalogoEmpresa e 
				WHERE t.IdTabla = e.IdTabla 
					AND e.IdEmpresa = @PI_IdEmpresa 
					AND ( e.IdTabla = @PI_IdTabla OR @PI_IdTabla IS NULL) 
					AND e.Codigo = c.Codigo ) 
		  ORDER BY t.IdTabla, convert(int,c.Descripcion)
	END
END





