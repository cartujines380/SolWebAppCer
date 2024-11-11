

CREATE procedure [Seguridad].[Seg_P_conUsuarioRolReemp]
@PV_IdUsuario      VARCHAR(20 )
AS
         SELECT distinct r.IdRol, r.Descripcion, ur2.IdUsuario as UserReemp,
		convert(varchar,ur.FechaInicial,110) as FechaInicial,
		convert(varchar,ur.FechaFinal,110) as FechaFinal
         FROM Seguridad.Seg_RolUsuario ur left outer join Seguridad.Seg_RolUsuario ur2
				ON ur.IdRol = ur2.IdRol INNER JOIN  Seguridad.Seg_Rol r
				ON r.IdRol = ur.IdRol
         WHERE ur.IdUsuario = @PV_IdUsuario
	       AND ur2.UsrReemplazo = @PV_IdUsuario
               
  





