

Create Proc [Seguridad].[Seg_P_IngresoEquipo]
@PI_Codigo	int,
@PI_Nombre varchar(100)
AS
	INSERT INTO Seguridad.Seg_Equipos(CodigoEquipo,Nombre)
	VALUES (@PI_Codigo,@PI_Nombre)







