

CREATE    VIEW [Seguridad].[SEG_V_RolAsignables] AS
SELECT IdRol, Nombre, Descripcion, IdEmpresa, IdSucursal
FROM Seguridad.Seg_ROL 
WHERE IdRol NOT IN (1,2,4)





