CREATE   procedure [Participante].[Par_P_ingCategoriaEmpresa]
@PI_docXML as varchar(1000),
@PO_IdCategoria int output
AS
declare @VL_idXML int, @VL_Descripcion varchar(100)
declare @VL_IdCategoriaEmpPadre int
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo IdParticipante
Select  @VL_Descripcion = Descripcion,
	@VL_IdCategoriaEmpPadre = IdCategoriaEmpPadre
FROM OPENXML (@VL_idXML, '/Usuario/ResultSet/Categoria') WITH Participante.Par_CategoriaEmpresa

-- Vslida que el participante No exista
if exists( SELECT 1 FROM Participante.Par_CategoriaEmpresa 
	WHERE IdCategoriaEmpPadre = @VL_IdCategoriaEmpPadre and Descripcion = @VL_Descripcion)
BEGIN
	raiserror (52100,16,1,@VL_Descripcion)	
	return
END

--Trae siguiente secuencia 2 = Categoria Padre
	EXEC Participante.Par_P_SigSecuencia 
		3,
		@PO_IdCategoria output

--Ingresa Categoria Padre
insert into Participante.Par_CategoriaEmpresa(IdCategoriaEmpresa,Descripcion,IdCategoriaEmpPadre,Nivel)
	SELECT @PO_IdCategoria,Descripcion,IdCategoriaEmpPadre,Nivel
FROM   OPENXML (@VL_idXML, '/Usuario/ResultSet/Categoria') WITH Participante.Par_CategoriaEmpresa
IF (@@error <> 0)
BEGIN
  	RAISERROR (52105,16,1)	
	RETURN
END 

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML

