
CREATE PROCEDURE [Seguridad].[Seg_P_ConsRolesOrganizacionMenu]
@PI_IdOrganizacion INT
AS
SELECT
	r.IdRol,
	r.Nombre,
	r.Descripcion,
	r.Status
FROM [Seguridad].[Seg_Rol] r
WHERE EXISTS(
	SELECT 1
	FROM [Seguridad].[Seg_OpcionTransRol] x
	WHERE x.IdRol = r.IdRol
		AND x.IdOrganizacion = @PI_IdOrganizacion
		--AND x.IdTransaccion = 2000
		AND x.IdTransaccion = 3000
		AND x.IdOpcion = 1
)
ORDER BY r.Nombre

