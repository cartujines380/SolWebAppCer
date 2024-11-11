CREATE PROCEDURE [Participante].[Par_P_ConContacto]
@PI_IdParticipante int
AS

	SELECT 	Participante.Par_F_getNombreParticipante(c.IdPartContacto) as Nombre,
		c.IdPartContacto,
		c.IdTipoContacto, 
		Catalogo.Ctl_F_conCatalogo(11,c.IdTipoContacto) as TipoContacto,
		p.Identificacion, p.IdTipoIdentificacion,
		Catalogo.Ctl_F_conCatalogo(7,p.IdTipoIdentificacion) as TipoIdentificacion
	FROM Participante.Par_Participante p, Participante.Par_Contacto c
	WHERE c.IdParticipante = @PI_IdParticipante
			and c.IdPartContacto = p.IdParticipante






