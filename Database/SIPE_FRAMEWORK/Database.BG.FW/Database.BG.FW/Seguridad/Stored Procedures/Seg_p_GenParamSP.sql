
CREATE procedure [Seguridad].[Seg_p_GenParamSP] --'Sige_Seguridad','Seguridad.Seg_P_ActualizaEquipo',1
@Base varchar(50),
@NombreSP varchar(100),
@IdOrganizacion int
AS
      
declare @SyscomText	varchar(4000)
,@LFCR          int
,@LineId        int
,@BasePos       int
,@CurrentPos    int
,@TextLength    int
,@BlankSpaceAdded   int
,@AddOnLen      int
,@DefinedLength int
,@Line          varchar(255)
,@namesp	varchar(100)
,@NumTrans	int
,@NumPos	int
,@Pos		int
,@LineTemp	varchar(255)

SELECT @NumTrans =  Isnull(max(IdTransaccion),0) + 1 
FROM Seguridad.Seg_Transaccion
WHERE IdOrganizacion = @IdOrganizacion
AND IdTransaccion < 1000

SELECT @NumTrans  AS IdTransaccion

Select @DefinedLength = 255
SELECT @BlankSpaceAdded = 0 /*Keeps track of blank spaces at end of lines. Note Len function ignores
                             trailing blank spaces*/
--elimina registros de tabla temporal

CREATE TABLE #Seguridad.Seg_SPText
(LineId	int ,namesp varchar(100),
Texto  varchar(255))

 DECLARE ms_crs_syscom  CURSOR LOCAL
        FOR SELECT o.name, c.text 
	FROM sysobjects o,syscomments c 
        WHERE o.type = 'P' and o.name like @NombreSP + '%' 
		and o.id = c.id and encrypted = 0
                and c.number = 1 and c.colid = 1
		order by o.name
        FOR READ ONLY
 
SELECT @LFCR = 2
SELECT @LineId = 1


OPEN ms_crs_syscom

FETCH NEXT FROM ms_crs_syscom into @namesp, @SyscomText

WHILE @@fetch_status >= 0
BEGIN
    SET @NumPos = 0
    SELECT  @BasePos    = 1
    SELECT  @CurrentPos = 1
    SELECT  @TextLength = LEN(@SyscomText)

    WHILE @CurrentPos  != 0
    BEGIN
        --Looking for end of line followed by carriage return
        SELECT @CurrentPos =   CHARINDEX(char(13)+char(10), @SyscomText, @BasePos)

        --If carriage return found
        IF @CurrentPos != 0
        BEGIN
            /*If new value for @Lines length will be > then the
            **set length then insert current contents of @line
            **and proceed.
            */
            While (isnull(LEN(@Line),0) + @BlankSpaceAdded + @CurrentPos-@BasePos + @LFCR) > @DefinedLength
            BEGIN
                SELECT @AddOnLen = @DefinedLength-(isnull(LEN(@Line),0) + @BlankSpaceAdded)
                INSERT #Seguridad.Seg_SPText VALUES
                ( @LineId,@namesp,
                  isnull(@Line, N'') + isnull(SUBSTRING(@SyscomText, @BasePos, @AddOnLen), N''))
                SELECT @Line = NULL, @LineId = @LineId + 1,
                       @BasePos = @BasePos + @AddOnLen, @BlankSpaceAdded = 0
            END
            SELECT @Line    = isnull(@Line, N'') + isnull(SUBSTRING(@SyscomText, @BasePos, @CurrentPos-@BasePos + @LFCR), N'')
            SELECT @BasePos = @CurrentPos+2
	    
	    if upper(ltrim(rtrim(replace(@line,char(13)+char(10),'')))) = 'AS' --+ char(13)+char(10)
	    BEGIN
		set @Line = '</SP>' -- + char(39) + ',null)'
		INSERT #Seguridad.Seg_SPText VALUES( @LineId,@namesp, ltrim(@Line) )
		select @Line = null
		break
	    end 
	    if @Line <> char(13)+char(10)
	    begin --reemplazo de datos
		set @LineTemp = @Line
		if upper(@LineTemp) like '%CREATE%PROC%'
		begin
		  
 		   SET @LineTemp = 
			-- 'insert Seguridad.Seg_transaccion values(' +
                        --      convert(varchar,@NumTrans) + ',' 
                        --      + convert(varchar,@IdOrganizacion) + ',' 
			--	+ char(39) + @namesp + char(39) 
    			-- + ',' + char(39) + 'A' + char(39) 
                        -- + ',null,' + char(39) 
			 '<SP nombre="' + @Base + '..' + @namesp + '">' 
       		  INSERT #Seguridad.Seg_SPText VALUES( @LineId,@namesp, ltrim(@LineTemp) )
		   SET @NumTrans = @NumTrans + 1
		end 

		if upper(ltrim(@Line)) like '%@%'
		begin
		   set @Line = ltrim(@Line)
		   set @Pos = charindex('@',@Line,1)
		   set @Line = substring(@Line,@Pos,len(@Line) - @Pos)
		   set @Pos = charindex(char(9),ltrim(@Line),1)
		   if @Pos = 0
			set @Pos = charindex(' ',ltrim(@Line),1)
                   if @Pos <> 0
		      set @Line = left(ltrim(@Line),@Pos-1) + '"' +
				substring(@Line,@Pos,len(@line)-@Pos)
		   set @Line = replace(@Line,' CHAR',' tipo="char" longitud="1" ')
		   set @Line = replace(@Line,char(9) + 'CHAR',' tipo="char" longitud="1" ')
		   set @Line = replace(@Line,'FLOAT',' tipo="float" longitud="8" ')
		   set @Line = replace(@Line,'VARCHAR',' tipo="varchar" longitud="4" ')
		   set @Line = replace(@Line,'INT',' tipo="int" longitud="4" ')	
		   set @Line = replace(@Line,'DATETIME',' tipo="datetime" longitud="8" ')
		   set @Line = replace(@Line,'"4" (','"')
		   set @Line = replace(@Line,'"4" ,','"4"')
		   set @Line = replace(@Line,'"8" ,','"8"')
		   set @Line = replace(@Line,'),','"')
		   set @Line = replace(@Line,')','"')
		   SET @Line = '<Param nombre="' + ltrim(@Line) + 
   			' posicion="' + convert(varchar,@NumPos) + '" direccion="input" />' 
		   SET @NumPos = @NumPos + 1
       		   INSERT #Seguridad.Seg_SPText VALUES( @LineId,@namesp, ltrim(@Line) )
		end

	    end 
            SELECT @LineId = @LineId + 1
            SELECT @Line = NULL
        END
        ELSE
        --else carriage return not found
        BEGIN
            IF @BasePos <= @TextLength
            BEGIN
                /*If new value for @Lines length will be > then the
                **defined length
                */
                While (isnull(LEN(@Line),0) + @BlankSpaceAdded + @TextLength-@BasePos+1 ) > @DefinedLength
                BEGIN
                    SELECT @AddOnLen = @DefinedLength - (isnull(LEN(@Line),0)  + @BlankSpaceAdded )
                    INSERT #Seguridad.Seg_SPText VALUES
                    ( @LineId,@namesp,
                      isnull(@Line, N'') + isnull(SUBSTRING(@SyscomText, @BasePos, @AddOnLen), N''))
                    SELECT @Line = NULL, @LineId = @LineId + 1,
                        @BasePos = @BasePos + @AddOnLen, @BlankSpaceAdded = 0
                END
                SELECT @Line = isnull(@Line, N'') + isnull(SUBSTRING(@SyscomText, @BasePos, @TextLength-@BasePos+1 ), N'')
                if charindex(' ', @SyscomText, @TextLength+1 ) > 0
                BEGIN
                    SELECT @Line = @Line + ' ', @BlankSpaceAdded = 1
                END
                BREAK
            END
        END
    END

	FETCH NEXT FROM ms_crs_syscom into @namesp,@SyscomText
END

IF @Line is NOT NULL
    INSERT #Seguridad.Seg_SPText VALUES( @LineId,@namesp, @Line )

--select text from #Seguridad.Seg_SPText order by LineId

CLOSE  ms_crs_syscom
DEALLOCATE 	ms_crs_syscom

--RETORNA PARAMETROS
SELECT texto FROM #Seguridad.Seg_SPText

--Elimina tabla temporal
drop table  #Seguridad.Seg_SPText






