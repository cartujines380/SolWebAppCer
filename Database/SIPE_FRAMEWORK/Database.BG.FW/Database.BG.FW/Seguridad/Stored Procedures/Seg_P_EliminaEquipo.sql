


Create Proc [Seguridad].[Seg_P_EliminaEquipo]
@PI_Codigo	int
AS
	DELETE Seguridad.Seg_Equipos
	WHERE CodigoEquipo = @PI_Codigo







