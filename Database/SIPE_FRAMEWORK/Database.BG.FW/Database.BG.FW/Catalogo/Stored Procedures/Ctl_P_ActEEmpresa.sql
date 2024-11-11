Create Proc [Catalogo].[Ctl_P_ActEEmpresa]
	@PI_xmlDoc varchar(8000)
AS
declare @VL_idXML int, @VL_IdModulo int, @VL_IdEmpresa int 
--Preparo el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_xmlDoc
--Obtengo los datos necesarios
SELECT @VL_IdModulo = IdModulo, @VL_IdEmpresa = IdEmpresa 
	FROM OPENXML (@VL_idXML, '/Usuario') 
		WITH ( IdModulo int, IdEmpresa int ) 
--Inicio la transaccion
BEGIN TRAN
--Ingresa los Nuevos Registros del Xml
insert into Catalogo.Ctl_EEmpresa( IdModulo, IdEmpresaPadre, IdEmpresaHija ) 
SELECT @VL_IdModulo, @VL_IdEmpresa, IdEmpresa 
	FROM OPENXML (@VL_idXML, '/Usuario/Empresa') WITH ( IdEmpresa int ) ex 
  	WHERE not exists ( Select 1 FROM Catalogo.Ctl_EEmpresa e 
				WHERE e.IdModulo = @VL_IdModulo 
				AND e.IdEmpresaPadre = @VL_IdEmpresa 
				AND e.IdEmpresaHija = ex.IdEmpresa ) 
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
-- Elimino los que No existen en el Xml
DELETE Catalogo.Ctl_EEmpresa 
WHERE Catalogo.Ctl_EEmpresa.IdModulo = @VL_IdModulo 
	AND Catalogo.Ctl_EEmpresa.IdEmpresaPadre = @VL_IdEmpresa 
	AND not exists ( 
		Select 1 FROM OPENXML (@VL_idXML, '/Usuario/Empresa') WITH ( IdEmpresa int ) ex 
			WHERE Catalogo.Ctl_EEmpresa.IdEmpresaHija = ex.IdEmpresa ) 
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
--Finalizo la transaccion
COMMIT TRAN
--Libero el documento XML
EXEC sp_xml_removedocument @VL_idXML





