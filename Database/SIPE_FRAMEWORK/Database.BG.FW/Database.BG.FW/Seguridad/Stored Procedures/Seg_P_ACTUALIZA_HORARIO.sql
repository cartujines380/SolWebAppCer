

CREATE procedure [Seguridad].[Seg_P_ACTUALIZA_HORARIO]
@PV_xmlHorario                 varchar(8000)
AS
declare @VL_idXML int
DECLARE @ln_idHorario int, @VL_Descripcion varchar(100), @VL_DiasFeriados bit
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlHorario
--Recupero IdHorario del horario
SELECT @ln_idHorario = IdHorario, @VL_Descripcion = Descripcion,
		@VL_DiasFeriados = DiasFeriados
FROM OPENXML (@VL_idXML, '/ResultSet/Horario',1) WITH
		 (IdHorario int,
		  Descripcion varchar(100),
		  DiasFeriados bit )

-- Verifica que el horario este definido en seguridad
IF not EXISTS(SELECT 1 FROM Seguridad.Seg_HORARIO WHERE IdHorario = @ln_idHorario)
BEGIN
	RAISERROR(50002,16,1,'Horario')
	RETURN
END 

--Inicia la trnasaccion
BEGIN TRAN
--Actualiza Horario
  Update Seguridad.Seg_HORARIO 
  SET descripcion = @VL_Descripcion, DiasFeriados = @VL_DiasFeriados
   WHERE IdHorario  = @ln_idHorario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
--Actualiza Dia del horario
UPDATE Seguridad.Seg_HorarioDia
  SET Dias = x.DIas,HoraInicio = x.HoraInicio,HoraFin = x.HoraFin
  FROM Seguridad.Seg_HorarioDia hd, OPENXML (@VL_idXML, '/ResultSet/Dias/Dia_M') WITH Seguridad.Seg_HorarioDia x
  WHERE hd.IdHorarioDia = x.IdHorarioDia and hd.IdHorario = x.IdHorario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
-- Ingresa las nuevas horarios Dias
Insert into Seguridad.Seg_HorarioDia(IdHorarioDia,IdHorario, Dias,HoraInicio,HoraFin)
	SELECT IdHorarioDia,IdHorario, Dias,HoraInicio,HoraFin 
		FROM OPENXML (@VL_idXML, '/ResultSet/Dias/Dia_N') WITH Seguridad.Seg_HorarioDia x
		WHERE not Exists(SELECT 1 FROM Seguridad.Seg_HorarioDia hd
				  WHERE hd.IdHorario = x.IdHorario and
					hd.IdHorarioDia = x.IdHorarioDia)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

-- Elimina las que no existen en el xml
DELETE Seguridad.Seg_HorarioDia 
WHERE Exists (SELECT 1 FROM OPENXML (@VL_idXML, '/ResultSet/Dias/Dia_E') WITH Seguridad.Seg_HorarioDia x
		   WHERE Seguridad.Seg_HorarioDia.IdHorario = x.IdHorario and
			 Seguridad.Seg_HorarioDia.IdHorarioDia = x.IdHorarioDia )
	AND Seguridad.Seg_HorarioDia.IdHorario = @ln_idHorario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML






