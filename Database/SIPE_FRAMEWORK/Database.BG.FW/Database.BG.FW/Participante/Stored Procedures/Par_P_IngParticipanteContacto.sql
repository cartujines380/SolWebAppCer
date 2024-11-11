CREATE PROCEDURE [Participante].[Par_P_IngParticipanteContacto]
@PI_docXML as varchar(3000),
@PO_IdParticipante int output
AS
declare @VL_idXML int, @VL_IdTipoIdentificacion varchar(10)
declare @VL_Identificacion varchar(100), @VL_TipoParticipante char, @VL_IdUsuario varchar(50)
declare @VL_IdDireccion int, @VL_IdMedioContacto int 

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo IdParticipante
Select	@VL_IdTipoIdentificacion = IdTipoIdentificacion,
		@VL_Identificacion = Identificacion,
		@VL_TipoParticipante = TipoParticipante,
		@VL_IdUsuario = IdUsuario	
FROM OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH
			(IdTipoIdentificacion Varchar(10),Identificacion varchar(100),
			TipoParticipante char(1), IdUsuario varchar(50))

Select	@VL_IdDireccion = isnull(IdDireccion,0),
		@VL_IdMedioContacto = isnull(IdMedioContacto,0)
FROM OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_MedioContacto

-- Valida que Identificacion de participante No exista 
if @VL_Identificacion <>''
begin
	if exists( SELECT 1 FROM Participante.Par_Participante WHERE Identificacion = @VL_Identificacion
			and IdTipoIdentificacion = @VL_IdTipoIdentificacion)
	BEGIN	
		raiserror (52001,16,1,@VL_Identificacion)	
		return
	END	
end
--verifica si usuario existe
if exists( SELECT 1 FROM Participante.Par_Participante WHERE IdUsuario = @VL_IdUsuario)
BEGIN
	raiserror (52000,16,1,@VL_IdUsuario)	
	return
END

--Inicia la transaccion
BEGIN TRAN

	INSERT INTO Participante.Par_Participante(IdTipoIdentificacion,Identificacion,
		TipoParticipante,IdUsuario,Estado,FechaRegistro,Opident)
	SELECT IdTipoIdentificacion, Identificacion, TipoParticipante,
		@VL_IdUsuario,Estado, getdate(),@VL_IdUsuario
	FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_participante
	IF (@@error <> 0)
	  BEGIN
	  ROLLBACK TRAN
	  RETURN
	END		
	--Recupeta IdParticipante
	SET @PO_IdParticipante = @@IDENTITY

	insert into Participante.Par_Persona(IdParticipante,Apellido1,Apellido2,Nombre1,
		Nombre2,IdTitulo,Sexo,FechaNacimiento,EstadoCivil, Ruc)
	SELECT @PO_IdParticipante, Apellido1,Apellido2,Nombre1,
		Nombre2,IdTitulo,Sexo,FechaNacimiento,EstadoCivil, Ruc
	FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_Persona
	
	
IF (@@error <> 0)
  BEGIN
  ROLLBACK TRAN
  RETURN
END
	if (@VL_IdDireccion > 0)
	   begin
		INSERT INTO Participante.Par_Direccion(IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
			IdPais,IdProvincia,IdCiudad,IdParroquia,IdBarrio,HorarioContacto,NombreContacto)
		SELECT @PO_IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
			IdPais,IdProvincia,IdCiudad,IdParroquia,IdBarrio,HorarioContacto,NombreContacto
 		FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_Direccion

		IF (@@error <> 0)
		  BEGIN
		     ROLLBACK TRAN
		     RETURN
		  END
	   end
	if (@VL_IdDireccion > 0 and @VL_IdMedioContacto > 0) 
	   begin
		Insert into Participante.Par_MedioContacto(IdParticipante,IdDireccion,IdMedioContacto,
			IdTipoMedioContacto,Valor)
		SELECT @PO_IdParticipante,IdDireccion,IdMedioContacto,
			IdTipoMedioContacto,Valor
		FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto') WITH Participante.Par_MedioContacto		  
		IF (@@error <> 0)
		   BEGIN
		      ROLLBACK TRAN
		      RETURN
		   END
	   end


COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML










