CREATE VIEW [Participante].[Par_V_UsuariosPases]
AS
	SELECT distinct rc.IdUsuario, p.Apellido1 + ' ' + p.Apellido2 + ' ' + p.Nombre1 + ' ' + p.Nombre2 as Nombre
	FROM Participante.Par_RegistroCliente rc 
		INNER JOIN Participante.Par_Persona p
			ON rc.IdParticipante = p.IdParticipante
		INNER JOIN Seguridad.Seg_RolUsuario ru
			ON rc.IdUsuario = ru.IdUsuario
		INNER JOIN Seguridad.Seg_OpcionTransRol otr
			ON ru.IdRol = otr.IdRol
	WHERE otr.IdOrganizacion = 175 AND otr.IdTransaccion = 2000
