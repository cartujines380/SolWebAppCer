CREATE PROCEDURE [Seguridad].[Seg_p_ActualizaTransaccion]
	@PV_xmlTrans                 varchar(max),
	@XmlEntrada varchar(max),
	@XmlSalida varchar(max),
	@XmlValidador varchar(max)
AS
declare @idXML int, @VL_IdTransaccion int,@IdFormulario int



DECLARE @Idtran int, @IdOpcion int, @IdOrg int, @Desc varchar(100), 
	@Nivel int, @IdHorario int,
	@IdAutorizacion int, @Parametro varchar(100), @Operador varchar(50), 
	@Valor varchar(50)

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @idXML OUTPUT, @PV_xmlTrans
--Recupero Idtransaccion
SELECT @VL_IdTransaccion = IdTransaccion, @IdOrg = IdOrganizacion
FROM OPENXML (@idXML, '/ResultSet/Transaccion') WITH Seguridad.Seg_Transaccion
SELECT  @IdFormulario = IdFormulario
   FROM   OPENXML (@idXML, '/ResultSet/Transaccion') WITH (IdFormulario int)

--Inicia la trnasaccion
BEGIN TRAN

--Actaliza Transaccion
UPDATE Seguridad.Seg_Transaccion
SET  Descripcion = xt.Descripcion
, Estado = xt.Estado
, PerfilContable = xt.PerfilContable
, Parametros = xt.Parametros
, Auditable = xt.Auditable
, IdServidor = xt.IdServidor
, NombreBase = xt.NombreBase
, NombreSP = xt.NombreSP
, IdServidorExec = xt.IdServidorExec
, Menu = xt.Menu
, Monitor = xt.Monitor
, XmlEntrada = @XmlEntrada
, XmlSalida = @XmlSalida
, XmlValidador = @XmlValidador
FROM Seguridad.Seg_Transaccion t, OPENXML (@idXML, '/ResultSet/Transaccion') WITH Seguridad.Seg_Transaccion xt
WHERE t.IdTransaccion = xt.IdTransaccion AND 
	t.IdOrganizacion = xt.IdOrganizacion
    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END

--Relaciona al Formulario si exite Idformulario
/*	UPDATE Seguridad.Seg_Formulario
	SET IdTransaccion = null,IdOrganizacion = null
	FROM Seguridad.Seg_Formulario
	WHERE IdTransaccion = @VL_IdTransaccion AND IdOrganizacion = @IdOrg

IF @IdFormulario is not null
BEGIN
	UPDATE Seguridad.Seg_Formulario
	SET IdTransaccion = @VL_IdTransaccion,IdOrganizacion = @IdOrg
	FROM Seguridad.Seg_Formulario
	WHERE IdFormulario = @IdFormulario
END
*/

--Ingresa OpcionTrans si no existe, si existe actualiza
	INSERT Seguridad.Seg_OpcionTrans (IdTransaccion, IdOrganizacion, IdOpcion, Descripcion, Nivel)
	SELECT IdTransaccion, IdOrganizacion, IdOpcion, Descripcion, Nivel
	FROM OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_N') WITH Seguridad.Seg_OpcionTrans
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	-- Asigan la transaccion al Rol 1 por default de la opcion nueva
	INSERT INTO Seguridad.Seg_OpcionTransRol(IdRol,IdTransaccion,IdOpcion,IdOrganizacion)
	SELECT 1, IdTransaccion,IdOpcion, IdOrganizacion
	 FROM   OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_N') WITH Seguridad.Seg_OpcionTrans
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END

	UPDATE Seguridad.Seg_OpcionTrans
	SET Descripcion = ox.Descripcion, Nivel = ox.Nivel
	FROM Seguridad.Seg_OpcionTrans o, OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_M') WITH Seguridad.Seg_OpcionTrans ox
	WHERE o.IdTransaccion = ox.IdTransaccion 
		AND o.IdOrganizacion = ox.IdOrganizacion
		AND o.IdOpcion = ox.IdOpcion 
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	DELETE Seguridad.Seg_HorarioTrans
	FROM Seguridad.Seg_HorarioTrans o, OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_E') WITH Seguridad.Seg_HorarioTrans ox
	WHERE o.IdTransaccion = ox.IdTransaccion 
		AND o.IdOrganizacion = ox.IdOrganizacion
		AND o.IdOpcion = ox.IdOpcion 
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END
	DELETE Seguridad.Seg_OpcionTrans
	FROM Seguridad.Seg_OpcionTrans o, OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_E') WITH Seguridad.Seg_OpcionTrans ox
	WHERE o.IdTransaccion = ox.IdTransaccion 
		AND o.IdOrganizacion = ox.IdOrganizacion
		AND o.IdOpcion = ox.IdOpcion 
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

--Elimina la opcion del rol 1 asignado
	/*DELETE Seguridad.Seg_OpcionTransRol
	FROM Seguridad.Seg_OpcionTransRol o, OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_E') WITH Seguridad.Seg_OpcionTrans ox
	WHERE   o.IdTransaccion = ox.IdTransaccion 
		AND o.IdOrganizacion = ox.IdOrganizacion
		AND o.IdOpcion = ox.IdOpcion 
		AND o.IdRol = 1
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END
*/

--Ingresa Horarios de Transaccions
	INSERT Seguridad.Seg_HorarioTrans(IdTransaccion, IdOrganizacion, IdOpcion, IdHorario)
	SELECT IdTransaccion, IdOrganizacion, IdOpcion, IdHorario
 	FROM   OPENXML (@idXML, '/ResultSet/OpcionHorario/Horario_N') WITH Seguridad.Seg_HorarioTrans
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	DELETE Seguridad.Seg_HorarioTrans
	FROM Seguridad.Seg_HorarioTrans o, OPENXML (@idXML, '/ResultSet/OpcionHorario/Horario_E') WITH Seguridad.Seg_HorarioTrans ox
	WHERE o.IdTransaccion = ox.IdTransaccion 
		AND o.IdOrganizacion = ox.IdOrganizacion
		AND o.IdOpcion = ox.IdOpcion 
		AND o.IdHorario = ox.IdHorario
	IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	--Ingresa Autorizacion de Transaccions
    INSERT Seguridad.Seg_Autorizacion (IdTransaccion,IdOrganizacion, IdOpcion, 
				IdHorario, Parametro, Operador, ValorAutorizado)
	SELECT IdTransaccion, IdOrganizacion, IdOpcion, 
		IdHorario, Parametro, Operador, ValorAutorizado
 	FROM   OPENXML (@idXML, '/ResultSet/OpcionAutorizacion/Autorizacion_N') WITH Seguridad.Seg_Autorizacion
	    IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	UPDATE Seguridad.Seg_Autorizacion
	SET  Parametro = ax.Parametro, Operador = ax.Operador, ValorAutorizado = ax.ValorAutorizado
	FROM Seguridad.Seg_Autorizacion a,OPENXML (@idXML, '/ResultSet/OpcionAutorizacion/Autorizacion_M') 
		WITH (Parametro varchar(100), Operador varchar(50), ValorAutorizado varchar(50),
			IdAutorizacion int, IdTransaccion int, IdOrganizacion int, IdOpcion int, IdHorario int)  ax
	WHERE a.IdAutorizacion = ax.IdAutorizacion		   
		AND	a.IdTransaccion = ax.IdTransaccion 
		AND a.IdOrganizacion = ax.IdOrganizacion 
		AND a.IdOpcion = ax.IdOpcion 
		AND a.IdHorario = ax.IdHorario
	    IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

	DELETE Seguridad.Seg_Autorizacion
	FROM Seguridad.Seg_Autorizacion a,OPENXML (@idXML, '/ResultSet/OpcionAutorizacion/Autorizacion_E')
		WITH (IdAutorizacion int, IdTransaccion int, IdOrganizacion int, IdOpcion int, IdHorario int) ax
	WHERE a.IdAutorizacion = ax.IdAutorizacion		    
		AND a.IdTransaccion = ax.IdTransaccion 
		AND a.IdOrganizacion = ax.IdOrganizacion 
		AND a.IdOpcion = ax.IdOpcion 
		AND a.IdHorario = ax.IdHorario
	    IF (@@error <> 0)
	    BEGIN  
		ROLLBACK TRAN
		RETURN
	    END

COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @idXML



