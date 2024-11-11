
CREATE PROCEDURE [Seguridad].[Seg_P_GrabaUsrAdmin]
	 @PI_DocXML xml
	,@PO_Usuario varchar(20) out
AS
BEGIN

	BEGIN TRAN

	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/New') AS R(nref)))
	BEGIN
		DECLARE @VL_Usuario varchar(20)

		SELECT @VL_Usuario = ISNULL( (SELECT MAX(Usuario)
			FROM [Seguridad].[Seg_Usuario] WHERE EsAdmin = 1), 'PRV00000')
		
		SELECT @VL_Usuario = 'PRV'+ right('00000'+@VL_Usuario,5)

		SET @PO_Usuario = LEFT(@VL_Usuario, 3) + RIGHT( '0000' + CONVERT(VARCHAR, (CONVERT(INT, SUBSTRING(@VL_Usuario, 4, 5)) + 1) ), 5)

		INSERT INTO [Seguridad].[Seg_Usuario]
			(IdEmpresa, Ruc, Usuario, IdParticipante, CodProveedor, CorreoE, Telefono, Celular
			 , EsAdmin, Estado, UsrAutorizador, FechaRegistro)
		SELECT
			 nref.value('@IdEmpresa','INT')
			,nref.value('@Ruc','VARCHAR(13)')
			,@PO_Usuario
			, -1
			,nref.value('@CodProveedor','VARCHAR(10)')
			,nref.value('@CorreoE','VARCHAR(50)')
			,nref.value('@Telefono','VARCHAR(50)')
			,nref.value('@Celular','VARCHAR(50)')
			,cast(1 as bit)
			,nref.value('@Estado','VARCHAR(1)')
			,nref.value('@UsrAutorizador','VARCHAR(20)')
			,GETDATE()
		FROM @PI_DocXML.nodes('/Root/New') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	END
	
	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/NewID') AS R(nref)))
	BEGIN
		UPDATE [Seguridad].[Seg_Usuario]
		SET
			IdParticipante = nref.value('@IdParticipante','INT')
		FROM [Seguridad].[Seg_Usuario] u INNER JOIN
			 @PI_DocXML.nodes('/Root/NewID') AS R(nref)
			 ON
				u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				u.Usuario =  nref.value('@Usuario','VARCHAR(20)')

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	END
	
	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/Mod') AS R(nref)))
	BEGIN
		----VALIDA CORREO REPETIDO
		--IF ( EXISTS (SELECT 1 FROM [Seguridad].[Seg_Usuario] u, @PI_DocXML.nodes('/Root/Mod') AS R(nref)
		--	WHERE u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
		--		u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
		--		u.CorreoE =  nref.value('@CorreoE','VARCHAR(50)') AND
		--		NOT u.Usuario =  nref.value('@Usuario','VARCHAR(20)') ) )
		--BEGIN
		--	ROLLBACK TRAN
		--	RAISERROR ('El Correo Electrónico definido ya fue ingresado para uno de los usuarios asociados a este RUC.', 16, 1)
		--	RETURN 0
		--END

		UPDATE [Seguridad].[Seg_Usuario]
		SET
			  CorreoE = nref.value('@CorreoE','VARCHAR(50)')
			, Telefono = nref.value('@Telefono','VARCHAR(50)')
			, Celular = nref.value('@Celular','VARCHAR(50)')
			, Estado = nref.value('@Estado','VARCHAR(1)')
			, FechaModificacion = GETDATE()
		FROM [Seguridad].[Seg_Usuario] u INNER JOIN
			 @PI_DocXML.nodes('/Root/Mod') AS R(nref)
			 ON
				u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				u.Usuario =  nref.value('@Usuario','VARCHAR(20)')

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	END
	
	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/Eli') AS R(nref)))
	BEGIN
		DELETE u
		FROM [Seguridad].[Seg_Usuario] u INNER JOIN
			 @PI_DocXML.nodes('/Root/Eli') AS R(nref)
			 ON
				u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				u.Usuario =  nref.value('@Usuario','VARCHAR(20)')

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	END

	COMMIT TRAN

END


