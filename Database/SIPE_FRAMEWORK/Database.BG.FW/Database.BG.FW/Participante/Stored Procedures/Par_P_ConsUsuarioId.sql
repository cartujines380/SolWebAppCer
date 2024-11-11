
CREATE Procedure [Participante].[Par_P_ConsUsuarioId]
@IdUsuario varchar(50)
AS
	DECLARE @IdParticipante int
	SELECT @IdParticipante = IdPArticipante
	FROM Participante.Par_RegistroCliente WHERE IdUsuario = @IdUsuario
	
	SELECT *
	FROM Participante.Par_F_MCParticipante(@IdParticipante,1,3)


