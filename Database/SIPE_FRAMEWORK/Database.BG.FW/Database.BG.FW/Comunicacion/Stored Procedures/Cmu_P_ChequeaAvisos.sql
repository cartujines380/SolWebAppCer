CREATE PROCEDURE [Comunicacion].[Cmu_P_ChequeaAvisos]  --'admin'
@PI_IdUsuario varchar(50)
AS
	SELECT  IdAviso, 
		IdUsuarioOrigen,
		FechaEnvia,
		CASE WHEN len(isnull(Mensaje,' ')) > 20	THEN substring(Mensaje,1,20)
			 ELSE Mensaje
		END as Mensaje
	FROM Comunicacion.Cmu_Aviso
	WHERE IdUsuarioDestino = @PI_IdUsuario
		AND Estado = 'N'
	ORDER BY FechaEnvia desc


