
CREATE PROC [Participante].[Par_P_ModParticipanteContactoAFS]
	@PI_docXML varchar(max)
AS

declare @VL_idXML int, @VL_IdParticipante int, @VL_IdTipoIdentificacion varchar(10),
		@VL_Identificacion varchar(100), @VL_TipoParticipante char(1),
		@VL_IdDireccion int, @VL_IdTipoMedioContacto varchar(10),
		@VL_IdMedioContacto int, @VL_ValorMC varchar(100),
		@VL_IdParametro int, @VL_IdTipoParametro varchar(10),
		@VL_ValorP varchar(100), @VL_IdParametroTTarj int,
		@VL_IdTipoParametroTTarj varchar(10), @VL_ValorPTTarj varchar(100)

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo Valores a modificar
Select
		@VL_IdParticipante = IdParticipante,
		@VL_IdTipoIdentificacion = IdTipoIdentificacion,
		@VL_Identificacion = Identificacion,
		@VL_TipoParticipante = TipoParticipante,
		@VL_IdDireccion = IdDireccion,
		@VL_IdMedioContacto = IdMedioContacto,
		@VL_IdTipoMedioContacto = IdTipoMedioContacto,
		@VL_ValorMC = ValorMC,
		@VL_IdParametro = IdParametro,
		@VL_IdTipoParametro = IdTipoParametro,
		@VL_ValorP = ValorP,
		@VL_IdParametroTTarj = IdParametroTTarj,
		@VL_IdTipoParametroTTarj = IdTipoParametroTTarj,
		@VL_ValorPTTarj = ValorPTTarj
FROM OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH
			(IdParticipante int, IdTipoIdentificacion Varchar(10),Identificacion varchar(100),
			TipoParticipante char(1), IdDireccion int, IdMedioContacto int,
			IdTipoMedioContacto varchar(10), ValorMC varchar(100), IdParametro int,
			IdTipoParametro varchar(10), ValorP varchar(100), IdParametroTTarj int,
			IdTipoParametroTTarj varchar(10), ValorPTTarj varchar(100))
--Inicia la transaccion
BEGIN TRAN

--Actualizo Participante
UPDATE Participante.Par_Participante
	SET
		IdTipoIdentificacion = @VL_IdTipoIdentificacion, Identificacion = @VL_Identificacion
	WHERE IdParticipante = @VL_IdParticipante
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END

--Actualizo Persona
UPDATE Participante.Par_Persona
	SET
		Apellido1 = px.Apellido1,
		Apellido2 = px.Apellido2,
		Nombre1 = px.Nombre1,
		Nombre2 = px.Nombre2
FROM Participante.Par_Persona p, OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_Persona px
	WHERE p.IdParticipante = px.IdParticipante AND p.IdParticipante = @VL_IdParticipante
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END

--Actualizo/Ingreso Medio Contacto
IF (EXISTS (SELECT 1 FROM Participante.Par_MedioContacto
			WHERE IdParticipante = @VL_IdParticipante
				AND IdDireccion = @VL_IdDireccion
				AND IdMedioContacto = @VL_IdMedioContacto))
BEGIN
	UPDATE Participante.Par_MedioContacto
		SET
			IdTipoMedioContacto = @VL_IdTipoMedioContacto,
			Valor = @VL_ValorMC
		WHERE IdParticipante = @VL_IdParticipante
			AND IdDireccion = @VL_IdDireccion
			AND IdMedioContacto = @VL_IdMedioContacto
END
	ELSE
BEGIN
	IF (NOT EXISTS (SELECT 1 FROM Participante.Par_Direccion
					WHERE IdParticipante = @VL_IdParticipante
						AND IdDireccion = @VL_IdDireccion))
	BEGIN
		INSERT INTO Participante.Par_Direccion (IdParticipante, IdDireccion, Direccion)
			VALUES (@VL_IdParticipante, @VL_IdDireccion, '')
		IF (@@error <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END
	END
	INSERT INTO Participante.Par_MedioContacto
		(IdParticipante, IdDireccion, IdMedioContacto, IdTipoMedioContacto, Valor)
		VALUES
		(@VL_IdParticipante, @VL_IdDireccion, @VL_IdMedioContacto, @VL_IdTipoMedioContacto, @VL_ValorMC)
END
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END

--Actualizo/Ingreso Parametro 1
IF (EXISTS (SELECT 1 FROM Participante.Par_ParametroParticipante
			WHERE IdParticipante = @VL_IdParticipante
				AND IdParametro = @VL_IdParametro))
BEGIN
	UPDATE Participante.Par_ParametroParticipante
		SET
			IdTipoParametro = @VL_IdTipoParametro,
			Valor = @VL_ValorP
		WHERE IdParticipante = @VL_IdParticipante
			AND IdParametro = @VL_IdParametro
END
ELSE
BEGIN
	INSERT INTO Participante.Par_ParametroParticipante
		(IdParticipante, IdParametro, IdTipoParametro, Valor)
		VALUES
		(@VL_IdParticipante, @VL_IdParametro, @VL_IdTipoParametro, @VL_ValorP)
END
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END

--Actualizo/Ingreso Parametro 2
IF (EXISTS (SELECT 1 FROM Participante.Par_ParametroParticipante
			WHERE IdParticipante = @VL_IdParticipante
				AND IdParametro = @VL_IdParametroTTarj))
BEGIN
	UPDATE Participante.Par_ParametroParticipante
		SET
			IdTipoParametro = @VL_IdTipoParametroTTarj,
			Valor = @VL_ValorPTTarj
		WHERE IdParticipante = @VL_IdParticipante
			AND IdParametro = @VL_IdParametroTTarj
END
ELSE
BEGIN
	INSERT INTO Participante.Par_ParametroParticipante
		(IdParticipante, IdParametro, IdTipoParametro, Valor)
		VALUES
		(@VL_IdParticipante, @VL_IdParametroTTarj, @VL_IdTipoParametroTTarj, @VL_ValorPTTarj)
END
IF (@@error <> 0)
BEGIN
	ROLLBACK TRAN
	RETURN
END

--Finaliza la transaccion
COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML





