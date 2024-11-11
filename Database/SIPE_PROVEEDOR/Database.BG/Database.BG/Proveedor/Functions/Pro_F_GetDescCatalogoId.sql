

CREATE FUNCTION [Proveedor].[Pro_F_GetDescCatalogoId]
(
	@Tabla int, @Codigo varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @ResultVar varchar(100)

	SELECT @ResultVar = Detalle
	FROM [Proveedor].[Pro_Catalogo]
	WHERE Tabla = @Tabla AND Codigo = @Codigo

	RETURN @ResultVar
END


