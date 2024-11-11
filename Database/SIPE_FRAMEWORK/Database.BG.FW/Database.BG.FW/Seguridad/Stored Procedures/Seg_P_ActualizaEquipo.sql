


Create Proc [Seguridad].[Seg_P_ActualizaEquipo]
@PI_Codigo	int,
@PI_Nombre varchar(100)
AS
	UPDATE Seguridad.Seg_Equipos
	SET Nombre = @PI_Nombre
	WHERE CodigoEquipo = @PI_Codigo







