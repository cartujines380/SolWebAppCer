
CREATE procedure [Seguridad].[Seg_p_IngresaUsuarioTran]
	@PV_xmlUsuario                 varchar(max)
AS
declare @VL_idXML int, @VL_IdUsuario varchar(20)
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlUsuario

--Recupero IdUsuario
SELECT @VL_IdUsuario = IdUsuario
FROM OPENXML (@VL_idXML, '/ResultSet',1) WITH
		 (IdUsuario varchar(20) )

-- Verifica que el usuario este definido en seguridad
IF EXISTS(SELECT 1 FROM Seguridad.Seg_Usuario WHERE idUsuario = @VL_IdUsuario)
BEGIN
	RAISERROR(50003,16,1,'Usuario')
	RETURN
END 

--Inicia la trnasaccion
BEGIN TRAN
/*--Ingresa Usuario
insert into Seguridad.Seg_usuario(IdUsuario) Values(@VL_IdUsuario)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
*/

--Ingresa RolUsuario
Insert into Seguridad.Seg_RolUsuario 
	SELECT * FROM   OPENXML (@VL_idXML, '/ResultSet/Roles/Rol') WITH Seguridad.Seg_RolUsuario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
-- Ingresa AutorizacionesUsuarios
Insert into Seguridad.Seg_AutorizacionUsuario 
	SELECT * FROM   OPENXML (@VL_idXML, '/ResultSet/Autorizaciones/Autorizacion') WITH Seguridad.Seg_AutorizacionUsuario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
-- Ingresa AutorizacionesUsuarios
Insert into Seguridad.Seg_TransUsuario 
	SELECT * FROM   OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_TransUsuario
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

  COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML









