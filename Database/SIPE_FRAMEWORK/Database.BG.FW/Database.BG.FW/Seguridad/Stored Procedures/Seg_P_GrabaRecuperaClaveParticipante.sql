
CREATE PROCEDURE [Seguridad].[Seg_P_GrabaRecuperaClaveParticipante]
	@PV_IdUsuario		varchar(50),
    @PV_ClaveNew		varchar(300)
AS 
	DECLARE @IdPart INT

	SELECT @IdPart = IdParticipante
		FROM Participante.Par_Participante
		WHERE IdUsuario = @PV_IdUsuario

	BEGIN TRAN

	UPDATE Participante.Par_Participante
		SET Clave = @PV_ClaveNew
		WHERE IdUsuario = @PV_IdUsuario
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	UPDATE Seguridad.Seg_Clave
		SET
			EsClaveCambio = 'N',
			FechaUltModClave = GETDATE()
		WHERE IdParticipante = @IdPart
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	COMMIT TRAN


