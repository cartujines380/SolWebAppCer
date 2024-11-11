CREATE Procedure [Seguridad].[Seg_P_ConsParamAplicacion]
	@IdAplicacion int
AS
	-- Recupera todos menos el del framework
		SELECT p.Parametro,p.Valor,p.Encriptado
		FROM	Seguridad.Seg_Aplicacion a 
				INNER JOIN Seguridad.Seg_ParamAplicacion  p
				ON a.IdAplicacion = p.IdAplicacion
		WHERE a.IdAplicacion = @IdAplicacion
			  --AND a.TipoServidor > 1 
			  AND a.IdAplicacion > 1
			

