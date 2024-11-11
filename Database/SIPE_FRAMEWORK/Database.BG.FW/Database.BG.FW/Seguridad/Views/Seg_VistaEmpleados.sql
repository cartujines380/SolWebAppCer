


  CREATE View  [Seguridad].[Seg_VistaEmpleados] AS
  Select *  From
  (
  Select p.Apellido1, p.Apellido2, p.Nombre1, p.Nombre2, ro.IdEmpresa, r.IdParticipante, p.Ruc, r.IdTipoLogin, ru.IdRol, ro.Descripcion DescripcionRol, par.Identificacion NumIdent, par.TipoParticipante, ru.IdUsuario, pc.Valor correo
  From Participante.Par_Persona p
  Inner Join Participante.Par_RegistroCliente r on p.IdParticipante = r.IdParticipante
  Inner join Participante.Par_Participante par on par.IdParticipante=p.IdParticipante
  Inner join Participante.Par_MedioContacto pc on pc.IdParticipante=p.IdParticipante
  Inner Join Seguridad.Seg_RolUsuario ru on r.IdUsuario = ru.IdUsuario
  Inner Join Seguridad.Seg_Rol ro on ro.IdRol = ru.IdRol 
  and ru.IdRol in (28,29,30) and pc.IdTipoMedioContacto=3
  ) pt
  PIVOT ( Count(  IdRol ) FOR IdRol IN ([28],[29],[30] )) p;

