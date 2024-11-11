USE [SIPE_PROVEEDOR]
GO

UPDATE C
SET C.DescAlterno='TE'
FROM [Proveedor].[Pro_Tabla] t
INNER JOIN [Proveedor].[Pro_Catalogo] c
	ON t.tabla=c.tabla
WHERE t.TablaNombre='tbl_ProvGrupoCompras'
and C.codigo = 'TE'