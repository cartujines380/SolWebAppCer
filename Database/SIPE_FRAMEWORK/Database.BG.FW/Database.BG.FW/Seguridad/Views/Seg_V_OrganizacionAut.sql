
CREATE  VIEW [Seguridad].[Seg_V_OrganizacionAut] AS
SELECT Org.IdOrganizacion,
    Org.Descripcion    
FROM Seguridad.Seg_ORGANIZACION Org, Seguridad.Seg_Aplicacion Apli
WHERE Apli.Idaplicacion = Org.Idaplicacion and Org.IdOrganizacion > 0







