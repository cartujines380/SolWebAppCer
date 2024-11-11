
CREATE  PROCEDURE [Seguridad].[Seg_P_conOrganizacion]
AS

SELECT IdOrganizacion,Descripcion 
FROM Seguridad.Seg_Organizacion
WHERE idorgpadre >= 0
ORDER BY Descripcion




