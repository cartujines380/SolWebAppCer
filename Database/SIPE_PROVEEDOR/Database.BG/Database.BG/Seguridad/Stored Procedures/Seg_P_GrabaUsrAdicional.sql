
CREATE PROCEDURE [Seguridad].[Seg_P_GrabaUsrAdicional]
	 @PI_DocXML xml
AS
BEGIN

	BEGIN TRAN

	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/New') AS R(nref)))
	BEGIN
		--VALIDA IDENTIFICACIÓN REPETIDA
		IF ( EXISTS (SELECT 1 FROM [Seguridad].[Seg_UsuarioAdicional] a, @PI_DocXML.nodes('/Root/New') AS R(nref)
			WHERE a.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				a.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				a.Identificacion =  nref.value('@Identificacion','VARCHAR(13)') AND
				NOT a.Usuario =  nref.value('@Usuario','VARCHAR(20)') ) )
		BEGIN
			ROLLBACK TRAN
			RAISERROR ('La Identificación ya fue ingresada para uno de los usuarios asociados a este RUC.', 16, 1)
			RETURN 0
		END

		--VALIDA CORREO REPETIDO
		--IF ( EXISTS (SELECT 1 FROM [Seguridad].[Seg_Usuario] u, @PI_DocXML.nodes('/Root/New') AS R(nref)
		--	WHERE u.IdEmpresa = nref.value('@IdEmpresa','INT') AND
		--		u.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
		--		u.CorreoE =  nref.value('@CorreoE','VARCHAR(50)') AND
		--		NOT u.Usuario =  nref.value('@Usuario','VARCHAR(20)') ) )
		--BEGIN
		--	ROLLBACK TRAN
		--	RAISERROR ('El Correo Electrónico definido ya fue ingresado para uno de los usuarios asociados a este RUC.', 16, 1)
		--	RETURN 0
		--END

		INSERT INTO [Seguridad].[Seg_Usuario]
			(IdEmpresa, Ruc, Usuario, IdParticipante, CodProveedor, CorreoE, Telefono, Celular
			, EsAdmin, Estado, UsrAutorizador, FechaRegistro,UsrCargo,UsrFuncion)
		SELECT
			 nref.value('@IdEmpresa','INT')
			,nref.value('@Ruc','VARCHAR(13)')
			,nref.value('@Usuario','VARCHAR(20)')
			,nref.value('@IdParticipante','INT')
			,nref.value('@CodProveedor','VARCHAR(10)')
			,nref.value('@CorreoE','VARCHAR(50)')
			,nref.value('@Telefono','VARCHAR(50)')
			,nref.value('@Celular','VARCHAR(50)')
			,cast(0 as bit)
			,nref.value('@Estado','VARCHAR(1)')
			,nref.value('@UsrAutorizador','VARCHAR(20)')
			,GETDATE()
			,nref.value('@Departamento','VARCHAR(5)')
			,nref.value('@Funcion','VARCHAR(5)')
			
		FROM @PI_DocXML.nodes('/Root/New') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
		
		INSERT INTO [Seguridad].[Seg_UsuarioAdicional]
			(IdEmpresa, Ruc, Usuario, TipoIdent, Identificacion, Apellido1, Apellido2, Nombre1, Nombre2, EstadoCivil, Genero, Pais, Provincia, Ciudad, Direccion,RecibeActas)
		SELECT
			 nref.value('@IdEmpresa','INT')
			,nref.value('@Ruc','VARCHAR(13)')
			,nref.value('@Usuario','VARCHAR(20)')
			,nref.value('@TipoIdent','VARCHAR(10)')
			,nref.value('@Identificacion','VARCHAR(13)')
			,nref.value('@Apellido1','VARCHAR(100)')
			,nref.value('@Apellido2','VARCHAR(100)')
			,nref.value('@Nombre1','VARCHAR(100)')
			,nref.value('@Nombre2','VARCHAR(100)')
			,nref.value('@EstadoCivil','VARCHAR(10)')
			,nref.value('@Genero','VARCHAR(10)')
			,nref.value('@Pais','VARCHAR(10)')
			,nref.value('@Provincia','VARCHAR(10)')
			,nref.value('@Ciudad','VARCHAR(10)')
			,nref.value('@Direccion','VARCHAR(300)')
			,CONVERT(BIT,nref.value('@RecibeActas','VARCHAR(1)'))
		FROM @PI_DocXML.nodes('/Root/New') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
		
		INSERT INTO [Seguridad].[Seg_UsuarioZona]
			(IdEmpresa, Ruc, Usuario, Zona)
		SELECT
			 nref.value('../@IdEmpresa','INT')
			,nref.value('../@Ruc','VARCHAR(13)')
			,nref.value('../@Usuario','VARCHAR(20)')
			,nref.value('@Zona','VARCHAR(12)')
		FROM @PI_DocXML.nodes('/Root/New/Zona') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		INSERT INTO [Seguridad].[Seg_UsrZonaAlmacen]
			(IdEmpresa, Ruc, Usuario, Zona, Almacen)
		SELECT
			 nref.value('../@IdEmpresa','INT')
			,nref.value('../@Ruc','VARCHAR(13)')
			,nref.value('../@Usuario','VARCHAR(20)')
			,nref.value('@codCiudad','VARCHAR(12)')
			,nref.value('@codAlmacen','VARCHAR(4)')
		FROM @PI_DocXML.nodes('/Root/New/Alm') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

	END
	
	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/Mod') AS R(nref)))
	BEGIN
		--VALIDA IDENTIFICACIÓN REPETIDA
		IF ( EXISTS (SELECT 1 FROM [Seguridad].[Seg_UsuarioAdicional] a, @PI_DocXML.nodes('/Root/Mod') AS R(nref)
			WHERE a.IdEmpresa = nref.value('@IdEmpresa','INT') AND
				a.Ruc =  nref.value('@Ruc','VARCHAR(13)') AND
				a.Identificacion =  nref.value('@Identificacion','VARCHAR(13)') AND
				NOT a.Usuario =  nref.value('@Usuario','VARCHAR(20)') ) )
		BEGIN
			ROLLBACK TRAN
			RAISERROR ('La Identificación ya fue ingresada para uno de los usuarios asociados a este RUC.', 16, 1)
			RETURN 0
		END

		--VALIDA CORREO REPETIDO
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
			, UsrCargo = nref.value('@Departamento','VARCHAR(5)')
			, UsrFuncion = nref.value('@Funcion','VARCHAR(5)')
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
		
		UPDATE [Seguridad].[Seg_UsuarioAdicional]
		SET
			  TipoIdent = nref.value('@TipoIdent','VARCHAR(10)')
			, Identificacion = nref.value('@Identificacion','VARCHAR(13)')
			, Apellido1 = nref.value('@Apellido1','VARCHAR(100)')
			, Apellido2 = nref.value('@Apellido2','VARCHAR(100)')
			, Nombre1 = nref.value('@Nombre1','VARCHAR(100)')
			, Nombre2 = nref.value('@Nombre2','VARCHAR(100)')
			, EstadoCivil = nref.value('@EstadoCivil','VARCHAR(10)')
			, Genero = nref.value('@Genero','VARCHAR(10)')
			, Pais = nref.value('@Pais','VARCHAR(10)')
			, Provincia = nref.value('@Provincia','VARCHAR(10)')
			, Ciudad = nref.value('@Ciudad','VARCHAR(10)')
			, Direccion = nref.value('@Direccion','VARCHAR(300)')
			, RecibeActas =  CONVERT(BIT,nref.value('@RecibeActas','VARCHAR(1)'))
		FROM [Seguridad].[Seg_UsuarioAdicional] u INNER JOIN
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

		DELETE [Seguridad].[Seg_UsuarioZona]
		FROM [Seguridad].[Seg_UsuarioZona] u INNER JOIN
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

		INSERT INTO [Seguridad].[Seg_UsuarioZona]
			(IdEmpresa, Ruc, Usuario, Zona)
		SELECT
			 nref.value('../@IdEmpresa','INT')
			,nref.value('../@Ruc','VARCHAR(13)')
			,nref.value('../@Usuario','VARCHAR(20)')
			,nref.value('@Zona','VARCHAR(12)')
		FROM @PI_DocXML.nodes('/Root/Mod/Zona') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		DELETE [Seguridad].[Seg_UsrZonaAlmacen]
		FROM [Seguridad].[Seg_UsrZonaAlmacen] u INNER JOIN
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

		INSERT INTO [Seguridad].[Seg_UsrZonaAlmacen]
			(IdEmpresa, Ruc, Usuario, Zona, Almacen)
		SELECT
			 nref.value('../@IdEmpresa','INT')
			,nref.value('../@Ruc','VARCHAR(13)')
			,nref.value('../@Usuario','VARCHAR(20)')
			,nref.value('@codCiudad','VARCHAR(12)')
			,nref.value('@codAlmacen','VARCHAR(4)')
		FROM @PI_DocXML.nodes('/Root/Mod/Alm') AS R(nref)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END


	END
	
	IF (EXISTS(SELECT TOP 1 1 FROM @PI_DocXML.nodes('/Root/Eli') AS R(nref)))
	BEGIN
		DELETE [Seguridad].[Seg_UsuarioZona]
		FROM [Seguridad].[Seg_UsuarioZona] u INNER JOIN
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

		DELETE u
		FROM [Seguridad].[Seg_UsuarioAdicional] u INNER JOIN
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


