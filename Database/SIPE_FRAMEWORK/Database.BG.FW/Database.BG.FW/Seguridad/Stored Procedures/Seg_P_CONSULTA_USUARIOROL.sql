
CREATE procedure [Seguridad].[Seg_P_CONSULTA_USUARIOROL] 
@PV_Codusuario      VARCHAR( 20)
AS
   SELECT distinct ur.IdRol, r.IdEmpresa, r.IdSucursal as IdOficina,
				r.nombre as NombreRol,
                ur.IdHorario,  h.descripcion as "DesHorario",
                convert(varchar,ur.fechainicial,110)as  "FechaInicial", 
                convert(varchar,ur.fechafinal,110)as  "FechaFinal",
                ur.Estado, 
                ur.TipoIdentificacion, 
                ur.IdIdentificacion,'' as DescIdentificacion,
                ur.UsrReemplazo, 
		Participante.Par_F_getNombreUsuario(ur.UsrReemplazo) as NomUserReemp
         FROM Seguridad.Seg_RolUsuario ur, Seguridad.Seg_Rol r,  Seguridad.Seg_Horario h
         WHERE ur.IdUsuario = @PV_Codusuario
               and ur.idrol = r.idrol 
               and ur.idhorario = h.idhorario 
  






