CREATE PROCEDURE [Seguridad].[Seg_p_IngresaTransaccion]
	@PV_xmlTrans                 varchar(max),
	@XmlEntrada varchar(max),
	@XmlSalida varchar(max),
	@XmlValidador varchar(max)
AS
declare @idXML int
declare @IdTrans int, @IdOrg int, @IdFormulario int

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @idXML OUTPUT, @PV_xmlTrans


--Recupera datos de la trnasaccion
SELECT  @IdOrg = IdOrganizacion, @IdTrans=IdTransaccion 
   FROM   OPENXML (@idXML, '/ResultSet/Transaccion') WITH Seguridad.Seg_Transaccion

SELECT  @IdFormulario = IdFormulario
   FROM   OPENXML (@idXML, '/ResultSet/Transaccion') WITH (IdFormulario int)

if exists(select 1 from Seguridad.Seg_Transaccion where IdTransaccion = @IdTrans and @IdOrg = IdOrganizacion)
BEGIN
	raiserror('Código de Transacción ya existe en este módulo.',16,1,'Seguridad')
	return
END
--Inicia la trnasaccion
BEGIN TRAN
--Ingresa Transaccion
insert into Seguridad.Seg_Transaccion (IdTransaccion,IdOrganizacion,Descripcion,
				Estado,PerfilContable,
				Parametros,Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,
				Menu,Monitor,XmlEntrada,XmlSalida, XmlValidador)
	SELECT @IdTrans, IdOrganizacion, Descripcion, Estado,PerfilContable , 
		Parametros, Auditable,IdServidor,NombreBase,NombreSP,IdServidorExec,
		Menu,Monitor,@XmlEntrada,@XmlSalida,@XmlValidador
FROM   OPENXML (@idXML, '/ResultSet/Transaccion') WITH Seguridad.Seg_Transaccion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Ingresa OpcionTrans
Insert into Seguridad.Seg_OpcionTrans(IdTransaccion,IdOpcion,IdOrganizacion,Descripcion,Nivel)
	SELECT @IdTrans, IdOpcion, IdOrganizacion, Descripcion, Nivel
 FROM   OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_N') WITH Seguridad.Seg_OpcionTrans
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Relaciona al Formulario si exite Idformulario
/*	UPDATE Seguridad.Seg_Formulario
	SET IdTransaccion = null,IdOrganizacion = null
	FROM Seguridad.Seg_Formulario
	WHERE IdTransaccion = @IdTrans AND IdOrganizacion = @IdOrg

IF @IdFormulario is not null
BEGIN
	UPDATE Seguridad.Seg_Formulario
	SET IdTransaccion = @IdTrans,IdOrganizacion = @IdOrg
	FROM Seguridad.Seg_Formulario
	WHERE IdFormulario = @IdFormulario
END
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
*/

-- Asigan la transaccion al Rol 1 por default
INSERT INTO Seguridad.Seg_OpcionTransRol(IdRol,IdTransaccion,IdOpcion,IdOrganizacion)
SELECT 1, @IdTrans,IdOpcion, IdOrganizacion
 FROM   OPENXML (@idXML, '/ResultSet/OpcionTrans/Opcion_N') WITH Seguridad.Seg_OpcionTrans
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Ingresa Horario OpcionTrans
Insert into Seguridad.Seg_HorarioTrans(IdTransaccion, IdOpcion, IdOrganizacion, IdHorario)
	SELECT @IdTrans, IdOpcion, IdOrganizacion, IdHorario
 FROM   OPENXML (@idXML, '/ResultSet/OpcionHorario/Horario_N') WITH Seguridad.Seg_HorarioTrans
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Ingresa Autorizacion
Insert into Seguridad.Seg_Autorizacion(IdTransaccion, IdOrganizacion, IdOpcion, IdHorario, Parametro, Operador, ValorAutorizado)
	SELECT @IdTrans, IdOrganizacion, IdOpcion, IdHorario, Parametro, Operador, ValorAutorizado
 FROM   OPENXML (@idXML, '/ResultSet/OpcionAutorizacion/Autorizacion_N') WITH Seguridad.Seg_Autorizacion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END


  COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @idXML








