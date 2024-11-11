


create  view [Seguridad].[Seg_v_usuariorol] as
select distinct ru.IdUsuario, u.Nombre, u.FechaExpira
 from Seguridad.Seg_RolUsuario ru, Seguridad.Seg_V_Usuario u
where  ru.IdUsuario=u.IdUsuario 






