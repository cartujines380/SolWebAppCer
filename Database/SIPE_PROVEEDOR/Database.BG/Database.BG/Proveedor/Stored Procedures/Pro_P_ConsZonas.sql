

CREATE PROCEDURE [Proveedor].[Pro_P_ConsZonas] 
AS
BEGIN
	
	SELECT 
	    DISTINCT TOP 1000  s.CodCiudad as CodZona , c.Detalle as Descripcion
		FROM  Pedidos.Ped_AlmacenSAP s
		inner join Proveedor.Pro_Catalogo c on c.Codigo = s.CodCiudad where c.Tabla = 1031
		and s.KioscoActivo = 'X'
		ORDER BY CodZona
END

