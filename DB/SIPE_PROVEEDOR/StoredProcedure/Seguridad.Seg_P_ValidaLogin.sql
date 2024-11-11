USE [SIPE_FRAMEWORK]
GO

/****** Object:  StoredProcedure [Seguridad].[Seg_P_ValidaLogin]    Script Date: 01/04/2020 14:05:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- exec [Seguridad].[Seg_P_ValidaLogin] 'usrmtraframe','QFCJ1Do+XOtcPMT2TnOyA+S0HR431kS00lEN6LydpDI='

CREATE PROCEDURE [Seguridad].[Seg_P_ValidaLogin]
@PV_Identificacion varchar(100),
@PV_Clave varchar(300),
@PV_Sociedad varchar(5)=null,
@PV_Transaccion varchar(300)='2'
AS
BEGIN
	DECLARE @IdParticipante INT,
			@Clave VARCHAR(300),
			@FechaUltClaveErr DATETIME,
			@NumIntentosClaveErr SMALLINT,
			@EsClaveNuevo VARCHAR(1),
			@EsClaveCambio VARCHAR(1),
			@EsClaveBloqueo VARCHAR(1),
			@Estado VARCHAR(10),
			@CodError VARCHAR(10),
			@Cantidad int=99999
	
	SET @CodError = '3013' --'Error, usuario o contraseña incorrecta'

	IF(@PV_Transaccion='2')
	BEGIN
		SELECT @IdParticipante = PA.IdParticipante, @Clave = Clave, @Estado = PA.Estado
		FROM Participante.Par_Participante PA
		where IdUsuario = @PV_Identificacion
		--AND  ISNULL(US.Sociedad,'')=ISNULL(@PV_Sociedad,ISNULL(US.Sociedad,''))
		--AND  US.Sociedad=ISNULL(@PV_Sociedad,US.Sociedad)
	END
	ELSE
	BEGIN

		SELECT @Cantidad=count(1)
				FROM Participante.Par_Participante PA
				INNER JOIN SIPE_PROVEEDOR.Seguridad.Seg_Usuario US
				ON PA.IdParticipante=US.IdParticipante
				where IdUsuario = @PV_Identificacion
				--AND  ISNULL(US.Sociedad,'')=ISNULL(@PV_Sociedad,ISNULL(US.Sociedad,''))
				AND  US.Sociedad=@PV_Sociedad

		SELECT @IdParticipante = PA.IdParticipante, @Clave = Clave, @Estado = PA.Estado
		FROM Participante.Par_Participante PA
		INNER JOIN SIPE_PROVEEDOR.Seguridad.Seg_Usuario US
		ON PA.IdParticipante=US.IdParticipante
		where IdUsuario = @PV_Identificacion
		--AND  ISNULL(US.Sociedad,'')=ISNULL(@PV_Sociedad,ISNULL(US.Sociedad,''))
		AND  US.Sociedad=@PV_Sociedad


	END
	

	IF (@@ROWCOUNT > 0)
		BEGIN

			IF (@Estado = 'I')
				BEGIN
					RAISERROR('Usuario no se encuentra activo.', 16, 1)
					RETURN
				END

			SELECT	@FechaUltClaveErr = FechaUltClaveErr,
					@NumIntentosClaveErr = NumIntentosClaveErr,
					@EsClaveNuevo = EsClaveNuevo,
					@EsClaveCambio = EsClaveCambio,
					@EsClaveBloqueo = EsClaveBloqueo
				FROM Seguridad.Seg_Clave
				WHERE IdParticipante = @IdParticipante

			IF (@EsClaveBloqueo = 'S')
				BEGIN
					
					SET @CodError = '14' --'Error, usuario bloqueado'

				END
			ELSE
				BEGIN

					IF (@Clave = @PV_Clave)
						BEGIN

							SET @CodError = '0' --'Ingreso exitoso'

							IF (@NumIntentosClaveErr >  0)
								BEGIN

									BEGIN TRAN

									UPDATE Seguridad.Seg_Clave
										SET NumIntentosClaveErr =  0
										WHERE IdParticipante = @IdParticipante

									IF (@@ERROR <> 0)
										BEGIN
											ROlLBACK TRAN
											RETURN
										END

									COMMIT TRAN

								END
						END
					ELSE
						BEGIN
						
							SET @NumIntentosClaveErr = @NumIntentosClaveErr + 1

							IF (@FechaUltClaveErr> DATEADD(hour,-6,GETDATE()) AND @NumIntentosClaveErr >= 11)
								BEGIN

									DECLARE @Asig_EsClaveBloqueo VARCHAR(1)

									SET @Asig_EsClaveBloqueo = 'N'

									IF (NOT @PV_Identificacion IN ('usrmtraframe'))
										BEGIN

											SET @CodError = '14' --'Error, usuario bloqueado'

											SET @Asig_EsClaveBloqueo = 'S'

										END

									BEGIN TRAN

									UPDATE Seguridad.Seg_Clave
										SET EsClaveBloqueo = @Asig_EsClaveBloqueo,
											FechaUltClaveErr = GETDATE(),
											NumIntentosClaveErr =  @NumIntentosClaveErr
										WHERE IdParticipante = @IdParticipante

									IF (@@ERROR <> 0)
										BEGIN
											ROlLBACK TRAN
											RETURN
										END

									COMMIT TRAN

								END
							ELSE
								BEGIN

									IF (NOT @FechaUltClaveErr> DATEADD(hour,-6,GETDATE()))
										BEGIN
											SET @NumIntentosClaveErr = 1
										END
										
									BEGIN TRAN

									UPDATE Seguridad.Seg_Clave
										SET FechaUltClaveErr = GETDATE(),
											NumIntentosClaveErr =  @NumIntentosClaveErr
										WHERE IdParticipante = @IdParticipante

									IF (@@ERROR <> 0)
										BEGIN
											ROlLBACK TRAN
											RETURN
										END
									
									COMMIT TRAN

								END

						END

				END
		END

	SELECT @CodError CodError,
		CASE @CodError
			WHEN '0' THEN 'Ingreso exitoso'
			WHEN '14' THEN 'Error, usuario bloqueado'
			WHEN '3013' THEN 'Error, usuario o contraseña incorrecta'
		END as MsgError

END
GO


