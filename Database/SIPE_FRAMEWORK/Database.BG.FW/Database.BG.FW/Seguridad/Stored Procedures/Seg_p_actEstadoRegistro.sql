
create procedure [Seguridad].[Seg_p_actEstadoRegistro]
@PV_xmlDoc                 varchar(3000)
AS
declare @VL_idXML int
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDoc

--Inicia la trnasaccion
BEGIN TRAN

--Ingresa RolUsuario
Update Seguridad.Seg_Registro
SET Estado = xr.Estado
FROM Seguridad.Seg_Registro r, 
     OPENXML (@VL_idXML, '/Usuario//ResultSet/Registros/Registro') WITH Seguridad.Seg_Registro xr
WHERE r.IdUsuario = xr.IdUsuario
and r.Token = xr.Token

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

  COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML







