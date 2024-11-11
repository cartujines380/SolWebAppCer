USE SIPE_PROVEEDOR
GO

DECLARE @IdTabla int

SELECT @IdTabla = max(Tabla + 1) from [Proveedor].[Pro_Tabla]

IF not exists
(
	select * 
	from [Proveedor].[Pro_Tabla]
	where TablaNombre = 'tbl_RedEmpresa'
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
		'tbl_RedEmpresa',
		'A'
	)
END
ELSE
BEGIN
	SELECT @IdTabla = Tabla from [Proveedor].[Pro_Tabla]
	where TablaNombre = 'tbl_RedEmpresa'
END

IF not exists
(
	select * 
	from [Proveedor].[Pro_Catalogo]
	where tabla = @IdTabla and codigo = 'BC'
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
		'BC',
		'BANCO GUAYAQUIL',
		'A'
	)
END

IF not exists
(
	select * 
	from [Proveedor].[Pro_Catalogo]
	where tabla = @IdTabla and codigo = 'GC'
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
		'GC',
		'GENERACOM',
		'A'
	)
END
	




