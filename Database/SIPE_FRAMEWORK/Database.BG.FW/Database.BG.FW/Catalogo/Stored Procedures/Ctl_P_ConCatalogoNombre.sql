CREATE PROC  [Catalogo].[Ctl_P_ConCatalogoNombre] 
@PI_NomCatalogo varchar(50), 
@PI_Ordenamiento char(1) 
AS
IF @PI_Ordenamiento = '0' 
BEGIN
	SELECT Catalogo.Ctl_Catalogo.Codigo , Catalogo.Ctl_Catalogo.Descripcion , Catalogo.Ctl_Catalogo.DescAlterno 
	  FROM .Catalogo.Ctl_Catalogo, .Catalogo.Ctl_Tabla
	  Where Catalogo.Ctl_Tabla.idTabla = Catalogo.Ctl_Catalogo.idTabla 
		AND Catalogo.Ctl_Tabla.Nombre = @PI_NomCatalogo 
		AND Catalogo.Ctl_Catalogo.Estado = 'A'
END
ELSE
BEGIN
	IF @PI_Ordenamiento = '1' --int
	BEGIN
		SELECT Catalogo.Ctl_Catalogo.Codigo , Catalogo.Ctl_Catalogo.Descripcion , Catalogo.Ctl_Catalogo.DescAlterno 
		  FROM .Catalogo.Ctl_Catalogo, .Catalogo.Ctl_Tabla
		  Where Catalogo.Ctl_Tabla.idTabla = Catalogo.Ctl_Catalogo.idTabla 
			AND Catalogo.Ctl_Tabla.Nombre = @PI_NomCatalogo 
			AND Catalogo.Ctl_Catalogo.Estado = 'A'
		  ORDER BY convert(int,Catalogo.Ctl_Catalogo.Descripcion)
	END
END
--Aseguro que se retorne la estructura
IF @@rowcount = 0
BEGIN
	SELECT 'N/A' Codigo , 'N/A' Descripcion , 'N/A' DescAlterno
END





