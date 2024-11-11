

CREATE FUNCTION [Proveedor].[Pro_F_GetDescCatalogoIdn]
(
	@Tabla int, @Codigo varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
		
	DECLARE @ResultVar varchar(100)

	IF(@Codigo='PE')
		SET @ResultVar='Pendiente Rine'
	ELSE
	BEGIN
		SELECT @ResultVar = Detalle
		FROM [Proveedor].[Pro_Catalogo]
		WHERE Tabla = @Tabla AND Codigo = @Codigo
	END
	RETURN @ResultVar
END


