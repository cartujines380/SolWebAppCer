USE [SIPE_PROVEEDOR]
GO

UPDATE [Pedidos].[Red_Empresa] 
SET descripcion = 'BANCO GUAYAQUIL' where codEmpresa = 'BC'

IF NOT EXISTS
(
	select * 
	from [Pedidos].[Red_Empresa] 
	where codEmpresa = 'GC' and descripcion = 'GENERACOM'
)
BEGIN
	INSERT INTO [Pedidos].[Red_Empresa] 
	(
		codEmpresa,
		descripcion
	)
	values
	(
		'GC',
		'GENERACOM'
	)
END