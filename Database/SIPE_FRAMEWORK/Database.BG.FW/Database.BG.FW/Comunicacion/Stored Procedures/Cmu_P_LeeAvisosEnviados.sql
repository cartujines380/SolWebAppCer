CREATE PROCEDURE [Comunicacion].[Cmu_P_LeeAvisosEnviados]
@PI_IdUsuario varchar(50)
AS
	SELECT IdAviso,
		IdUsuarioDestino as Usuario,
		FechaEnvia,
		Estado
	FROM Comunicacion.Cmu_Aviso
	WHERE IdUsuarioOrigen = @PI_IdUsuario
	ORDER BY IdAviso desc



