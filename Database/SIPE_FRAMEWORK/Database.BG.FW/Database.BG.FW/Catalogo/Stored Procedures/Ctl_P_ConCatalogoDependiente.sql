CREATE PROC  [Catalogo].[Ctl_P_ConCatalogoDependiente] 
@PI_CodCatalogoPadre varchar(50),
@PI_IdTablaHija int
AS
	SELECT Codigo, Descripcion, DescAlterno, Estado, Relacion1 
		FROM Catalogo.Ctl_Catalogo
		WHERE	IdTabla = @PI_IdTablaHija
			AND DescAlterno in(@PI_CodCatalogoPadre,'-1')
	ORDER BY Codigo

