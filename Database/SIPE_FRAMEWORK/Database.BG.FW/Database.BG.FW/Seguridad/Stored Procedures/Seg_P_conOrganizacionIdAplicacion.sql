
CREATE Procedure [Seguridad].[Seg_P_conOrganizacionIdAplicacion]
	(
		@PI_IdAplicacion int 
	)

AS

SELECT IdOrganizacion,Descripcion 
FROM Seguridad.Seg_Organizacion
WHERE IdAplicacion = @PI_IdAplicacion
ORDER BY Descripcion


