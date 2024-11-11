CREATE Procedure [Seguridad].[Seg_P_ConsServidores]
@TipoServidor int
AS
	SELECT IdServidor = IdAplicacion, Nombre
	FROM Seguridad.Seg_Aplicacion
	WHERE TipoServidor = @TipoServidor


