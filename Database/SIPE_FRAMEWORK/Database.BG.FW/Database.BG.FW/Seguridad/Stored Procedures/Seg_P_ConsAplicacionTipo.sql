CREATE PROCEDURE [Seguridad].[Seg_P_ConsAplicacionTipo] 
@TipoServidor varchar(10)
AS

	select IdAplicacion, Nombre,
		SrvEsAdm = isnull(  (Select 'S' From Seguridad.Seg_paramaplicacion p 
					WHERE a.IdAplicacion = p.IdAplicacion 
					 AND p.Parametro = 'Administrador' 
					 AND p.valor = 'S'), 'N' )
	from Seguridad.Seg_aplicacion a
	WHERE TipoServidor = @TipoServidor




