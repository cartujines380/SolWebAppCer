
/*
EXEC [Seguridad].[Seg_P_ActivaClaveParticipante] '<Root IdEmpresa="1" Ruc="1702576651001" Usuario="usrTesting01" IdParticipante=""
	CorreoE="" Celular="" Telefono="" CodImgSegura="" ClaveNew="">
		<Resp Codigo="001" Respuesta="abc" />
	</Root>'
*/
CREATE PROCEDURE [Seguridad].[Seg_P_ActivaClaveParticipante]
(
	@PI_DocXML xml
) AS

	DECLARE
		 @IdParticipante	INT
		,@CodImgSegura		VARCHAR(10)
		,@ClaveNew			VARCHAR(300)

	SELECT
		@IdParticipante		= nref.value('@IdParticipante','INT'),
		@CodImgSegura		= nref.value('@CodImgSegura','VARCHAR(10)'),
		@ClaveNew			= nref.value('@ClaveNew','VARCHAR(300)')
	FROM @PI_DocXML.nodes('/Root') AS R(nref)

	BEGIN TRAN

	UPDATE Participante.Par_Participante
		SET Clave = @ClaveNew
		WHERE IdParticipante = @IdParticipante
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	UPDATE Seguridad.Seg_Clave
		SET
			EsClaveNuevo = 'N',
			EsClaveBloqueo = 'N',
			EsClaveCambio = 'N',
			FechaUltModClave = GETDATE(),
			ImagenSecreta = @CodImgSegura
		WHERE IdParticipante = @IdParticipante
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	DELETE FROM Seguridad.Seg_RespuestaSegura WHERE IdParticipante = @IdParticipante
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	INSERT INTO Seguridad.Seg_RespuestaSegura (IdParticipante, CodPregunta, Respuesta)
		SELECT
			  @IdParticipante
			, nref.value('@Codigo','VARCHAR(10)')
			, nref.value('@Respuesta','VARCHAR(50)')
		FROM @PI_DocXML.nodes('/Root/Resp') AS R(nref)
	IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END

	COMMIT TRAN


