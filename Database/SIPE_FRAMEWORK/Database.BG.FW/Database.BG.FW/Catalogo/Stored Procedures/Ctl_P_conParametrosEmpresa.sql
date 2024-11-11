create PROCEDURE [Catalogo].[Ctl_P_conParametrosEmpresa]
@IdEmpresa int, @IdParametro int
AS
	SELECT Nombre, Valor1, Valor2, Estado 
	  FROM Catalogo.Ctl_ParametrosEmpresa 
	  WHERE IdEmpresa = @IdEmpresa AND IdParametro = @IdParametro 




