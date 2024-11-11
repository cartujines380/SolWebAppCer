CREATE   PROCEDURE [Participante].[Par_P_eliOrganigrama]
@PI_IdOrganigrama int,
@PI_IdEmpresa int

AS

if exists(SELECT 1 From Participante.Par_Organigrama Where IdOrganigramaPadre = @PI_IdOrganigrama and IdEmpresa = @PI_IdEmpresa)
	BEGIN
		raiserror (52152,16,1,@PI_IdOrganigrama)
		return
	END
--Pregunto si existe para ser eliminar
IF NOT EXISTS(SELECT 1 FROM Participante.Par_Organigrama WHERE IdOrganigrama = @PI_IdOrganigrama and IdEmpresa = @PI_IdEmpresa)
BEGIN
	raiserror (52151,16,1,@PI_IdOrganigrama,@PI_IdEmpresa)	
	return
END

--Elimina Categoria
DELETE Participante.Par_Organigrama
 FROM Participante.Par_Organigrama 
WHERE IdOrganigrama = @PI_IdOrganigrama and IdEmpresa = @PI_IdEmpresa
IF ( @@error <> 0 ) 
BEGIN 
	raiserror (52154,16,1)
	RETURN  
END 





