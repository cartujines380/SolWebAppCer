
CREATE PROC  [Catalogo].[Ctl_P_ConCalendarioPeriodalxAño] 
	@PI_Año smallint 
AS 
--select * from Catalogo.Ctl_Semana
--PRIMER DATASET
SELECT p.Periodo, 
S1Desde = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaInicio ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 1), 
S1Hasta = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaFin ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 1),  
S2Desde = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaInicio ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 2), 
S2Hasta = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaFin ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 2),  
S3Desde = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaInicio ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 3), 
S3Hasta = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaFin ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 3),  
S4Desde = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaInicio ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 4), 
S4Hasta = (SELECT TOP 1 Catalogo.Ctl_F_Fecha( FechaFin ) 
	FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año AND p.Periodo = s.Periodo AND s.Semana = 4) 
FROM Catalogo.Ctl_Periodo p 
	WHERE p.Año = @PI_Año 


DECLARE @FirstDate as datetime, @LastDate as datetime 
SELECT @FirstDate = min( FechaInicio ) 
FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año 
SELECT @LastDate = max( FechaFin ) 
FROM Catalogo.Ctl_Semana s 
	WHERE s.Año = @PI_Año 
IF ( @FirstDate IS NULL ) 
BEGIN 
	SELECT @FirstDate = max( FechaFin ) 
	FROM Catalogo.Ctl_Semana s 
		WHERE s.Año = ( @PI_Año - 1 ) 
	IF ( @FirstDate IS NOT NULL ) 
	BEGIN 
		SELECT @FirstDate = dateadd( day, 1, @FirstDate ) 
	END 
END 

--SEGUNDO DATASET
SELECT FirstDate = @FirstDate, LastDate = @LastDate 

--TERCER DATASET
SELECT 
	t.TipoTemporada, 
	FechaInicio = Catalogo.Ctl_F_Fecha( t.FechaInicio ), 
	FechaFin = Catalogo.Ctl_F_Fecha( t.FechaFin ) 
FROM Catalogo.Ctl_Temporada t 
	WHERE t.FechaInicio BETWEEN @FirstDate AND @LastDate 
		AND t.FechaFin BETWEEN @FirstDate AND @LastDate 




