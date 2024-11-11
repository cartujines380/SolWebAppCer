
CREATE PROCEDURE [Proveedor].[Pro_P_ConsCatalogo] 
	@PI_Tabla varchar(50)
AS
BEGIN

	SELECT codigo, detalle, DescAlterno
	  FROM [Proveedor].[Pro_Catalogo] C
	  INNER JOIN [Proveedor].[Pro_Tabla] T ON 
	  C.tabla = T.tabla
	  WHERE T.TablaNombre = @PI_Tabla
	  AND C.ESTADO='A'
	  ORDER BY detalle

END


	
