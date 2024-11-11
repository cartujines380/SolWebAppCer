
create PROCEDURE [Seguridad].[Seg_P_ConsBaseDatos]
AS
	
SELECT name as Nombre FROM master..sysdatabases
	WHERE dbid > 4
	ORDER BY name







