
CREATE Procedure [Seguridad].[Seg_P_consServApl]
@IdAplicacion int
AS
	-- consulta los servidores asociados a la aplicacion, con sus parametros
	SELECT sa.IdServidor, sa.TipoServidor, a.Nombre, pa.Parametro, pa.Valor, pa.Encriptado
	FROM Seguridad.Seg_ServAplicacion sa 
			INNER JOIN Seguridad.Seg_ParamAplicacion pa
			ON pa.IdAplicacion = sa.IdServidor
			INNER JOIN Seguridad.seg_Aplicacion a
			ON a.IdAplicacion = sa.IdServidor
	WHERE sa.IdAplicacion = @IdAplicacion
	ORDER BY sa.IdServidor


