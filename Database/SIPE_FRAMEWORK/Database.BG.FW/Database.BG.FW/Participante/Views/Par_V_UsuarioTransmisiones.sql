CREATE   VIEW [Participante].[Par_V_UsuarioTransmisiones]
AS
	SELECT     p.IdParticipante, p.IdUsuario, p.Identificacion, ISNULL(pe.Apellido1, '') + ' ' + ISNULL(pe.Apellido2, '') + ' ' + ISNULL(pe.Nombre1, '') 
                      + ' ' + ISNULL(pe.Nombre2, '') AS Nombre,                       
		   isnull((
		    select top 1 valor  from participante.par_medioContacto mc
		    where p.IdParticipante = mc.IdParticipante  	  
		    and mc.idTipoMedioContacto = 3 
		   ),'') Correo

FROM         
                      Participante.Par_Persona AS pe  INNER JOIN
                      Participante.Par_Participante AS p ON pe.IdParticipante = p.IdParticipante
WHERE p.TipoPartRegistro = 'T'
