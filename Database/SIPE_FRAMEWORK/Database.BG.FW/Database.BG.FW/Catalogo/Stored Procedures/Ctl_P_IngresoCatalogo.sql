Create Proc [Catalogo].[Ctl_P_IngresoCatalogo]
@PI_docXML varchar(3000)
AS
declare @idXML int
declare @IdTabla int
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @idXML OUTPUT, @PI_docXML
--Inicia la trnasaccion
BEGIN TRAN
--Recupera la ultima trnasaccion
Select @IdTabla = isnull(max(IdTabla),0) + 1
FROM Catalogo.Ctl_Catalogo
--Ingresa Transaccion
insert into Catalogo.Ctl_Catalogo  ( IdTabla, Codigo, Descripcion, DescAlterno, estado )
	SELECT convert(int,IdTabla), Codigo, Descripcion, DescAlterno, convert(char,estado)
FROM   OPENXML (@idXML, '/ResultSet/Catalogos/Catalogo') WITH Catalogo.Ctl_Catalogo
IF (@@error <> 0)
  ROLLBACK TRAN
--Ingresa por cada tabla lo mismo
SELECT @IdTabla
  COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @idXML




