Create Proc [Catalogo].[Ctl_P_ActualizaCatalogo]
@PI_docXML as varchar(3000)
AS
declare @VL_idXML int
declare @VL_IdTabla int
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML
--Inicia la transaccion
BEGIN TRAN
-- Obtengo IdTabla
Select @VL_IdTabla = convert(int,IdTabla)
FROM OPENXML (@VL_idXML, '/ResultSet/Tabla') WITH Catalogo.Ctl_Catalogo
--Modifica Catalogo
UPDATE Catalogo.Ctl_Catalogo
SET Descripcion = xp.Descripcion, DescAlterno = xp.DescAlterno, estado = convert(char,xp.estado)
FROM   Catalogo.Ctl_Catalogo p, OPENXML (@VL_idXML, '/ResultSet/Catalogos/Catalogo') WITH Catalogo.Ctl_Catalogo xp
WHERE p.IdTabla = @VL_IdTabla  and p.Codigo = xp.Codigo
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
-- Ingresa los Nuevos Catalogos
insert into Catalogo.Ctl_Catalogo  ( IdTabla, Codigo, Descripcion, DescAlterno, estado )
	SELECT @VL_IdTabla, Codigo, Descripcion, DescAlterno, convert(char,estado)
FROM   OPENXML (@VL_idXML, '/ResultSet/Catalogos/Catalogo') WITH Catalogo.Ctl_Catalogo
WHERE Codigo not in (Select Codigo
		FROM Catalogo.Ctl_Catalogo 
		WHERE IdTabla = @VL_IdTabla )
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
-- Elimina las que no existen en xml
DELETE Catalogo.Ctl_Catalogo
WHERE IdTabla = @VL_IdTabla 
	AND Codigo not in (Select Codigo
		FROM OPENXML (@VL_idXML, '/ResultSet/Catalogos/Catalogo') WITH Catalogo.Ctl_Catalogo
		)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML




