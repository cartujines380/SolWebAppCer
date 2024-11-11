USE [SIPE_PROVEEDOR]
GO

DELETE c
FROM [Proveedor].[Pro_Tabla] t
INNER JOIN [Proveedor].[Pro_Catalogo] c
	ON t.tabla=c.tabla
WHERE t.tablaNombre='tbl_LineaNegocio'
and C.codigo in ('F','J','P','R','S')