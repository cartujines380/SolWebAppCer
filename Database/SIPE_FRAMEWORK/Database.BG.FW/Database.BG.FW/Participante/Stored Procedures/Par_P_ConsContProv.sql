CREATE PROCEDURE [Participante].[Par_P_ConsContProv]
@PI_IdParticipante int
as
-- Información General 
SELECT	p.IdParticipante,Catalogo.Ctl_F_conCatalogo(7,p.IdTipoIdentificacion) as TipoIdentificacion,
		p.Identificacion, p.IdUsuario as Alias, pe.Nombre1,pe.Nombre2,pe.Apellido1,
		pe.Apellido2
	FROM Participante.Par_Contacto co INNER JOIN Participante.Par_Persona pe 
				on co.IdPartContacto = pe.IdParticipante INNER JOIN Participante.Par_Participante p
				on pe.IdParticipante = p.IdParticipante
	WHERE    co.IdParticipante = @PI_IdParticipante

-- Información de sus contactos
SELECT mc.IdParticipante,Catalogo.Ctl_F_conCatalogo(10,mc.IdTipoMedioContacto) as Medio,
		mc.Valor
FROM Participante.Par_Contacto co INNER JOIN Participante.Par_Persona pe 
				on co.IdPartContacto = pe.IdParticipante INNER JOIN Participante.Par_Direccion di
				on pe.IdParticipante = di.IdParticipante INNER JOIN Participante.Par_MedioContacto mc
				on di.IdParticipante = mc.IdParticipante
	WHERE   co.IdParticipante = @PI_IdParticipante
		




