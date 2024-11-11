
CREATE Procedure [Seguridad].[Seg_P_consDiasFeriados]
@IdEmpresa int
AS
	SELECT IdEmpresa,IdSucursal, Dia
	FROM Seguridad.Seg_DiasFeriados
	WHERE IdEmpresa = @IdEmpresa 
