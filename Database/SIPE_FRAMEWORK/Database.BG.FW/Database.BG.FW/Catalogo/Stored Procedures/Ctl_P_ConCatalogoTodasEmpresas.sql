CREATE PROC  [Catalogo].[Ctl_P_ConCatalogoTodasEmpresas] 
	@PI_IdTabla int, 
	@PI_Ordenamiento char(1) 
AS 
IF EXISTS ( SELECT 1 FROM .Catalogo.Ctl_CatalogoEmpresa t, .Catalogo.Ctl_Catalogo c 
  WHERE t.IdTabla = c.IdTabla 
	AND t.Codigo = c.Codigo 
	AND t.IdTabla = @PI_IdTabla 
	AND c.Estado = 'A' ) 
BEGIN 
	IF @PI_Ordenamiento = '0' 
	BEGIN
		SELECT t.IdEmpresa, c.Codigo , c.Descripcion , c.DescAlterno 
		  FROM .Catalogo.Ctl_CatalogoEmpresa t, .Catalogo.Ctl_Catalogo c 
		  WHERE t.IdTabla = c.IdTabla 
			AND t.Codigo = c.Codigo 
			AND t.IdTabla = @PI_IdTabla 
			AND c.Estado = 'A' 
		  ORDER BY t.IdEmpresa, c.Descripcion 
	END
	ELSE
	BEGIN
		IF @PI_Ordenamiento = '1' --int
		BEGIN
		SELECT t.IdEmpresa, c.Codigo , c.Descripcion , c.DescAlterno 
		  FROM .Catalogo.Ctl_CatalogoEmpresa t, .Catalogo.Ctl_Catalogo c 
		  WHERE t.IdTabla = c.IdTabla 
			AND t.Codigo = c.Codigo 
			AND t.IdTabla = @PI_IdTabla 
			AND c.Estado = 'A' 
			  ORDER BY t.IdEmpresa, convert(int,c.Descripcion)
		END
	END
END 
ELSE
BEGIN
	--Aseguro que se retorne la estructura
	SELECT -1 IdEmpresa, '-1' Codigo , 'N/A' Descripcion , 'N/A' DescAlterno
END





