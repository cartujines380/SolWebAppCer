create    VIEW [Participante].[Par_V_Organigrama]
AS
	SELECT IdOrganigrama as Codigo, 
    	Descripcion, Nivel ,
    	IdOrganigramaPadre as "CODIGOPADRE",IdEmpresa
    FROM Participante.Par_Organigrama






