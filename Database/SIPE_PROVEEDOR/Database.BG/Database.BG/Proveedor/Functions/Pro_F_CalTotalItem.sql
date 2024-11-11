

CREATE FUNCTION [Proveedor].[Pro_F_CalTotalItem]
(
	@Cantidad numeric(11,2),
	@Precio numeric(11,2),
	@Descuento1 numeric(11,2),
	@Descuento2 numeric(11,2),
	@IndIva1 varchar(1)
)
RETURNS numeric(11,2)
AS
BEGIN
	DECLARE @ResultVar numeric(11,2)
	DECLARE @VL_Val100 numeric(11,2)
	SET @VL_Val100 = convert(numeric(11,2), 100)

	SELECT @ResultVar =
			(
				--(
				--	(@Precio - ( (@Descuento1/@VL_Val100) * @Precio ) )
				--		-
				--	(
				--		(@Descuento2/@VL_Val100) * 
				--		(@Precio - ( (@Descuento1/@VL_Val100) * @Precio ) )
				--	)
				--)
				--* @Cantidad
				@Precio * @Cantidad
			)
			* CASE WHEN @IndIva1 = '2' THEN CAST(1.12 AS NUMERIC(11,2)) ELSE CAST(1 AS NUMERIC(11,2)) END

	RETURN @ResultVar
END


