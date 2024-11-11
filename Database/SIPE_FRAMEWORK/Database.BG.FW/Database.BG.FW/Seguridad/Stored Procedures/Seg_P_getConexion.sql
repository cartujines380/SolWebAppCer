

CREATE  procedure [Seguridad].[Seg_P_getConexion]
@PI_IdAplicacion   int
AS
  SET NOCOUNT ON
  	-- Se va a controlar que si el usuario esta registrado, 
	
	SELECT Parametro,Valor, Encriptado
	FROM Seguridad.Seg_ParamAplicacion
	WHERE IdAplicacion = @PI_IdAplicacion
	UNION
	SELECT Parametro='TipoServidor', Valor = TipoServidor, 0 as Encriptado
	FROM Seguridad.Seg_Aplicacion
	WHERE IdAplicacion = @PI_IdAplicacion


