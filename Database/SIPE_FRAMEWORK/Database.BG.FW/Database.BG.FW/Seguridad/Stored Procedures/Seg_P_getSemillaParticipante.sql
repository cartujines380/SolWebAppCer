
CREATE Procedure [Seguridad].[Seg_P_getSemillaParticipante]
@IdParticipante int,
@IdUsuario varchar(50),
@FechaAct datetime,
@PO_Semilla varchar(100) output
AS
	IF @IdParticipante > 0 
		SELECT TOP 1 @PO_Semilla = Semilla
		FROM Seguridad.Seg_Semilla
		WHERE IdParticipante = @IdParticipante
				AND FechaAct < @FechaAct
		ORDER BY FechaAct DESC
	ELSE -- se envia el IdUsuario
		SELECT TOP 1 @PO_Semilla = Semilla
		FROM Participante.Par_RegistroCliente rc 
				INNER JOIN Seguridad.Seg_Semilla s
				ON rc.IdParticipante = s.IdParticipante
		WHERE rc.IdUsuario = @IdUsuario
				AND s.FechaAct < @FechaAct
		ORDER BY s.FechaAct DESC

