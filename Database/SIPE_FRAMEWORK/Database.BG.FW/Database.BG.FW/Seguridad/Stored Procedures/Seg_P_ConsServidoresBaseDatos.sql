
CREATE Procedure [Seguridad].[Seg_P_ConsServidoresBaseDatos]
AS

	-- Recupera los servidores tipo base de datos Administradores
	SELECT a.IdAplicacion, a.Nombre, a.TipoServidor
	from Seguridad.Seg_aplicacion a
			INNER JOIN Seguridad.Seg_ParamAplicacion pa
			ON a.IdAplicacion = pa.Idaplicacion
	WHERE TipoServidor IN (2,3,6,11) 
		  AND pa.Parametro = 'Administrador' AND pa.Valor = 'S' 


