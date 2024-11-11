
CREATE PROCEDURE [Seguridad].[Seg_P_DesbloqueoClaveParticipante]
	@PV_IdUsuario		varchar(50),
    @PV_CambiarClave	varchar(1),
    @PV_ClaveNew		varchar(300)
AS 
	DECLARE @IdPart INT

	SELECT @IdPart = IdParticipante
		FROM Participante.Par_Participante
		WHERE IdUsuario = @PV_IdUsuario

	BEGIN TRAN

	IF (@PV_CambiarClave = 'S')
		BEGIN

			UPDATE Participante.Par_Participante
				SET Clave = @PV_ClaveNew
				WHERE IdUsuario = @PV_IdUsuario
			IF (@@ERROR <> 0)
				BEGIN
					ROLLBACK TRAN
					RETURN
				END
				
			IF (EXISTS(SELECT TOP 1 r.CodPregunta FROM [Seguridad].[Seg_RespuestaSegura] r
						WHERE r.IdParticipante = @IdPart))
				BEGIN
					UPDATE Seguridad.Seg_Clave
						SET
							EsClaveBloqueo = 'N',
							EsClaveCambio = 'S'
						WHERE IdParticipante = @IdPart
				END
			ELSE
				BEGIN
					UPDATE Seguridad.Seg_Clave
						SET
							EsClaveBloqueo = 'N',
							EsClaveCambio = 'N',
							EsClaveNuevo = 'S'
						WHERE IdParticipante = @IdPart
				END
			IF (@@ERROR <> 0)
				BEGIN
					ROLLBACK TRAN
					RETURN
				END

		END
	ELSE
		BEGIN

			UPDATE Seguridad.Seg_Clave
				SET
					EsClaveBloqueo = 'N'
				WHERE IdParticipante = @IdPart
			IF (@@ERROR <> 0)
				BEGIN
					ROLLBACK TRAN
					RETURN
				END

		END

	COMMIT TRAN


