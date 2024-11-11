

CREATE procedure [Seguridad].[Seg_p_ActualizaUsuarioTran]
	@PV_xmlUsuario                 varchar(max)
AS
declare @VL_idXML int, @VL_IdUsuario varchar(20)
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlUsuario

--Recupero IdUsuario
SELECT @VL_IdUsuario = IdUsuario
FROM OPENXML (@VL_idXML, '/ResultSet',1) WITH
		 (IdUsuario varchar(20) )

--Inicia la trnasaccion
BEGIN TRAN

/*-- Verifica que el usuario este definido en partic, si no lo ingresa
IF not EXISTS(SELECT 1 FROM Seguridad.Seg_Usuario WHERE idUsuario = @VL_IdUsuario)
BEGIN
	 --Ingresa Usuario en base seguridad
	insert into Seguridad.Seg_usuario(IdUsuario) Values(@VL_IdUsuario)
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END 
*/

-- Ingresa RolUsuario Nuevas
insert into Seguridad.Seg_RolUsuario(IdUsuario,IdRol,IdHorario,Estado,
	FechaInicial,FechaFinal,
	TipoIdentificacion,IdIdentificacion,UsrReemplazo)
	SELECT IdUsuario,IdRol,IdHorario,Estado,
	FechaInicial,FechaFinal,
	TipoIdentificacion,IdIdentificacion,UsrReemplazo
FROM   OPENXML (@VL_idXML, '/ResultSet/Roles/Rol') WITH Seguridad.Seg_RolUsuario cx
  WHERE not exists (Select IdUsuario
		    FROM Seguridad.Seg_RolUsuario c
			WHERE c.IdUsuario = cx.IdUsuario
				AND c.IdRol = cx.IdRol)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Actualiza las existentes
UPDATE Seguridad.Seg_RolUsuario
SET 	IdHorario = xe.IdHorario,
	Estado = xe.Estado,
	FechaInicial = xe.FechaInicial,FechaFinal = xe.FechaFinal,
	TipoIdentificacion = xe.TipoIdentificacion,
	IdIdentificacion = xe.IdIdentificacion,
	UsrReemplazo = xe.UsrReemplazo
FROM   Seguridad.Seg_RolUsuario c, OPENXML (@VL_idXML, '/ResultSet/Roles/Rol') WITH Seguridad.Seg_RolUsuario xe
WHERE c.IdUsuario = xe.IdUsuario AND c.IdRol = xe.IdRol
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Elimina las que no existen en xmlde RolUsuario
DELETE Seguridad.Seg_RolUsuario
FROM Seguridad.Seg_RolUsuario c
WHERE  c.IdUsuario = @VL_IdUsuario
	and not exists (Select 1
		    FROM OPENXML (@VL_idXML, '/ResultSet/Roles/Rol') WITH Seguridad.Seg_RolUsuario cx
			WHERE cx.IdRol = c.IdRol)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

-- Ingresa Autorizacion de Usuario Nuevas
insert into Seguridad.Seg_AutorizacionUsuario(IdUsuario,IdAutorizacion,NumAutorizacion,
		FechaInicio,FechaFin,Valor,IdUsuarioAutorizador)
	
	SELECT IdUsuario,IdAutorizacion, NumAutorizacion,
		FechaInicio, FechaFin, Valor,IdUsuarioAutorizador
FROM   OPENXML (@VL_idXML, '/ResultSet/Autorizaciones/Autorizacion') WITH Seguridad.Seg_AutorizacionUsuario
  WHERE idAutorizacion not in (Select a.idAutorizacion
		    FROM Seguridad.Seg_AutorizacionUsuario a
			WHERE a.IdUsuario = IdUsuario AND
			      a.IdAutorizacion = IdAutorizacion)
				
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Actualiza las Autorizaciones de Usuario existentes
UPDATE Seguridad.Seg_AutorizacionUsuario 
SET 	IdUsuario = ax.IdUsuario,
	IdAutorizacion = ax.IdAutorizacion,
	NumAutorizacion = ax.NumAutorizacion,
	FechaInicio = ax.FechaInicio,
	FechaFin = ax.FechaFin,
	Valor = ax.Valor,
	IdUsuarioAutorizador = ax.IdUsuarioAutorizador
FROM   Seguridad.Seg_AutorizacionUsuario au, OPENXML (@VL_idXML, '/ResultSet/Autorizaciones/Autorizacion') WITH Seguridad.Seg_AutorizacionUsuario ax
WHERE   au.IdUsuario = ax.IdUsuario AND
	au.IdAutorizacion = ax.IdAutorizacion

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Elimina las que no existen en xmlde TransUsuario
DELETE Seguridad.Seg_AutorizacionUsuario
FROM Seguridad.Seg_AutorizacionUsuario a
WHERE  a.IdUsuario = @VL_IdUsuario
	and not exists (Select 1
		    FROM OPENXML (@VL_idXML, '/ResultSet/Autorizaciones/Autorizacion') WITH Seguridad.Seg_AutorizacionUsuario
			WHERE a.IdAutorizacion = IdAutorizacion)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

-- Ingresa TransUsuario Nuevas
insert into Seguridad.Seg_TransUsuario(IdUsuario,IdTransaccion,IdOpcion,
		IdOrganizacion,IdHorario,Estado,
	FechaInicial,FechaFinal,
	TipoIdentificacion,IdIdentificacion,UsrReemplaza)
	SELECT IdUsuario,IdTransaccion,IdOpcion,
		IdOrganizacion,IdHorario,Estado,
	FechaInicial,FechaFinal,
	TipoIdentificacion,IdIdentificacion,UsrReemplaza
FROM   OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_TransUsuario cx
  WHERE not exists (Select IdUsuario
		    FROM Seguridad.Seg_TransUsuario c
			WHERE c.IdUsuario = cx.IdUsuario
				and c.IdTransaccion = cx.IdTransaccion
				and c.IdOpcion = cx.IdOpcion 
				and c.IdOrganizacion = cx.IdOrganizacion)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Actualiza las TransUsuario existentes
UPDATE Seguridad.Seg_TransUsuario
SET 	IdHorario = xe.IdHorario,
	Estado = xe.Estado,
  	FechaInicial = xe.FechaInicial,FechaFinal = xe.FechaFinal,
	TipoIdentificacion = xe.TipoIdentificacion,
	IdIdentificacion = xe.IdIdentificacion,
	UsrReemplaza = xe.UsrReemplaza
FROM   Seguridad.Seg_TransUsuario c, OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_TransUsuario xe
WHERE   xe.IdUsuario = c.IdUsuario 
	and xe.IdTransaccion = c.IdTransaccion
	and xe.IdOpcion = c.IdOpcion 
	and xe.IdOrganizacion = c.IdOrganizacion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Elimina las que no existen en xmlde TransUsuario
DELETE Seguridad.Seg_TransUsuario
FROM Seguridad.Seg_TransUsuario c
WHERE  c.IdUsuario = @VL_IdUsuario
	and not exists (Select 1
		    FROM OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_TransUsuario cx
			WHERE cx.IdTransaccion = c.IdTransaccion
			and cx.IdOpcion = c.IdOpcion 
			and cx.IdOrganizacion = c.IdOrganizacion)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END



COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML





