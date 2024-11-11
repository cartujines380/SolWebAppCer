CREATE VIEW [Participante].[Par_V_Banco]
AS
	SELECT IdParticipante , Nombre
	FROM Participante.Par_Empresa
	WHERE IdCategoriaEmpresa in (7,8)






