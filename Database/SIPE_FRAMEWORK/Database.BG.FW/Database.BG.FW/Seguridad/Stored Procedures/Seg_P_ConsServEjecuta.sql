
CREATE Procedure [Seguridad].[Seg_P_ConsServEjecuta]
	@IdServidor int
AS
DECLARE @ParamServidor varchar(100)
-- Recupera la instancia del servidor de entrada
SELECT @ParamServidor = Valor
FROM Seguridad.Seg_ParamAplicacion
WHERE IdAplicacion = @IdServidor AND Parametro = 'Servidor'

-- Recupera los servidores que tengan el mismo parametro de servidor que el que se
-- envia como dato
	SELECT a.IdAplicacion, a.Nombre, a.TipoServidor
	from Seguridad.Seg_aplicacion a
			INNER JOIN Seguridad.Seg_ParamAplicacion pa
			ON a.IdAplicacion = pa.Idaplicacion
	WHERE pa.Parametro = 'Servidor' AND pa.Valor =  @ParamServidor


