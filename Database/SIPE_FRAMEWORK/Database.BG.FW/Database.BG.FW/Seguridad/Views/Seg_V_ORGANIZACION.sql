
CREATE  VIEW [Seguridad].[Seg_V_ORGANIZACION] AS
SELECT Org.IDORGANIZACION as Codigo,
    Org.DESCRIPCION,
    Apli.Nombre as Aplicacion,
    Org.IDORGPADRE as "CODIGOPADRE",
    Org.NIVEL
FROM Seguridad.Seg_ORGANIZACION Org, Seguridad.Seg_Aplicacion Apli
WHERE Apli.Idaplicacion = Org.Idaplicacion






