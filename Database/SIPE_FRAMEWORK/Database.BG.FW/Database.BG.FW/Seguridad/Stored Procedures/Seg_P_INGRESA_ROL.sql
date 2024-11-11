
CREATE procedure [Seguridad].[Seg_P_INGRESA_ROL]
@PV_xmlDoc                 varchar(max)
AS
declare @VL_idXML int, @VL_NombreRol varchar(260)

   DECLARE @VL_sigIdRol           int
 

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDoc

--Recupero IdUsuario
SELECT @VL_NombreRol = Nombre
FROM OPENXML (@VL_idXML, '/ResultSet/Rol') WITH Seguridad.Seg_Rol

-- Verifica si el rol existe
IF EXISTS(SELECT 1 FROM Seguridad.Seg_Rol WHERE Nombre = @VL_NombreRol)
BEGIN
	RAISERROR(50003,16,1,'Rol')
	RETURN
END 

--Inicia la trnasaccion
BEGIN TRAN

    Select @VL_sigIdRol = ISNULL(max(idRol),0) + 1 
        From Seguridad.Seg_ROL 
        
    INSERT INTO Seguridad.Seg_ROL(IdRol,IdEmpresa,IdSucursal,Descripcion,Status, Nombre)
            SELECT @VL_sigIdRol,IdEmpresa,IdSucursal,Descripcion,Status, Nombre
	FROM OPENXML (@VL_idXML, '/ResultSet/Rol') WITH Seguridad.Seg_Rol
    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RAISERROR(51013,16,1)
	RETURN
    END

--Ingresa RolUsuario
Insert into Seguridad.Seg_OpcionTransRol(IdRol,IdTransaccion,IdOpcion,IdOrganizacion)
	SELECT @VL_sigIdRol,IdTransaccion,IdOpcion,IdOrganizacion
	 FROM   OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_OpcionTransRol

    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RAISERROR(51014,16,1)
	RETURN
    END

  COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML

--Retorno IdRol
select @VL_sigIdRol








