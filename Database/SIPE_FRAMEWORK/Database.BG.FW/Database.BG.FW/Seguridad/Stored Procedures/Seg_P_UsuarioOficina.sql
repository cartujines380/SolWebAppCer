CREATE PROCEDURE [Seguridad].[Seg_P_UsuarioOficina]
@PI_IdEmpresa int,
@PI_IdOficina int
AS
	SELECT	distinct p.IdParticipante, u.IdUsuario, p.Identificacion, 
			p.IdTipoIdentificacion,
			Catalogo.Ctl_F_conCatalogo(7,p.IdTipoIdentificacion) as TipoIdentificacion,
			Participante.Par_F_getNombreUsuario(u.IdUsuario) as Nombre,
			p.FechaRegistro,
			u.IdRol, u.Rol
	FROM	Seguridad.Seg_V_UsuarioOficina u inner join  Participante.Par_Participante p on
			u.IdUsuario = p.IdUsuario
	WHERE	u.IdEmpresa = @PI_IdEmpresa
			and u.IdOficina = @PI_IdOficina

	





