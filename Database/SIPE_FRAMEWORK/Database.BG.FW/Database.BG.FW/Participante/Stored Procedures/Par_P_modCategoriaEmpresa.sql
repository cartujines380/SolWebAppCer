CREATE   procedure [Participante].[Par_P_modCategoriaEmpresa]
@PI_docXML as varchar(1000)
AS
declare @VL_idXML int, @VL_IdCategoria int, @VL_Descripcion varchar(100), @VL_IdCategoriaEmpPadre int

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo IdParticipante
Select  @VL_IdCategoria = IdCategoriaEmpresa,
	@VL_Descripcion = Descripcion,
	@VL_IdCategoriaEmpPadre = IdCategoriaEmpPadre
FROM OPENXML (@VL_idXML, '/Usuario/ResultSet/Categoria') WITH Participante.Par_CategoriaEmpresa

--Pregunto si existe para ser modificado
IF NOT EXISTS(SELECT 1 FROM Participante.Par_CategoriaEmpresa WHERE IdCategoriaEmpresa = @VL_IdCategoria)
BEGIN
	raiserror (52101,16,1)	
	return
END
if exists( SELECT 1 FROM Participante.Par_CategoriaEmpresa 
	WHERE IdCategoriaEmpPadre = @VL_IdCategoriaEmpPadre and Descripcion = @VL_Descripcion
		and IdCategoriaEmpresa <> @VL_IdCategoria )
BEGIN
	raiserror (52100,16,1,@VL_Descripcion)	
	return
END


--Ingresa Categoria Padre
UPDATE Participante.Par_CategoriaEmpresa
 SET Descripcion = xc.Descripcion,
	IdCategoriaEmpPadre = xc.IdCategoriaEmpPadre,
	Nivel = xc.Nivel
FROM Participante.Par_CategoriaEmpresa c,  OPENXML (@VL_idXML, '/Usuario/ResultSet/Categoria') WITH Participante.Par_CategoriaEmpresa xc
WHERE c.IdCategoriaEmpresa = xc.IdCategoriaEmpresa

IF (@@error <> 0)
BEGIN
  	RAISERROR (52107,16,1)	
	RETURN
END 

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML





