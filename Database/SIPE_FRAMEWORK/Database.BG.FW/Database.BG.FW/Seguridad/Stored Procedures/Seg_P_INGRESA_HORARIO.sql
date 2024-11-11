

CREATE procedure [Seguridad].[Seg_P_INGRESA_HORARIO]
@PV_xmlHorario                 varchar(8000)
AS
declare @VL_idXML int, @VL_DescHorario varchar(20), @VL_DiasFeriados bit
DECLARE @ln_idHorario    int
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlHorario
--Recupero Descripcion del horario
SELECT @VL_DescHorario = Descripcion,
		@VL_DiasFeriados = DiasFeriados
FROM OPENXML (@VL_idXML, '/ResultSet/Horario',1) WITH
		 (Descripcion varchar(100),DiasFeriados bit )

-- Verifica que el horario este definido en seguridad
IF EXISTS(SELECT 1 FROM Seguridad.Seg_HORARIO WHERE Descripcion = @VL_DescHorario)
BEGIN
	RAISERROR(50003,16,1,'Horario')
	RETURN
END 

--Inicia la trnasaccion
BEGIN TRAN
   Select  @ln_idHorario = ISNULL(max(idhorario),0) + 1
        From Seguridad.Seg_HORARIO 
--Ingresa Usuario
  INSERT INTO Seguridad.Seg_HORARIO(idhorario,descripcion,DiasFeriados) 
  VALUES(@ln_idHorario,@VL_DescHorario,@VL_DiasFeriados)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
--Ingresa Dia del horario
Insert into Seguridad.Seg_HorarioDia(IdHorarioDia,IdHorario, Dias,HoraInicio,HoraFin)
	SELECT IdHorarioDIa,@ln_idHorario, Dias,HoraInicio,HoraFin 
		FROM OPENXML (@VL_idXML, '/ResultSet/Dias/Dia_N') WITH Seguridad.Seg_HorarioDia
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
COMMIT TRAN
SELECT IdHorario = @ln_idHorario

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML






