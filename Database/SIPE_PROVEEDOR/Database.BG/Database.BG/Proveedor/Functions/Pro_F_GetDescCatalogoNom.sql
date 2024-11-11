

CREATE FUNCTION [Proveedor].[Pro_F_GetDescCatalogoNom]
(
	@TblNombre int, @Codigo varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @ResultVar varchar(100)

	SELECT @ResultVar = c.Detalle
	FROM [Proveedor].[Pro_Catalogo] c
		INNER JOIN [Proveedor].[Pro_Tabla] t
			ON t.Tabla = c.Tabla
	WHERE t.TablaNombre = @TblNombre AND Codigo = @Codigo

	RETURN @ResultVar
END


