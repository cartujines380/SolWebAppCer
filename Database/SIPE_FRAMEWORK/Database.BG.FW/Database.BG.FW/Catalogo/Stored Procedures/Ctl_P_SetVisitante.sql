create Procedure [Catalogo].[Ctl_P_SetVisitante]
@PO_Contador int output
AS
BEGIN TRAN
	UPDATE Catalogo.Ctl_Visitante
	SET Contador = Contador + 1 
	IF @@Error <> 0
		ROLLBACK
COMMIT

SELECT @PO_Contador = Contador FROM Catalogo.Ctl_Visitante




