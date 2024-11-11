

CREATE   PROCEDURE [Seguridad].[Seg_p_Retorna_Conexion]
@PV_idAplicacion      int
AS

   SELECT Parametro, Valor --servidor, usuario, clave
     FROM Seguridad.Seg_ParamApliccion
     WHERE IdAplicacion=@PV_idAplicacion





