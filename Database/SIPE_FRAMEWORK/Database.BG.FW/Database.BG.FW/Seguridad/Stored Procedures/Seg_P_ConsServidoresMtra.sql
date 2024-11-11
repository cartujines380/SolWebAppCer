CREATE Procedure [Seguridad].[Seg_P_ConsServidoresMtra]

AS
	SELECT *--IdAplicacion, Nombre
	FROM Seguridad.Seg_Aplicacion
	WHERE TipoServidor in (5,7)


