USE [SIPE_PROVEEDOR]
GO

UPDATE Seguridad.Seg_EmpleadoLinea
SET linea = 'LE'
WHERE ruc = 'cgarcia' and Usuario = 'cgarcia' and Linea = 'P'

UPDATE Seguridad.Seg_EmpleadoLinea
SET linea = 'PU'
WHERE ruc = 'cgarcia' and Usuario = 'cgarcia' and Linea = 'J'

UPDATE Seguridad.Seg_EmpleadoLinea
SET linea = 'TE'
WHERE ruc = 'cgarcia' and Usuario = 'cgarcia' and Linea = 'F'