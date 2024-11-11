CREATE PROC [Catalogo].[Ctl_P_ActualizaTabla]
	@PV_xmlDoc varchar(max)
AS
DECLARE @VL_idXML int 
DECLARE @VL_Descripcion varchar(150) 
--Preparo el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PV_xmlDoc
--Inicio la transaccion
BEGIN TRAN
-- Uso de tablas temporales para los WITH
CREATE TABLE #T1(IdTbl int, Nom varchar(50) COLLATE DATABASE_DEFAULT, Des varchar(150) COLLATE DATABASE_DEFAULT) 
CREATE TABLE #T2(IdTbl int,Cod varchar(50) COLLATE DATABASE_DEFAULT, Des varchar(150) COLLATE DATABASE_DEFAULT, DesAlt varchar(150) COLLATE DATABASE_DEFAULT, Est char(1) COLLATE DATABASE_DEFAULT) -- COLLATE SQL_Latin1_General_CP1_CI_AS
--Verifico que la(s) Tabla(s) este(n) definida(s)
IF NOT EXISTS (SELECT 1 FROM OPENXML (@VL_idXML, '/Usuario/Tabla',1) WITH #T1 cx
			WHERE cx.IdTbl IN (SELECT t.IdTabla FROM Catalogo.Ctl_Tabla t))
BEGIN
	RAISERROR(55002,16,1,'')--'Catalogo_no_existe')
	RETURN
END 
--Actualizamos la(s) tabla(s)
UPDATE Catalogo.Ctl_Tabla
	SET Nombre = cx.Nom,
	    Descripcion = cx.Des
FROM Catalogo.Ctl_Tabla t, OPENXML (@VL_idXML, '/Usuario/Tabla',1) WITH #T1 cx
   WHERE t.IdTabla = cx.IdTbl
-- Ingreso los Catalogos  Nuevos de la(s) Tabla(s)
insert into Catalogo.Ctl_Catalogo(IdTabla,Codigo,Descripcion,DescAlterno,Estado)
	SELECT IdTbl,Cod,Des,DesAlt,Est
FROM   OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 cx
  WHERE not exists (Select 1
		    FROM Catalogo.Ctl_Catalogo c
			WHERE c.IdTabla = cx.IdTbl
				AND c.Codigo = cx.Cod)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
--Actualizo los existentes
UPDATE Catalogo.Ctl_Catalogo
SET 	Descripcion = cx.Des,
	DescAlterno = cx.DesAlt,
	Estado = cx.Est
FROM   Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 cx
WHERE c.IdTabla = cx.IdTbl
   AND c.Codigo = cx.Cod
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
-- Elimino los que no existen en xml de catalogo
SELECT TOP 1 @VL_Descripcion = c.Descripcion
FROM Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 c1
WHERE  c.IdTabla = c1.IdTbl
	AND c.Necesario = 1
	AND NOT EXISTS (Select 1
		    FROM OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 cx
			WHERE cx.IdTbl = c.IdTabla AND cx.Cod = c.Codigo)
IF (NOT @VL_Descripcion IS NULL AND @VL_Descripcion <> '')
BEGIN
	RAISERROR (55006,16,1,@VL_Descripcion)
	ROLLBACK TRAN
	RETURN
END
ELSE
BEGIN
	DELETE Catalogo.Ctl_Catalogo
	FROM Catalogo.Ctl_Catalogo c, OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 c1
	WHERE  c.IdTabla = c1.IdTbl
		and not exists (Select 1
				FROM OPENXML (@VL_idXML, '/Usuario/Tabla/Catalogo') WITH #T2 cx
				WHERE cx.IdTbl = c.IdTabla AND cx.Cod = c.Codigo)
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
END
-- Ellimino las Tablas temporales
DROP TABLE #T1
DROP TABLE #T2
--Finalizo la transaccion
COMMIT TRAN
--Libero el documento XML
EXEC sp_xml_removedocument @VL_idXML





