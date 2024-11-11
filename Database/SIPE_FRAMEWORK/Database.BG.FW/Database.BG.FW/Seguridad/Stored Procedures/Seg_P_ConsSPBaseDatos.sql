
CREATE PROCEDURE [Seguridad].[Seg_P_ConsSPBaseDatos]
@PI_BaseDatos varchar(100)
AS
  DECLARE @query varchar(1000)
  SET @query = 'SELECT name as Nombre FROM ' + @PI_BaseDatos + 
	'..sysobjects WHERE type=' + char(39) + 'P' + char(39) + ' ORDER BY name '
  EXEC (@query)







