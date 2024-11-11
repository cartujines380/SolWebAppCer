

-- exec [Pedidos].[Ped_P_ConsCiudadesEnAlmacen]

CREATE PROC [Pedidos].[Ped_P_ConsCiudadesEnAlmacen]
AS
BEGIN
	--SELECT DISTINCT c.CodCiudad, c.NomCiudad
	--FROM [Pedidos].[Ped_Almacen] a
	--	INNER JOIN [Pedidos].[Ped_Ciudad] c
	--		ON a.CodCiudad = c.CodCiudad
	--ORDER BY 2

	SELECT DISTINCT c.CodCiudad, c.NomCiudad
	FROM [Pedidos].[Ped_AlmacenSAP] a
		INNER JOIN [Pedidos].[Ped_CiudadSAP] c
			ON a.CodCiudad = c.CodCiudad
	ORDER BY 2
END


