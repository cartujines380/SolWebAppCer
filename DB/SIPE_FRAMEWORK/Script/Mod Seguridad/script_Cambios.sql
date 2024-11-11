USE [SIPE_FRAMEWORK]
GO

UPDATE [Seguridad].[Seg_Transaccion]
   SET [Auditable] = 'S'
 WHERE  [Menu] = 1
 AND IDORGANIZACION = 39
 AND estado = 'A'
GO

SELECT * FROM [Seguridad].[Seg_Transaccion]
 WHERE  [Menu] = 1
 AND estado = 'A'
  AND IDORGANIZACION = 39