CREATE   VIEW [Participante].[Par_V_Empleado]
AS
	SELECT     e.IdParticipante, p.IdUsuario, e.IdEmpresa, p.Identificacion, ISNULL(pe.Apellido1, '') + ' ' + ISNULL(pe.Apellido2, '') + ' ' + ISNULL(pe.Nombre1, '') 
                      + ' ' + ISNULL(pe.Nombre2, '') AS Nombre, e.IdCargo, Catalogo.Ctl_F_conCatalogo(207, e.IdCargo) AS Cargo, e.IdOficina, 
                      Participante.Par_F_getNombreParticipante(e.IdOficina) AS Oficina, e.Estado, 'P' AS TipoParticipante,
		   isnull((
		    select top 1 valor  from participante.par_medioContacto mc
		    where e.IdParticipante = mc.IdParticipante  	  
		    and mc.idTipoMedioContacto = 3 
		   ),'') Correo

FROM         Participante.Par_Empleado AS e INNER JOIN
                      Participante.Par_Persona AS pe ON e.IdParticipante = pe.IdParticipante INNER JOIN
                      Participante.Par_Participante AS p ON pe.IdParticipante = p.IdParticipante









