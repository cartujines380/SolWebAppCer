

CREATE    VIEW [Participante].[Par_V_Cliente]
AS
	SELECT     p.IdParticipante, c.IdEmpresa, p.IdUsuario, p.IdTipoIdentificacion, p.Identificacion, 'Persona' AS TipoParticipante, ISNULL(pe.Apellido1, '') 
                      + ' ' + ISNULL(pe.Apellido2, '') + ' ' + ISNULL(pe.Nombre1, '') + ' ' + ISNULL(pe.Nombre2, '') AS Nombre, c.Estado, pe.IdEmpRel AS IdEmpPadre, 
                      Participante.Par_F_getNombreParticipante(pe.IdEmpRel) AS NombreEmpPadre,
		   isnull((
		    select top 1 valor  from participante.par_medioContacto mc
		    where c.IdParticipante = mc.IdParticipante  	  
		    and mc.idTipoMedioContacto = 3 
		   ),'') Correo

FROM         Participante.Par_Cliente AS c INNER JOIN
                      Participante.Par_Persona AS pe ON c.IdParticipante = pe.IdParticipante INNER JOIN
                      Participante.Par_Participante AS p ON pe.IdParticipante = p.IdParticipante
UNION
SELECT     p.IdParticipante, c.IdEmpresa, p.IdUsuario, p.IdTipoIdentificacion, p.Identificacion, 'Empresa' AS TipoParticipante, e.Nombre, c.Estado, 
                      e.IdEmpresaPadre AS IdEmpPadre, Participante.Par_F_getNombreParticipante(e.IdEmpresaPadre) AS NombreEmpPadre,
		   isnull((
		    select top 1 valor  from participante.par_medioContacto mc
		    where c.IdParticipante = mc.IdParticipante  	  
		    and mc.idTipoMedioContacto = 3 
		   ),'') Correo
FROM         Participante.Par_Cliente AS c INNER JOIN
                      Participante.Par_Empresa AS e ON c.IdParticipante = e.IdParticipante INNER JOIN
                      Participante.Par_Participante AS p ON e.IdParticipante = p.IdParticipante









