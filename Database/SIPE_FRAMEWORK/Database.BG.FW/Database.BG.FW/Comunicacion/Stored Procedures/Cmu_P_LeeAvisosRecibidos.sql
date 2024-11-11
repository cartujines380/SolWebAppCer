CREATE PROCEDURE [Comunicacion].[Cmu_P_LeeAvisosRecibidos]
@PI_IdUsuario varchar(50)
AS
	SELECT IdAviso,IdUsuarioOrigen as Usuario,
		FechaEnvia,
		Estado
	FROM Comunicacion.Cmu_Aviso
	WHERE IdUsuarioDestino = @PI_IdUsuario
	ORDER BY IdAviso desc


