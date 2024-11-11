USE SIPE_PROVEEDOR
GO

DECLARE @IdTabla int,
		@count int,
		@codCategoria varchar(10),
		@descripcion varchar(50)

SELECT @IdTabla = max(Tabla + 1) from [Proveedor].[Pro_Tabla]

IF not exists
(
	select * 
	from [Proveedor].[Pro_Tabla]
	where TablaNombre = 'tbl_LineaNegocio'
)
BEGIN
	INSERT INTO [Proveedor].[Pro_Tabla]
	(
		tabla,
		TablaNombre,
		estado
	)
	values
	(
		@IdTabla,
		'tbl_LineaNegocio',
		'A'
	)
END
ELSE
BEGIN
	SELECT @IdTabla = Tabla from [Proveedor].[Pro_Tabla]
	where TablaNombre = 'tbl_LineaNegocio'
END

IF OBJECT_ID('tempdb..#LineaNegocio') IS NOT NULL
BEGIN
	DROP TABLE #Categoria
END

SELECT *
into #LineaNegocio 
FROM [Pedidos].[Red_Categoria]

SELECT @count = COUNT(*) FROM #LineaNegocio

WHILE @count > 0
BEGIN
	SELECT 
		TOP(1)
		@codCategoria = codEmpresa
	FROM #LineaNegocio

	SELECT 
		@descripcion = descripcion
	FROM [Pedidos].[Red_Categoria]
	WHERE codEmpresa = @codCategoria

	IF not exists
	(
		select * 
		from [Proveedor].[Pro_Catalogo]
		where tabla = @IdTabla and codigo = @codCategoria
	)
	BEGIN
		INSERT INTO [Proveedor].[Pro_Catalogo]
		(
			tabla,
			codigo,
			detalle,
			estado
		)
		values
		(
			@IdTabla,
			@codCategoria,
			@descripcion,
			'A'
		)
	END

	DELETE TOP (1) FROM #LineaNegocio
    SELECT @count = COUNT(*) FROM #LineaNegocio
END




