
/*
EXEC [Seguridad].[Seg_P_FirstLogonUsrGrabar] '<Root IdEmpresa="1" Ruc="1702576651001" Usuario="usrTesting01" IdParticipante=""
	CorreoE="" Celular="" Telefono="" CodImgSegura="">
		<Resp Codigo="001" Respuesta="abc" />
	</Root>'
*/

CREATE PROCEDURE [Seguridad].[Seg_P_FirstLogonUsrGrabar]
	( @PI_DocXML xml )
AS
BEGIN

	BEGIN TRAN

	UPDATE [Seguridad].[Seg_Usuario]
		SET
			  CorreoE = nref.value('@CorreoE','VARCHAR(50)')
			, Telefono = nref.value('@Telefono','VARCHAR(50)')
			, Celular = nref.value('@Celular','VARCHAR(50)')
		FROM [Seguridad].[Seg_Usuario] u INNER JOIN
			 @PI_DocXML.nodes('/Root') AS R(nref)
			 ON
				u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				u.Usuario =  nref.value('@Usuario','VARCHAR(20)')

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

	COMMIT TRAN

END

