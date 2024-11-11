
CREATE PROC [Catalogo].[Ctl_P_IngCalendarioPeriodalxAño]
	@PI_xmlDoc varchar(8000)
AS
declare @VL_idXML int, @VL_Año smallint, @VL_Tipo char(3) 
--Preparo el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_xmlDoc
--Obtengo los datos necesarios
SELECT @VL_Año = Año, @VL_Tipo = Tipo 
	FROM OPENXML (@VL_idXML, '/Usuario') 
		WITH ( Año smallint, Tipo char(3) ) 
--Inicio la transaccion
BEGIN TRAN
IF (@VL_Tipo = 'CAL')
BEGIN
		--Ingreso los Periodos
		INSERT INTO Catalogo.Ctl_Periodo ( Periodo, Año ) 
			SELECT distinct( Periodo ), @VL_Año 
			FROM OPENXML (@VL_idXML, '/Usuario/Semana') 
				WITH ( Periodo tinyint, Semana tinyint, FInicio datetime, FFin datetime ) 
		IF (@@error <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END 
		--Ingreso las Semanas
		INSERT INTO Catalogo.Ctl_Semana 
			(Año, Periodo, Semana, FechaInicio, FechaFin) 
			SELECT @VL_Año, Periodo, Semana, FInicio, FFin 
			FROM OPENXML (@VL_idXML, '/Usuario/Semana') 
				WITH ( Periodo tinyint, Semana tinyint, FInicio datetime, FFin datetime ) 
		IF (@@error <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END
END
IF (@VL_Tipo = 'TEM')
BEGIN
		--Elimino las anteriores
		DELETE Catalogo.Ctl_Temporada 
			FROM Catalogo.Ctl_Temporada t,
				OPENXML (@VL_idXML, '/Usuario/TemporadaDel')
					WITH ( TipoTemporada char(1), FechaInicio datetime, FechaFin datetime ) x
			WHERE t.TipoTemporada = x.TipoTemporada 
				AND t.FechaInicio = x.FechaInicio AND t.FechaFin = x.FechaFin 
		IF (@@error <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END
		--Ingreso las Temporadas Nuevas
		INSERT INTO Catalogo.Ctl_Temporada
			(TipoTemporada,FechaInicio,FechaFin)
			SELECT TipoTemporada, FechaInicio, FechaFin
			FROM OPENXML (@VL_idXML, '/Usuario/TemporadaNew')
				WITH ( TipoTemporada char(1), FechaInicio datetime, FechaFin datetime )
		IF (@@error <> 0)
		BEGIN
			ROLLBACK TRAN
			RETURN
		END
END
--Finalizo la transaccion
COMMIT TRAN
--Libero el documento XML
EXEC sp_xml_removedocument @VL_idXML





