Create Proc [Catalogo].[Ctl_P_IngCatalogoDependiente]
@PV_xmlDOC varchar(8000)
AS
declare @VL_IdXML int, @VL_IdTablaHija int 
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDOC
--Obtengo el Id de la Tabla Hija
SELECT @VL_IdTablaHija = IdTabla
FROM OPENXML (@VL_IdXML, '/Usuario/TablaHija') WITH (IdTabla int)
--Controla el Error por Catalogo Existente
IF EXISTS(SELECT 1 
	FROM Catalogo.Ctl_Catalogo c, OPENXML (@VL_IdXML, '/Usuario/TablaPadre')
		WITH Catalogo.Ctl_Catalogo cx
	WHERE c.IdTabla = cx.IdTabla AND c.Codigo = cx.Codigo)
BEGIN
	RAISERROR(55003,16,1,'')--'Catalogo_Padre_ya_existe')
	RETURN
END
--Controla el Error si un Catalogo Hijo Nuevo ya Existe
--Están 1 exists dentro de otro exists xq daba error de OPENXML, aunque el select con 1 solo exists fuera del IF funciona bien
IF EXISTS( SELECT  1
	FROM Catalogo.Ctl_Catalogo c 
	WHERE c.IdTabla = @VL_IdTablaHija AND 
		EXISTS(SELECT 1 FROM OPENXML (@VL_IdXML, '/Usuario/TablaHija/Catalogo')
			WITH Catalogo.Ctl_Catalogo cx 
			WHERE c.Codigo = cx.Codigo AND c.DescAlterno <> cx.DescAlterno ))
BEGIN
	RAISERROR(55005,16,1,'')--'Catalogo_Hijo_ya_existe')
	RETURN
END
--Inicia la transaccion
BEGIN TRAN
--Ingresa el nueva Catalogo Padre
	INSERT INTO Catalogo.Ctl_Catalogo (IdTabla, Codigo, Descripcion, DescAlterno, Estado)
		SELECT IdTabla, Codigo, Descripcion, DescAlterno, Estado
		FROM OPENXML (@VL_IdXML, '/Usuario/TablaPadre') WITH Catalogo.Ctl_Catalogo
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
--Ingresa los Nuevos Catalogos Hijos
	insert into Catalogo.Ctl_Catalogo  ( IdTabla, Codigo, Descripcion, DescAlterno, Estado )
		SELECT @VL_IdTablaHija, Codigo, Descripcion, DescAlterno, Estado
	FROM   OPENXML (@VL_IdXML, '/Usuario/TablaHija/Catalogo') WITH Catalogo.Ctl_Catalogo
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
-- Finalizo la Transaccion
COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_IdXML




