CREATE procedure [Participante].[Par_P_SigSecuencia]
@PI_IdTabla int,
@PO_IdSecuencia int output
AS
	SELECT @PO_IdSecuencia = isnull(IdSecuencia,0) + 1
	FROM Participante.Par_Secuencia 
	WHERE IdTabla = @PI_IdTabla
	
	UPDATE Participante.Par_Secuencia
	SET IdSecuencia = @PO_IdSecuencia
	WHERE IdTabla = @PI_IdTabla




