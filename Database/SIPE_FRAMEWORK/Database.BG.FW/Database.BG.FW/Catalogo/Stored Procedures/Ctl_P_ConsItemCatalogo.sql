
CREATE PROC [Catalogo].[Ctl_P_ConsItemCatalogo]
@PI_IdTabla int,
@PI_Codigo varchar(50)
AS
	SELECT Descripcion, DescAlterno, Estado
	FROM Catalogo.Ctl_Catalogo
		WHERE IdTabla = @PI_IdTabla AND Codigo = @PI_Codigo




