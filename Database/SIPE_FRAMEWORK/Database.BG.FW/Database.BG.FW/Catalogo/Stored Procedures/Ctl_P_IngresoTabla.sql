Create Proc [Catalogo].[Ctl_P_IngresoTabla]
@PV_xmlDOC varchar(3000) --, @PO_IdTabla int output

AS

declare @VL_IdXML int, @VL_Nombre varchar(50) 
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDOC
--Verifica que no exista ese nombre de tabla
SELECT @VL_Nombre = Nombre
FROM OPENXML (@VL_IdXML, '/ResultSet/Tabla') WITH Catalogo.Ctl_Tabla
--Controla el Error por nombre de Tabla existente
IF EXISTS(SELECT 1 FROM Catalogo.Ctl_Tabla WHERE Nombre = @VL_Nombre)
BEGIN
	RAISERROR(50003,16,1,'Tabla')
	RETURN
END
--Inicia la transaccion
BEGIN TRAN
--Obtiene el siguiente secuencial
	--exec Catalogo.Ctl_P_SigSecuencia
		--1, -- Para la Tabla
		--@PO_IdTabla output
--Ingresa la nueva Tabla
	INSERT INTO Catalogo.Ctl_Tabla (IdTabla,Nombre,Descripcion)
		SELECT IdTabla,Nombre,Descripcion
		FROM OPENXML (@VL_IdXML, '/ResultSet/Tabla') WITH Catalogo.Ctl_Tabla
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
--Ingresa los Catalogos
	insert into Catalogo.Ctl_Catalogo  ( IdTabla, Codigo, Descripcion, DescAlterno, Estado )
		SELECT IdTabla, Codigo, Descripcion, DescAlterno, Estado
	FROM   OPENXML (@VL_IdXML, '/ResultSet/Catalogos/Catalogo') WITH Catalogo.Ctl_Catalogo
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_IdXML





