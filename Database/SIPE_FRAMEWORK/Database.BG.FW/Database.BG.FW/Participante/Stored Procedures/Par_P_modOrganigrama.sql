CREATE PROCEDURE [Participante].[Par_P_modOrganigrama]
@PI_docXML as varchar(1000)

AS
declare @VL_idXML int, @VL_IdOrganigrama int, @VL_IdEmpresa int, @VL_Descripcion varchar(100)

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo IdParticipante
Select @VL_IdOrganigrama = IdOrganigrama, @VL_IdEmpresa = IdEmpresa, @VL_Descripcion = Descripcion
FROM OPENXML (@VL_idXML, '/Usuario/ResultSet/Organigrama') WITH Participante.Par_Organigrama

--Pregunto si existe para ser modificado
IF NOT EXISTS(SELECT 1 FROM Participante.Par_Organigrama 
		WHERE IdOrganigrama = @VL_IdOrganigrama and IdEmpresa = @VL_IdEmpresa)
BEGIN
	raiserror (52151,16,1,@VL_IdOrganigrama,@VL_IdEmpresa)	
	return
END

IF EXISTS(SELECT 1 FROM Participante.Par_Organigrama 
	WHERE Descripcion = @VL_Descripcion AND IdEmpresa = @VL_IdEmpresa and IdOrganigrama <> @VL_IdOrganigrama)
BEGIN
	raiserror (52150,16,1,@VL_Descripcion,@VL_IdEmpresa)	
	return
END


--Ingresa Categoria Padre
UPDATE Participante.Par_Organigrama
SET Descripcion = xc.Descripcion,
	IdOrganigramaPadre = xc.IdOrganigramaPadre,
	Nivel = xc.Nivel, IdEmpleado = xc.IdEmpleado
FROM Participante.Par_Organigrama c,  OPENXML (@VL_idXML, '/Usuario/ResultSet/Organigrama') WITH Participante.Par_Organigrama xc
WHERE c.IdOrganigrama = xc.IdOrganigrama and c.IdEmpresa = xc.IdEmpresa
IF ( @@error <> 0 ) 
BEGIN 
	raiserror (52155,16,1)	
	RETURN  
END 

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML




