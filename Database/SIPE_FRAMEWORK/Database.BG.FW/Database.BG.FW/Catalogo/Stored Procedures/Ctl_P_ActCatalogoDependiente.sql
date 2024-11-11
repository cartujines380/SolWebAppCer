Create Proc [Catalogo].[Ctl_P_ActCatalogoDependiente]
@PV_xmlDoc    varchar(8000)
AS
declare @VL_idXML int, @VL_IdTablaHija int 
--Preparo el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDoc
--Obtengo el Id de la Tabla Hija
SELECT @VL_IdTablaHija = IdTabla
FROM OPENXML (@VL_IdXML, '/Usuario/TablaHija') WITH Catalogo.Ctl_Catalogo
--Controla el Error si Catalogo Padre No Existe
IF NOT EXISTS(SELECT 1 
	FROM Catalogo.Ctl_Catalogo c, OPENXML (@VL_IdXML, '/Usuario/TablaPadre')
		WITH Catalogo.Ctl_Catalogo cx
	WHERE c.IdTabla = cx.IdTabla AND c.Codigo = cx.Codigo)
BEGIN
	RAISERROR(55004,16,1,'')--'Catalogo_Padre_no_existe')
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
--Inicio la transaccion
BEGIN TRAN
--Actualizo el Catalogo Padre
UPDATE Catalogo.Ctl_Catalogo
SET 	Descripcion = cx.Descripcion,
	DescAlterno = cx.DescAlterno,
	Estado = cx.Estado
FROM   Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/TablaPadre') WITH Catalogo.Ctl_Catalogo cx
WHERE c.IdTabla = cx.IdTabla
  AND c.Codigo = cx.Codigo
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
--Ingresa los Nuevos Catalogos Hijos
insert into Catalogo.Ctl_Catalogo(IdTabla,Codigo,Descripcion,DescAlterno,Estado)
	SELECT @VL_IdTablaHija,Codigo,Descripcion,DescAlterno,Estado
FROM   OPENXML (@VL_idXML, '/Usuario/TablaHija/Catalogo') WITH Catalogo.Ctl_Catalogo cx
  WHERE not exists (Select 1
		    FROM Catalogo.Ctl_Catalogo c
			WHERE c.IdTabla = @VL_IdTablaHija
				AND c.Codigo = cx.Codigo)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
--Actualizo los Catalogos Hijos Existentes
UPDATE Catalogo.Ctl_Catalogo
SET 	Descripcion = cx.Descripcion,
	DescAlterno = cx.DescAlterno,
	Estado = cx.Estado
FROM   Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/TablaHija/Catalogo') WITH Catalogo.Ctl_Catalogo cx
WHERE c.IdTabla = @VL_IdTablaHija
   AND c.Codigo = cx.Codigo
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
-- Elimino los que No existen en el Xml
DELETE Catalogo.Ctl_Catalogo
FROM Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/TablaHija/Catalogo') WITH Catalogo.Ctl_Catalogo c2 
WHERE  c.IdTabla = @VL_IdTablaHija AND c.DescAlterno = c2.DescAlterno
	AND not exists (Select 1
		    FROM OPENXML (@VL_idXML, '/Usuario/TablaHija/Catalogo') WITH Catalogo.Ctl_Catalogo cx
			WHERE @VL_IdTablaHija= c.IdTabla AND cx.Codigo = c.Codigo)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
--Finalizo la transaccion
COMMIT TRAN
--Libero el documento XML
EXEC sp_xml_removedocument @VL_idXML





