CREATE View [Seguridad].[Seg_V_Servidores] AS
SELECT IdAplicacion as IdServidor, Nombre, TipoServidor
	FROM Seguridad.Seg_Aplicacion
	WHERE TipoServidor IN (2,3,5,6,7,9)

