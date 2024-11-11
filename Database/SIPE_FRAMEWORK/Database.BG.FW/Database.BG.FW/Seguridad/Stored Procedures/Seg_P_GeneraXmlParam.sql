/*
exec Seguridad.Seg_P_GeneraXmlParam 5,'SIPE_FrameWork',
	'Seguridad.Seg_P_ACTUALIZA_ROL',1

*/

CREATE PROCEDURE [Seguridad].[Seg_P_GeneraXmlParam]
@IdServidor int,
@Base varchar(50),
@NombreSP varchar(100),
@IdOrganizacion int
AS
SET NOCOUNT ON
DECLARE @posicion int, @linea nvarchar(4000), @car char, @palabra varchar(100),
        @carant char, @param varchar(50), @tipodato varchar(20), @long int,
	@pos int, @direccion varchar(10), @estado tinyint, @default bit
DECLARE @comentario tinyint, @xmlParam varchar(255)
declare @nameSP varchar(100), @Query varchar(255)

-- Retorna Siguiente IdTransaccion
SELECT Isnull(max(IdTransaccion),0) + 1  AS IdTransaccion
FROM Seguridad.Seg_Transaccion
WHERE IdOrganizacion = @IdOrganizacion
AND IdTransaccion < 1000

-- Crea una tabla temporal que tiene los elementos parametros a ser enviados
CREATE TABLE #temp_Xml
( parametro varchar(255) NULL) 
SELECT @xmlParam = '<SP nombre="' + @Base + '.' + @NombreSP + '">'
INSERT #temp_Xml VALUES(@xmlParam)
SELECT @xmlParam = ''

-- Tabla temporal que contiene las lineas de texto del SP a procesar
CREATE TABLE #temp_SP
( name varchar(60), text nvarchar(4000), colid int)

IF EXISTS(SELECT 1 FROM Seguridad.Seg_Aplicacion 
					WHERE IdAplicacion = @IdServidor AND Nombre = 'Local')
BEGIN
SELECT @Query = 'INSERT #temp_SP
		SELECT ' + char(39) + @NombreSP + char(39) + ' as name, c.text, c.colid 
		FROM ' + @Base + '..syscomments c 
        	WHERE c.id = ' + CONVERT(char,OBJECT_ID(@NombreSP)) --and c.number = 1 and c.colid = 1
END
ELSE --Es un servidor Remoto
BEGIN
SELECT @Query = 'INSERT #temp_SP
		SELECT ' + char(39) + @NombreSP + char(39) + ' as name, c.text, c.colid 
		FROM ' + @Base + '..syscomments c 
        	WHERE c.id = ' + CONVERT(char,OBJECT_ID(@NombreSP)) --and c.number = 1 and c.colid = 1
END
EXEC (@Query)

SELECT @pos = 0
SELECT @param='',@tipodato='',@long=0,@direccion='input', @default = 0
SELECT @palabra = '', @car='', @comentario = 0

--declare un cursor
DECLARE c_temp_SP  CURSOR
        FOR SELECT name, text FROM #temp_SP
		order by colid
       FOR READ ONLY

OPEN c_temp_SP

FETCH c_temp_SP into @nameSP, @linea
SELECT @posicion = 0

WHILE @@FETCH_STATUS = 0 -- SybaseProcesa cada linea del texto del SP
BEGIN

	--SELECT @linea	
	--Procesa cada linea
	-- inicializa posicion, si tiene -1 es porque encontro AS y debe salir
	IF @posicion = -1
		BREAK
	ELSE
		SELECT @posicion = 1

	-- Lee caracter por caracter de la linea
	WHILE  @posicion <= DATALENGTH(@linea)
	BEGIN
	--SELECT pos= @posicion, long=DATALENGTH(@linea), 'while'

	   SELECT @carant = @car
	   SELECT @car = SUBSTRING(@linea, @posicion, 1)
		
	   IF (ascii(@car) in (32,45,47,44,9,42,40,41,61)) --' ','-','/',',''tab','*','(',')'
	   	OR @comentario = 1
		OR ( ascii(@carant) = 13 AND ascii(@car) = 10 ) --Nueva linea
	   begin

		if ascii(@car) = 61 -- en caso de encontrar '='
		BEGIN
			SET @default = 1
		END 

		--if ascii(@carant) = 13 AND ascii(@car) = 10 --Nueva linea
		--	SELECT ascii(@car),ascii(@carant)
		-- Pregunto si la palabra es un parametro
--		SELECT @palabra as Palabra
		IF substring(@palabra,1,1) = '@' -- es parametro
		BEGIN
			SELECT @param = @palabra
		END

		-- Pregunta por el tipo de dato
		EXEC Seguridad.Seg_P_istipodato @palabra, @estado output
		IF @estado = 1 
		BEGIN
			SELECT @tipodato = lower(@palabra)
			EXEC Seguridad.Seg_P_longtipodato @palabra, @long output
			-- CONVERSION DE TIPO DE DATO A NATIVO DE SQL CUANDO RECIBA UN TYPE DE USUARIO
			-- DMUNOZ 2012-09-18
			EXEC [Seguridad].[Seg_P_ConversionTipoDato] @tipodato output
		END
		
		--EXEC bpm_isnumber @palabra, @estado output --sybase
		SELECT @estado = IsNumeric(@palabra) --SQLSERVER
		
		IF @estado = 1 AND @tipodato <> '' AND @default = 0 --Su longitud
		begin
			SELECT @long = convert(int,@palabra)
			--select @long
		end
		IF upper(@palabra) in('OUTPUT','OUT')
	 		SELECT @direccion = 'output'

		IF (@car = ',' OR upper(@palabra) = 'AS' ) and @param <> '' --Termina definicion de Parametro
		BEGIN
			-- SELECT @param,@tipodato,@long, @direccion
			SELECT @xmlParam =  
  				'<Param posicion="' + convert(varchar,@pos) + '" '
				+ ' nombre="'    +  @param + '" '
				+ ' direccion="' +  @direccion + '" '
				+ ' tipo="'  +  @tipodato + '" '
				+ ' longitud="'  +  convert(varchar,@long) + '" />' 
			-- Ingreso el registro en la tabla temporal de xml
			INSERT #temp_Xml VALUES(@xmlParam)
			
			SELECT @param='',@tipodato='',@long=0, @direccion = 'input', @default = 0
			SELECT @pos = @pos + 1  
		END
	
		-- Pregunto si se forma el comentario --
--		select '-' + @palabra + '-', 'pregunta AS'

		IF upper(@palabra) = 'AS'
		begin
--			print 'as encontrado'
			SELECT @posicion = -1 --DATALENGTH(@linea) -- para que finalice el while
--			SELECT @posicion
			break

		end
		-- Pregunto si no se forma el bloque de comentaro /*
		IF (@car = '*' and @carant = '/') OR (@car = '-' and @carant = '-')
			SELECT @comentario = 1 -- lo que sigue es comentario
		IF (@car = '/' and @carant = '*') OR (ascii(@car) = 13)
			SELECT @comentario = 0 
		SELECT @palabra = '' --encera la palabra

	   end
	   ELSE
	   BEGIN
			IF NOT ascii(@car) in (10,13,32) --nueva linea o ' '  
			    SELECT @palabra = ltrim(rtrim(@palabra)) + @car
	   END 	
	   SELECT @posicion = @posicion + 1
--		SELECT @posicion
	--SELECT @palabra
	END

	--Siguiente liea del texto del SP
	FETCH c_temp_SP into @nameSP, @linea
	

END 

SELECT @xmlParam = '</SP>'
INSERT #temp_Xml VALUES(@xmlParam)
-- Retorna Xml de parametro
SELECT parametro FROM #temp_Xml

CLOSE c_temp_SP
DEALLOCATE c_temp_SP





