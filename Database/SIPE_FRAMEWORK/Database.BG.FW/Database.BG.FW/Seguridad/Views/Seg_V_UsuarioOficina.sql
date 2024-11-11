CREATE VIEW [Seguridad].[Seg_V_UsuarioOficina]
AS
	SELECT ru.IdUsuario, r.IdEmpresa, r.IdSucursal as idOficina, r.IdRol, r.Descripcion as Rol
	FROM Seguridad.Seg_Rol r inner join Seguridad.Seg_rolUsuario ru on r.IdRol = ru.IdRol



