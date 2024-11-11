

CREATE procedure [Seguridad].[Seg_P_ACTUALIZA_ROL]
@PV_xmlDoc                 varchar(max)
AS
declare @VL_idXML int, @VL_IdRol int

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDoc

--Recupero IdRol
SELECT @VL_IdRol = IdRol
FROM OPENXML (@VL_idXML, '/ResultSet/Rol') WITH Seguridad.Seg_Rol

-- Verifica si el rol no existe
IF NOT EXISTS(SELECT 1 FROM Seguridad.Seg_Rol WHERE IdRol = @VL_IdRol)
BEGIN
	RAISERROR(50001,16,1,'Rol')
	RETURN
END 

--Inicia la trnasaccion
BEGIN TRAN

       
    UPDATE Seguridad.Seg_ROL
	SET IdEmpresa = xr.IdEmpresa, IdSucursal = xr.IdSucursal,
	Descripcion = xr.Descripcion,
	Status = xr.Status, Nombre = xr.Nombre
    FROM 	Seguridad.Seg_ROL r, OPENXML (@VL_idXML, '/ResultSet/Rol') WITH Seguridad.Seg_Rol xr
    WHERE r.IdRol = xr.IdRol

    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END

-- Elimina las que no existen
DELETE Seguridad.Seg_OpcionTransRol
FROM Seguridad.Seg_OpcionTransRol c
WHERE  c.IdRol = @VL_IdRol
	and not exists (Select 1
		    FROM OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_OpcionTransRol cx
			WHERE cx.Idtransaccion = c.Idtransaccion
			AND cx.IdOpcion = c.IdOpcion
			AND cx.IdOrganizacion = c.IdOrganizacion)
    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END
--Ingresa nuevas
Insert into Seguridad.Seg_OpcionTransRol(IdRol,IdTransaccion,IdOpcion,IdOrganizacion)
	SELECT IdRol,IdTransaccion,IdOpcion,IdOrganizacion
	 FROM   OPENXML (@VL_idXML, '/ResultSet/Transacciones/Transaccion') WITH Seguridad.Seg_OpcionTransRol cx
	  WHERE not exists (Select 1
		    FROM Seguridad.Seg_OpcionTransRol c
			WHERE cx.IdRol = c.IdRol
			AND cx.Idtransaccion = c.Idtransaccion
			AND cx.IdOpcion = c.IdOpcion
			AND cx.IdOrganizacion = c.IdOrganizacion)
    IF (@@error <> 0)
    BEGIN  
	ROLLBACK TRAN
	RETURN
    END

  COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML
 






