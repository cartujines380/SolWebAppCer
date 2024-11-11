CREATE procedure [Catalogo].[Ctl_P_SigSecuencia]
@PI_IdTabla int,
@PO_IdSecuencia int output
AS
	SELECT @PO_IdSecuencia = isnull(IdSecuencia,0) + 1
	FROM Catalogo.Ctl_Secuencia 
	WHERE IdTabla = @PI_IdTabla
	
	UPDATE Catalogo.Ctl_Secuencia
	SET IdSecuencia = @PO_IdSecuencia
	WHERE IdTabla = @PI_IdTabla





