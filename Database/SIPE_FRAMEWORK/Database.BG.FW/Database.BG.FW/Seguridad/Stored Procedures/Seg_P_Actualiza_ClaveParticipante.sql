CREATE procedure [Seguridad].[Seg_P_Actualiza_ClaveParticipante]
		@PV_IdUsuario			 varchar(100),
        @PV_ClaveOld			 varchar(300),
        @PV_ClaveNew             varchar(300)
AS 
	IF EXISTS (SELECT 1 FROM Participante.Par_Participante where IdUsuario = @PV_IdUsuario and Clave = @PV_ClaveOld)
		BEGIN
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

			select 0 CodError, 'Clave actualizada correctamente' MsgError
		END
    ELSE
		BEGIN
			select 13 CodError, 'Contraseña incorrecta' MsgError
		END


