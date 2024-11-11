
CREATE VIEW [Participante].[Par_V_UsuariosGestion]
AS
	SELECT DISTINCT rc.IdUsuario, p.Apellido1 + ' ' + p.Apellido2 + ' ' + p.Nombre1 + ' ' + p.Nombre2 as Nombre, otr.IdOrganizacion
	FROM Participante.Par_RegistroCliente rc 
		INNER JOIN Participante.Par_Persona p
			ON rc.IdParticipante = p.IdParticipante
		INNER JOIN Seguridad.Seg_RolUsuario ru
			ON rc.IdUsuario = ru.IdUsuario
		INNER JOIN Seguridad.Seg_OpcionTransRol otr
			ON ru.IdRol = otr.IdRol
	WHERE otr.IdTransaccion = 2000
	 --solo roles del sistemas de Cobranza

